using System.Collections.Specialized;
using SimpleServer.Persisters;

namespace SimpleServer.RouteHandlers
{
    internal static class DefaultRouteHandler
    {
        public static string DefaultRoute(NameValueCollection v, IPersister p)
        {
            return "Hello World!";
        }
    }
}
