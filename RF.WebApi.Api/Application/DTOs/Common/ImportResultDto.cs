namespace RF.WebApi.Api.Application.DTOs.Common
{
    public class ImportRowMessageDto
    {
        public int RowNo { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ImportResultDto
    {
        public int SuccessCount { get; set; }
        public List<ImportRowMessageDto> Warnings { get; set; } = new();
        public List<ImportRowMessageDto> Errors { get; set; } = new();
    }
}
