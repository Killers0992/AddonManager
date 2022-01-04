namespace AddonManager
{
    using NetworkedPlugins.API;
    using NetworkedPlugins.API.Events.Player;
    using NetworkedPlugins.API.Events.Server;
    using System;

    public class AddonManagerHandler : NPAddonHandler<AddonConfig>
    {
        public override void OnEnable()
        {
            this.PlayerJoined += OnPlayerJoin;
            this.PlayerLeft += OnPlayerLeft;
            this.PlayerPreAuth += OnPreAuth;
            this.WaitingForPlayers += OnWaitingForPlayers;
            this.RoundEnded += OnRoundEnded;
            base.OnEnable();
        }

        private void OnRoundEnded(RoundEndedEvent ev)
        {
            Logger.Info($"Round ended on server \"{ev.Server.FullAddress}\".");
        }

        private void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            Logger.Info($"Waiting for players on server \"{ev.Server.FullAddress}\".");
        }

        private void OnPreAuth(PlayerPreAuthEvent ev)
        {
            Logger.Info($"Player preauth \"{ev.UserID}\" country \"{ev.Country}\" from server \"{ev.Server.FullAddress}\".");
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
            this.PlayerPreAuth -= OnPreAuth;
            this.WaitingForPlayers -= OnWaitingForPlayers;
            this.RoundEnded -= OnRoundEnded;
            base.OnDisable();
        }
    }
}
