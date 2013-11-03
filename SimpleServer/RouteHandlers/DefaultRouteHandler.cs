using System.Collections.Specialized;
using System.IO;
using SimpleServer.Helpers;
using SimpleServer.Persisters;

namespace SimpleServer.RouteHandlers
{
    internal static class DefaultRouteHandler
    {
        public static string GetDefaultResponseView { get { return DefaultResponsePage; } }
        
        public static readonly string DefaultResponsePage;

        static DefaultRouteHandler()
        {
            DefaultResponsePage = File.ReadAllText(ServerHelpers.DefaultResponseViewPath);            
        }
        
        public static string DefaultRoute(NameValueCollection v, IPersister p)
        {
            return DefaultResponsePage;
        }
    }
}
