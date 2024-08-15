using System.Net;
using DaemonMC.Utils.Text;

namespace DaemonMC.Network.RakNet
{
    public class RakSessionManager
    {
        public static Dictionary<IPEndPoint, RakSession> sessions = new Dictionary<IPEndPoint, RakSession>();

        public static void addSession(IPEndPoint ip, long guid)
        {
            if (sessions.TryGetValue(ip, out var session))
            {
                Log.warn($"Couldn't create session, {ip.Address.ToString()} with session GUID {session.GUID} already connected.");
                return;
            }
            sessions.Add(ip, new RakSession(guid));
        }

        public static RakSession getSession(IPEndPoint ip)
        {
            if (sessions.TryGetValue(ip, out var session))
            {
                return session;
            }
            else
            {
                Log.warn($"Session for {ip.Address.ToString()}");
            }
            return null;
        }

        public static void deleteSession(IPEndPoint ip)
        {
            if (!sessions.Remove(ip))
            {
                Log.warn($"Couldn't delete session for {ip.Address.ToString()}, session doesn't exist.");
                return;
            }
            else
            {
                Log.info($"{ip.Address.ToString()} Requested disconnect and got disconnected successfully.");
            }
        }

        public static void Compression(IPEndPoint ip, bool enable)
        {
            if (!sessions.TryGetValue(ip, out var session))
            {
                Log.warn($"Session doesn't exist for {ip.Address.ToString()}.");
                return;
            }
            sessions[ip].initCompression = enable;
        }
    }
}
