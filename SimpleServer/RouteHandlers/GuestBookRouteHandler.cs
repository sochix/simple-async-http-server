using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using NLog;
using SimpleServer.Helpers;
using SimpleServer.Persisters;

namespace SimpleServer.RouteHandlers
{
    internal static class GuestBookRouteHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static string GetGuestbookRecordsView { get { return GuestBookRecordsView; } }
        public static string GetGuestbookThanksView { get { return GuestBookThanksView; } }
        
        public static readonly string GuestBookRecordsView;
        public static readonly string GuestBookThanksView;

        static GuestBookRouteHandler()
        {
            GuestBookRecordsView = File.ReadAllText(ServerHelpers.GuestBookRecordsViewPath);
            GuestBookThanksView = File.ReadAllText(ServerHelpers.GuestBookThanksViewPath);
        }

        public static string AddRecord(NameValueCollection v, IPersister p)
        {
            //TODO: we should add a validation for this params
            string user = String.IsNullOrEmpty(v["user"]) ? null : v["user"];
            string message = String.IsNullOrEmpty(v["message"]) ? null : v["message"];

            if (user != null && message != null)
            {
                p.AddRecord(new Tuple<string, string>(user, message));
                logger.Info("Adding guestbook record with user = {0} and message = {1}", user, message);
                return ServerHelpers.InjectDataToTheView(user, GuestBookThanksView);
            }

            logger.Warn("Get request for adding empty user or message to guestbook");
            return DefaultRouteHandler.GetDefaultResponseView;
        }

        public static string GetAllRecords(NameValueCollection v, IPersister p)
        {
            var records = p.GetAllRecords();
            logger.Trace("Found {0} guestbook records", records.Count());

            var sb = new StringBuilder();
            
            foreach (var record in records)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td>{0}</td><td>{1}</td>", record.Item1, record.Item2);
                sb.Append("</tr>");
            }
            return ServerHelpers.InjectDataToTheView(sb.ToString(), GetGuestbookRecordsView);
        }
    }
}
