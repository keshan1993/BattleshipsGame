namespace Battleships.Web.DataAccess.DTOs
{
    public class BaseResponse
    {
        public int? numCreatedID { get; set; }
        public bool? bitSuccess { get; set; }
        public ErrorDTO? Error { get; set; }
    }

    public class ErrorDTO
    {
        public string? ErrorType { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
