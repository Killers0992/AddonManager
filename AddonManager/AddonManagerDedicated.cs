using NetworkedPlugins.API;
using NetworkedPlugins.API.Enums;
using NetworkedPlugins.API.Models;
using System;
using System.Collections.Generic;

namespace AddonManager
{
    public class AddonManagerDedicated : NPAddonDedicated<AddonConfig, AddonConfig>
    {
        public override string AddonAuthor { get; } = "Killers0992";
        public override string AddonName { get; } = "AddonManager";
        public override Version AddonVersion { get; } = new Version(1, 0, 0);
        public override string AddonId { get; } = "MLcvnJ3CcnJNHT2R";

        public override NPPermissions Permissions { get; } = new NPPermissions()
        {
            ReceivePermissions = new List<AddonSendPermissionTypes>(),
            SendPermissions = new List<AddonReceivePermissionTypes>() 
            {
                AddonReceivePermissionTypes.RemoteAdminNewCommands,
                AddonReceivePermissionTypes.RemoteAdminMessages
            }
        };
    }
}
