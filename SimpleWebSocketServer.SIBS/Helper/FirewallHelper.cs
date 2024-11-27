using System.Linq;
using WindowsFirewallHelper;

namespace SimpleWebSocketServer.SIBS.Lib.Helper
{
    public static class FirewallHelper
    {
        /// <summary>
        /// Adds an inbound rule to the firewall for the specified application name and port.
        /// If a rule with the same name exists, it will be replaced.
        /// </summary>
        /// <param name="applicationName">The name of the application.</param>
        /// <param name="port">The port number.</param>
        /// <returns>The result of the operation.</returns>
        public static bool AddInboundRule(string applicationName, ushort port)
        {
            var firewall = FirewallManager.Instance;

            string inboundRuleName = $"{applicationName}Inbound";
            string outboundRuleName = $"{applicationName}Outbound";

            // Remove existing inbound rule if it exists
            var existingInboundRule = firewall.Rules
                .FirstOrDefault(r => r.Name == inboundRuleName && r.Direction == FirewallDirection.Inbound);
            if (existingInboundRule != null)
            {
                FirewallManager.Instance.Rules.Remove(existingInboundRule);
            }

            // Remove existing outbound rule if it exists
            var existingOutboundRule = firewall.Rules
                .FirstOrDefault(r => r.Name == outboundRuleName && r.Direction == FirewallDirection.Outbound);
            if (existingOutboundRule != null)
            {
                FirewallManager.Instance.Rules.Remove(existingOutboundRule);
            }

            // Create and add the new inbound rule
            var inboundRule = firewall.CreatePortRule(
                FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public,
                inboundRuleName,
                FirewallAction.Allow,
                port,
                FirewallProtocol.TCP);
            inboundRule.Direction = FirewallDirection.Inbound;
            FirewallManager.Instance.Rules.Add(inboundRule);

            // Create and add the new outbound rule
            var outboundRule = firewall.CreatePortRule(
                FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public,
                outboundRuleName,
                FirewallAction.Allow,
                port,
                FirewallProtocol.TCP);
            outboundRule.Direction = FirewallDirection.Outbound;
            FirewallManager.Instance.Rules.Add(outboundRule);

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
