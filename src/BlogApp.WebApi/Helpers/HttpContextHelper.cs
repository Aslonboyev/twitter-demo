namespace BlogApp.WebApi.Helpers
{
    public class HttpContextHelper
    {
        public static IHttpContextAccessor Accessor { get; set; }
        public static HttpResponse Response => Accessor.HttpContext.Response;

        public static IHeaderDictionary ResponseHeaders => Response.Headers;
        
        public static HttpContext HttpContext => Accessor?.HttpContext;
        public static long? UserId => GetUserId();
        public static string UserRole => HttpContext?.User.FindFirst("UserRole")?.Value;

        private static long? GetUserId()
        {
            long id;    
            bool canParse = long.TryParse(HttpContext?.User?.Claims.FirstOrDefault(p => p.Type == "Id")?.Value, out id);
            return canParse ? id : null;
        }
    }
}
