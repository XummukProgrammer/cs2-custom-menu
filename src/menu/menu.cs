using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using CS2ScreenMenuAPI;
using Microsoft.Extensions.Localization;

public static partial class Menu
{
    static Plugin Instance = Plugin.Instance;
    static Config Config = Instance.Config;
    static IStringLocalizer Localizer = Instance.Localizer;

    static readonly Dictionary<int, PlayerCooldown> Cooldowns = new();
    class PlayerCooldown
    {
        public Dictionary<string, DateTime> OptionCooldowns { get; set; } = new Dictionary<string, DateTime>();
    }

    [CommandHelper(minArgs: 0, whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public static void Open(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null) return;

        if (Plugin.commandMenuId.TryGetValue(info.GetCommandString, out var menuId))
        {
            var menuConfig = Instance.Config.Menus[menuId];

            string permission = menuConfig.Permission.ToLower();

            string team = menuConfig.Team.ToLower();

            bool isTeamValid = (team == "t" || team == "terrorist") && player.Team == CsTeam.Terrorist ||
                               (team == "ct" || team == "counterterrorist") && player.Team == CsTeam.CounterTerrorist ||
                               (team == "" || team == "both" || team == "all");

            if ((!string.IsNullOrEmpty(permission) && !AdminManager.PlayerHasPermissions(player, permission)) || !isTeamValid)
            {
                player.PrintToChat(Config.Prefix + Localizer["NoPermission"]);
                return;
            }

            switch (menuConfig.Type.ToLower())
            {
                case "chat":
                case "text":
                    Chat.Open(player, menuId);
                    break;
                case "html":
                case "center":
                case "centerhtml":
                case "hud":
                    HTML.Open(player, menuId);
                    break;
                case "wasd":
                case "wasdmenu":
                    WASD.Open(player, menuId);
                    break;
                case "screen":
                case "screenmenu":
                    Screen.Open(player, menuId);
                    break;
                default:
                    HTML.Open(player, menuId);
                    break;
            }
        }
    }

    static void ExecuteOption(CCSPlayerController player, Options option)
    {
        if (!Cooldowns.ContainsKey(player.Slot))
            Cooldowns[player.Slot] = new PlayerCooldown();

        if (CommandCooldown(player, option.Command))
        {
            player.PrintToChat(Config.Prefix + Localizer["Cooldown"]);
            return;
        }

        if (!string.IsNullOrEmpty(option.Command))
            player.PrintToChat(Config.Prefix + Localizer["Selected", option.Title]);

        var commands = option.Command.Split(',');
        foreach (var command in commands)
            player.ExecuteClientCommandFromServer(command.Trim());

        if (option.Sound.Contains("vsnd"))
            player.ExecuteClientCommand($"play {option.Sound}");

        if (option.CloseMenu)
        {
            MenuManager.CloseActiveMenu(player);
            WASD.WasdManager.CloseMenu(player);
            MenuAPI.CloseActiveMenu(player);
        }

        if (option.Cooldown > 0)
        {
            Cooldowns[player.Slot].OptionCooldowns[option.Command] = DateTime.Now.AddSeconds(option.Cooldown);
            Instance.AddTimer(option.Cooldown, () =>
            {
                if (Cooldowns.ContainsKey(player.Slot) && Cooldowns[player.Slot].OptionCooldowns.ContainsKey(option.Command))
                    Cooldowns[player.Slot].OptionCooldowns.Remove(option.Command);
            });
        }
    }

    static bool CommandCooldown(CCSPlayerController? player, string command)
    {
        if (player == null || !Cooldowns.ContainsKey(player.Slot))
            return false;

        if (Cooldowns[player.Slot].OptionCooldowns.TryGetValue(command, out var cooldownEndTime))
        {
            if (DateTime.Now < cooldownEndTime)
                return true;
        }

        return false;
    }
}