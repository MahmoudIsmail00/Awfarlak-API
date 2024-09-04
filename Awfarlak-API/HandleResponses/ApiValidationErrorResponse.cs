namespace Awfarlak_API.HandleResponses
{
    public class ApiValidationErrorResponse : ApiException
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }

        public IEnumerable<string> Errors { get; set; }
    }
}
