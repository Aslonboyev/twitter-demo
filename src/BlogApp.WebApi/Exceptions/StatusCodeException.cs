using System.Net;

namespace BlogApp.WebApi.Exceptions
{
    public class StatusCodeException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public StatusCodeException(HttpStatusCode statusCode, string message) : base(message)
        {
            HttpStatusCode = statusCode;
        }
    }
}
