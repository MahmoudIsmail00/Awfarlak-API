namespace Awfarlak_API.HandleResponses
{
    public class ApiResponse
    {

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message;
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int code)
            => code switch
            {
                400 => "Bad Request",
                401 => "You are not authorized!!",
                404 => "resource not found",
                500 => "Internal Server Error",
                _ => null
            };

    }
}
