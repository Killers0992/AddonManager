namespace AddonManager
{
    using NetworkedPlugins.API;
    using NetworkedPlugins.API.Events.Player;
    using System;

    public class AddonManagerHandler : NPAddonHandler<AddonConfig>
    {
        public override void OnEnable()
        {
            this.PlayerJoined += OnPlayerJoin;
            this.PlayerLeft += OnPlayerLeft;
            base.OnEnable();
        }

        private void OnPlayerLeft(PlayerLeftEvent ev)
        {
            Logger.Info($"Player left \"{ev.Player.Nickname}\" ({ev.Player.UserID}) from server \"{ev.Player.Server.FullAddress}\".");
        }

        private void OnPlayerJoin(PlayerJoinedEvent ev)
        {
            Logger.Info($"Player joined \"{ev.Player.Nickname}\" ({ev.Player.UserID}) server \"{ev.Player.Server.FullAddress}\".");
        }

        public override void OnDisable()
        {
            this.PlayerJoined -= OnPlayerJoin;
            this.PlayerLeft -= OnPlayerLeft;
            base.OnDisable();
        }
    }
}
