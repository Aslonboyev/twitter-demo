namespace BlogApp.WebApi.Helpers
{
    public class HttpContextHelper
    {
        public static IHttpContextAccessor Accessor = null!;
        public static HttpResponse Response => Accessor.HttpContext.Response;

        public static IHeaderDictionary ResponseHeaders => Response.Headers;
    }
}
