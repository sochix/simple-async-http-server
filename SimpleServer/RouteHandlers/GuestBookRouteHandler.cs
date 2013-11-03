using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using NLog;
using SimpleServer.Persisters;

namespace SimpleServer.RouteHandlers
{
    internal static class GuestBookRouteHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static string AddRecord(NameValueCollection v, IPersister p)
        {
            //TODO: we should add a validation for this params
            string user = String.IsNullOrEmpty(v["user"]) ? null : v["user"];
            string message = String.IsNullOrEmpty(v["message"]) ? null : v["message"];

            if (user != null && message != null)
            {
                p.AddRecord(new Tuple<string, string>(user, message));
                logger.Info("Adding guestbook record with user = {0} and message = {1}", user, message);
            }

            logger.Warn("Get request for adding empty user or message to guestbook");

            return "Post to questbook"; //TODO: say thank you to user
        }

        public static string GetAllRecords(NameValueCollection v, IPersister p)
        {
            var records = p.GetAllRecords();
            var sb = new StringBuilder();

            foreach (var record in records)
            {
                sb.AppendLine(record.ToString());
            }

            logger.Trace("Found {0} guestbook records", records.Count());

            return sb.ToString();
        }
    }
}
