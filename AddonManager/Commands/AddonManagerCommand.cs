using NetworkedPlugins.API;
using NetworkedPlugins.API.Enums;
using NetworkedPlugins.API.Interfaces;
using NetworkedPlugins.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AddonManager.Commands
{
    public class AddonManagerCommand : ICommand
    {
        public string CommandName { get; } = "AM";

        public string Description { get; } = "Addon manager for networkedplugins.";

        public string Permission { get; } = "addonmanager.admin";

        public CommandType Type { get; } = CommandType.RemoteAdmin;

        public void Invoke(NPPlayer player, string[] arguments)
        {
            if (arguments.Length == 0)
            {
                player.SendRAMessage(string.Concat("Addon Manager:",
                    Environment.NewLine,
                    "- AM installed",
                    Environment.NewLine,
                    "- AM install <name>",
                    Environment.NewLine,
                    "- AM uninstall <name>",
                    Environment.NewLine,
                    "- AM reload <name>",
                    Environment.NewLine,
                    "- AM setservername <name>",
                    Environment.NewLine,
                    "- AM linkedservers",
                    Environment.NewLine,
                    "- AM list"), "AM");
                return;
            }
            switch (arguments[0].ToUpper())
            {
                case "INSTALLED":
                    {
                        List<string> addons = new List<string>();
                        foreach (var ad in player.Server.ServerConfig.InstalledAddons)
                        {
                            var addon = player.Server.GetAddon(ad);
                            if (addon == null)
                                continue;
                            addons.Add($" - {addon.AddonName.Replace(" ", "-").ToUpper()} ({addon.AddonVersion}) made by \"{addon.AddonAuthor}\".");
                        }
                        string str = "Addon Manager | Installed addons:";
                        if (addons.Count == 0)
                            str += "\n  - No addons installed, install some from AM list.";
                        else
                            str += $"\n{string.Join("\n", addons)}";
                        player.SendRAMessage(str, "AM");
                    }
                    break;
                case "INSTALL":
                    {
                        if (arguments.Length < 2)
                        {
                            player.SendRAMessage("Syntax: AM install <addonName>", "AM");
                            return;
                        }
                        var name = arguments[1].Replace(" ", "-").ToUpper();
                        var addon = NPManager.Singleton.DedicatedAddonHandlers.Values.FirstOrDefault(p => p.DefaultAddon.AddonName.Replace(" ", "-").ToUpper() == name);
                        if (addon == null)
                        {
                            player.SendRAMessage($"Addon \"{name}\" not exists!", "AM");
                            return;
                        }

                        if (player.Server.ServerConfig.InstalledAddons.Contains(addon.DefaultAddon.AddonId))
                        {
                            player.SendRAMessage($"Addon \"{name}\" is already installed!", "AM");
                            return;
                        }

                        player.Server.ServerConfig.InstalledAddons.Add(addon.DefaultAddon.AddonId);
                        player.Server.SaveServerConfig();
                        player.Server.LoadAddon(addon.DefaultAddon.AddonId);
                        player.SendRAMessage($"Addon \"{name}\" installed!", "AM");
                    }
                    break;
                case "UNINSTALL":
                    {
                        if (arguments.Length < 2)
                        {
                            player.SendRAMessage("Syntax: AM uninstall <addonName>", "AM");
                            return;
                        }
                        var name = arguments[1].Replace(" ", "-").ToUpper().Replace("ADDONMANAGER", "");
                        var addon = NPManager.Singleton.DedicatedAddonHandlers.Values.FirstOrDefault(p => p.DefaultAddon.AddonName.Replace(" ", "-").ToUpper() == name);
                        if (addon == null)
                        {
                            player.SendRAMessage($"Addon \"{name}\" not exists!", "AM");
                            return;
                        }

                        if (!player.Server.ServerConfig.InstalledAddons.Contains(addon.DefaultAddon.AddonId))
                        {
                            player.SendRAMessage($"Addon \"{name}\" is not installed!", "AM");
                            return;
                        }

                        player.Server.UnloadAddon(addon.DefaultAddon.AddonId);
                        player.Server.ServerConfig.InstalledAddons.Remove(addon.DefaultAddon.AddonId);
                        player.Server.SaveServerConfig();
                        player.SendRAMessage($"Addon \"{name}\" uninstalled!", "AM");
                    }
                    break;
                case "RELOAD":
                    {
                        if (arguments.Length < 2)
                        {
                            player.SendRAMessage("Syntax: AM reload <addonName>", "AM");
                            return;
                        }
                        var name = arguments[1].Replace(" ", "-").ToUpper();
                        var addon = NPManager.Singleton.DedicatedAddonHandlers.Values.FirstOrDefault(p => p.DefaultAddon.AddonName.Replace(" ", "-").ToUpper() == name);
                        if (addon == null)
                        {
                            player.SendRAMessage($"Addon \"{name}\" not exists!", "AM");
                            return;
                        }

                        if (!player.Server.ServerConfig.InstalledAddons.Contains(addon.DefaultAddon.AddonId))
                        {
                            player.SendRAMessage($"Addon \"{name}\" is not installed!", "AM");
                            return;
                        }

                        player.Server.UnloadAddon(addon.DefaultAddon.AddonId);
                        player.Server.LoadAddon(addon.DefaultAddon.AddonId);
                        player.SendRAMessage($"Addon \"{name}\" reloaded!", "AM");
                    }
                    break;
                case "LIST":
                    {
                        List<string> addons = new List<string>();
                        foreach (var addon in NPManager.Singleton.DedicatedAddonHandlers.Values)
                        {
                            addons.Add($" - {addon.DefaultAddon.AddonName.Replace(" ", "-").ToUpper()} ({addon.DefaultAddon.AddonVersion}) made by \"{addon.DefaultAddon.AddonAuthor}\".");
                        }
                        string str = "Addon Manager | Avaliable addons:";
                        if (addons.Count == 0)
                            str += "\n  - No addons avaliable.";
                        else
                            str += $"\n{string.Join("\n", addons)}";
                        player.SendRAMessage(str, "AM");
                    }
                    break;
                case "LINKEDSERVERS":
                    {
                        List<string> servers = new List<string>();
                        foreach (var server in NPManager.Singleton.Servers.Values.Where(p => p.ServerConfig.LinkToken == player.Server.ServerConfig.LinkToken).OrderBy(p => p.ServerPort))
                        {
                            servers.Add($" - {server.ServerName} ({server.FullAddress})");
                        }
                        string str = "Addon Manager | Linked servers:";
                        if (servers.Count == 0)
                            str += "\n  - No servers linked.";
                        else
                            str += $"\n{string.Join("\n", servers)}";
                        player.SendRAMessage(str, "AM");
                    }
                    break;
                case "SETSERVERNAME":
                    {
                        if (arguments.Length < 2)
                        {
                            player.SendRAMessage("Syntax: AM setservername <name>", "AM");
                            return;
                        }
                        var name = string.Join(" ", arguments.Skip(1));

                        if (name.Length > 20)
                        {
                            player.SendRAMessage("Servername cant be longer than 20 characters!", "AM");
                            return;
                        }

                        player.Server.ServerConfig.ServerName = name;
                        player.Server.SaveServerConfig();
                        player.SendRAMessage($"Changed servername to \"{name}\".", "AM");
                    }
                    break;
                default:
                    player.SendRAMessage(string.Concat("Addon Manager:",
                        Environment.NewLine,
                        "- AM installed",
                        Environment.NewLine,
                        "- AM install <name>",
                        Environment.NewLine,
                        "- AM uninstall <name>",
                        Environment.NewLine,
                        "- AM reload <name>",
                        Environment.NewLine,
                        "- AM setservername <name>",
                        Environment.NewLine,
                        "- AM linkedservers",
                        Environment.NewLine,
                        "- AM list"), "AM");
                    break;
            }
        }
    }
}
