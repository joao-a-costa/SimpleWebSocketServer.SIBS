using WindowsFirewallHelper;

namespace SimpleWebSocketServer.SIBS.Lib.Helper
{
    public static class FirewallHelper
    {
        /// <summary>
        /// Adds an inbound rule to the firewall for the specified application name and port.
        /// </summary>
        /// <param name="applicationName">The name of the application.</param>
        /// <param name="port">The port number.</param>
        /// <returns>The result of the operation.</returns>
        public static bool AddInboundRule(string applicationName, ushort port)
        {
            var firewall = FirewallManager.Instance;

            var rule = firewall.CreatePortRule(
                FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public,
                $"{applicationName}Inbound",
                FirewallAction.Allow,
                port,
                FirewallProtocol.TCP);
            rule.Direction = FirewallDirection.Inbound;
            FirewallManager.Instance.Rules.Add(rule);

            rule = firewall.CreatePortRule(
                FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public,
                $"{applicationName}Outbound",
                FirewallAction.Allow,
                port,
                FirewallProtocol.TCP);
            rule.Direction = FirewallDirection.Outbound;
            FirewallManager.Instance.Rules.Add(rule);

            return true;
        }

        /// <summary>
        /// Extracts the IP and port from the specified input string.
        /// </summary>
        /// <param name="input">The input string containing the IP and port.</param>
        /// <returns>The IP address and port number.</returns>
        public static (string ip, int port) GetIpAndPort(string input)
        {
            // Extract the part after "http://"
            string address = input.Substring(input.IndexOf("://") + 3).TrimEnd('/');

            // Split by ':' to separate IP and port
            var parts = address.Split(':');

            string ip;
            int port;

            if (parts[0] == "+")
            {
                // If the first part is '+', use "0.0.0.0"
                ip = "0.0.0.0";
                port = int.Parse(parts[1]);
            }
            else
            {
                // Otherwise, take the IP and port from the input
                ip = parts[0];
                port = int.Parse(parts[1]);
            }

            return (ip, port);
        }
    }
}
