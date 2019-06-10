namespace ApiCoreNHibernateCrudPagination.Dtos.Responses.Shared
{
    public class ErrorDtoResponse : AppResponse
    {
        public ErrorDtoResponse() : base(false)
        {
        }

        public ErrorDtoResponse(string message) : base(false, message)
        {
        }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}