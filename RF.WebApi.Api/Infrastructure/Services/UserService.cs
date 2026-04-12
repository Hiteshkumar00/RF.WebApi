using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Application.DTOs.User;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly RFDBContext _RFDBContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(RFDBContext context, IMapper mapper, IConfiguration configuration)
        {
            _RFDBContext = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<int>> CreateUser(CreateUserDto createUserDto)
        {
            return await ServiceResponse<int>.Execute(async err =>
            {
                if (createUserDto.Role == "SuperAdmin" && (!Token.IsSuperAdmin || Token.UserId != -1))
                {
                    err.AddError(UserMessages.UnauthorizedAction);
                    return default;
                }

                var isEmailTaken = _RFDBContext.Users.Any(u => u.Email == createUserDto.Email);
                if (isEmailTaken)
                {
                    err.AddError(string.Format(UserMessages.EmailExists, createUserDto.Email));
                    return default;
                }

                if (createUserDto.Role == "SuperAdmin")
                {
                    createUserDto.AccountId = null;
                }
                else if (createUserDto.Role == "Admin")
                {
                    if (createUserDto.AccountId == null || createUserDto.AccountId == 0)
                    {
                        err.AddError(UserMessages.AccountRequired);
                        return default;
                    }

                    var accountExists = await _RFDBContext.Accounts.AnyAsync(a => a.Id == createUserDto.AccountId);
                    if (!accountExists)
                    {
                        err.AddError(AccountMessages.NotFound);
                        return default;
                    }
                }

                var newUser = _mapper.Map<User>(createUserDto);

                newUser.Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

                _RFDBContext.Users.Add(newUser);
                int rowsAffected = await _RFDBContext.SaveChangesAsync();

                return newUser.Id ?? default;
            });
        }

        public Task<ServiceResponse<string>> LoginAsAdmin(int AccountId)
        {
            return ServiceResponse<string>.Execute(async err =>
            {
                if (!Token.IsSuperAdmin)
                {
                    err.AddError(UserMessages.OnlySuperAdminSwitch);
                    return default;
                }

                var accountExists = await _RFDBContext.Accounts.AnyAsync(a => a.Id == AccountId);
                if (!accountExists)
                {
                    err.AddError(UserMessages.TargetAccountNotFound);
                    return default;
                }

                var user = await _RFDBContext.Users.FindAsync(Token.UserId);

                var response = CreateToken(user, AccountId);
                if (!response.Success)
                {
                    err.SetErrors(response);
                    return default;
                }

                return response.Data;
            });
        }

        public Task<ServiceResponse<string>> LoginAsSuperAdmin()
        {
            return ServiceResponse<string>.Execute(async err =>
            {
                if (!Token.IsSuperAdmin)
                {
                    err.AddError(UserMessages.UnauthorizedAction);
                    return default;
                }

                var user = await _RFDBContext.Users.FindAsync(Token.UserId);
                if (user == null)
                {
                    err.AddError(UserMessages.NotFound);
                    return default;
                }

                var response = CreateToken(user, 0); // Passing 0 to ensure AccountId is 0 (Super Admin mode)
                if (!response.Success)
                {
                    err.SetErrors(response);
                    return default;
                }

                return response.Data;
            });
        }

        public Task<ServiceResponse<string>> Login(LoginUserDto loginDto)
        {
            return ServiceResponse<string>.Execute(async err =>
            {
                var user = await _RFDBContext.Users
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                {
                    err.AddError(UserMessages.InvalidLogin);
                    return string.Empty;
                }

                if (!user.IsActive)
                {
                    err.AddError(UserMessages.Deactivated);
                    return string.Empty;
                }

                var tokenResponse = CreateToken(user);

                if (!tokenResponse.Success)
                {
                    err.SetErrors(tokenResponse);
                    return string.Empty;
                }

                return tokenResponse.Data;
            });
        }

        private ServiceResponse<string> CreateToken(User user, int? targetAccountId = null)
        {
            return ServiceResponse<string>.ExecuteSync(err =>
            {
                var effectiveAccountId = targetAccountId ?? user.AccountId;
                string accountIdString = effectiveAccountId?.ToString() ?? "0";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.Role, user.Role ?? "Admin"),
                    new Claim("AccountId", accountIdString)
                };

                var keyString = _configuration.GetSection("Jwt:Key").Value;

                if (string.IsNullOrWhiteSpace(keyString) || keyString.Length < 64)
                {
                    err.AddError(UserMessages.JwtKeyError);
                    return default;
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = creds,
                    Issuer = _configuration.GetSection("Jwt:Issuer").Value,
                    Audience = _configuration.GetSection("Jwt:Audience").Value
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            });
        }

        public Task<ServiceResponse<bool>> DeleteUser(int Id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                if (Id <= 0)
                {
                    err.AddError(UserMessages.InvalidId);
                    return false;
                }

                var userToDelete = await _RFDBContext.Users.FindAsync(Id);

                if (userToDelete == null)
                {
                    err.AddError(UserMessages.NotFound);
                    return false;
                }

                if (userToDelete.Role == "SuperAdmin" && (!Token.IsSuperAdmin || Token.UserId != -1))
                {
                    err.AddError(UserMessages.UnauthorizedAction);
                    return false;
                }

                if (Id == Token.UserId)
                {
                    err.AddError(UserMessages.SelfDeleteProhibited);
                    return false;
                }

                await _RFDBContext.Users.Where(u => u.Id == Id).ExecuteDeleteAsync();

                return true;
            });
        }

        public Task<ServiceResponse<bool>> ActivateDeactivateUser(int Id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                if (Id <= 0)
                {
                    err.AddError(UserMessages.InvalidId);
                    return false;
                }

                var user = await _RFDBContext.Users.FindAsync(Id);

                if (user == null)
                {
                    err.AddError(UserMessages.NotFound);
                    return false;
                }

                if (user.Role == "SuperAdmin" && (!Token.IsSuperAdmin || Token.UserId != -1))
                {
                    err.AddError(UserMessages.UnauthorizedAction);
                    return false;
                }

                if (Id == Token.UserId)
                {
                    err.AddError(UserMessages.SelfDeactivateProhibited);
                    return false;
                }

                await _RFDBContext.Users
                    .Where(u => u.Id == Id)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsActive, !user.IsActive));

                return true;
            });
        }

        public Task<ServiceResponse<bool>> ResetPasswordBySuperAdmin(ResetPasswordBySuperAdminDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                if (dto.UserId <= 0)
                {
                    err.AddError(UserMessages.InvalidId);
                    return false;
                }

                var user = await _RFDBContext.Users.FindAsync(dto.UserId);

                if (user == null)
                {
                    err.AddError(UserMessages.NotFound);
                    return false;
                }

                if (user.Role == "SuperAdmin" && (!Token.IsSuperAdmin || Token.UserId != -1))
                {
                    err.AddError(UserMessages.UnauthorizedAction);
                    return false;
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

                await _RFDBContext.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<UserDto>> GetUserById(int Id)
        {
            return ServiceResponse<UserDto>.Execute(async err =>
            {
                if (Id <= 0)
                {
                    err.AddError(UserMessages.InvalidId);
                    return null;
                }

                var user = await _RFDBContext.Users.FindAsync(Id);

                if (user == null)
                {
                    err.AddError(UserMessages.NotFound);
                    return default;
                }

                if (user.Role == "SuperAdmin" && (!Token.IsSuperAdmin || Token.UserId != -1))
                {
                    err.AddError(UserMessages.UnauthorizedAction);
                    return default;
                }

                return _mapper.Map<UserDto>(user);
            });
        }


        public Task<ServiceResponse<bool>> UpdateUser(UserDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {

                if (dto.Role == "SuperAdmin" && (!Token.IsSuperAdmin || Token.UserId != -1))
                {
                    err.AddError(UserMessages.UnauthorizedAction);
                    return default;
                }

                if (dto.Role == "SuperAdmin")
                {
                    dto.AccountId = null;
                }
                else if (dto.Role == "Admin")
                {
                    if (dto.AccountId == null || dto.AccountId == 0)
                    {
                        err.AddError(UserMessages.AccountRequired);
                        return default;
                    }

                    var accountExists = await _RFDBContext.Accounts.AnyAsync(a => a.Id == dto.AccountId);
                    if (!accountExists)
                    {
                        err.AddError(AccountMessages.NotFound);
                        return default;
                    }
                }

                var user = await _RFDBContext.Users.FindAsync(dto.Id);
                if (user == null)
                {
                    err.AddError(UserMessages.NotFound);
                    return false;
                }

                _mapper.Map(dto, user);

                await _RFDBContext.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<List<UserDto>>> GetAllUsers()
        {
            return ServiceResponse<List<UserDto>>.Execute(async err =>
            {
                IQueryable<User> query = _RFDBContext.Users
                    .Include(u => u.Account)
                    .AsNoTracking();

                query = query.Where(u => u.Id > 0);

                if (!Token.IsSuperAdmin || Token.UserId != -1)
                {
                    query = query.Where(u => u.Role != "SuperAdmin");
                }

                var users = await query.ToListAsync();

                return _mapper.Map<List<UserDto>>(users);
            });
        }


    }
}
