namespace AddonManager
{
    using NetworkedPlugins.API.Interfaces;

    public class AddonConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}
