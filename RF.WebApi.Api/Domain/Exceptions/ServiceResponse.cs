namespace RF.WebApi.Api.Domain.Exceptions
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public void AddError(string message)
        {
            Success = false;
            Errors.Add(message);
        }

        public void SetErrors<TOther>(ServiceResponse<TOther> otherResponse)
        {
            this.Success = otherResponse.Success;
            if (otherResponse.Errors != null)
            {
                this.Errors.AddRange(otherResponse.Errors);
            }
        }

        public static async Task<ServiceResponse<T>> Execute(Func<ServiceResponse<T>, Task<T>> businessLogic)
        {
            var response = new ServiceResponse<T>();
            try
            {
                response.Data = await businessLogic(response);
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }

        public static ServiceResponse<T> ExecuteSync(Func<ServiceResponse<T>, T> businessLogic)
        {
            var response = new ServiceResponse<T>();
            try
            {
                response.Data = businessLogic(response);
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }

        private static void HandleException(ServiceResponse<T> response, Exception ex)
        {
            response.AddError(ex.Message);
            if (ex.InnerException != null)
                response.AddError(ex.InnerException.Message);
        }

        public object ToResult() => new
        {
            Success = this.Success,
            Data = this.Data,
            Message = string.Join(" | ", this.Errors)
        };
    }
}