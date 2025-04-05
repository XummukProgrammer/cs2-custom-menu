using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Localization;
using CS2MenuManager.API.Class;
using CS2MenuManager.API.Enum;
using CS2MenuManager.API.Interface;

public static partial class Menu
{
    static Plugin Instance = Plugin.Instance;
    static Config Config = Instance.Config;
    static IStringLocalizer Localizer = Instance.Localizer;

    static readonly Dictionary<int, Dictionary<Config_Command, DateTime>> Cooldowns = new();

    [CommandHelper(minArgs: 0, whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public static void Open(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null) return;

        if (Plugin.commandMenuId.TryGetValue(info.GetCommandString, out var menuName))
        {
            var menuConfig = Config.Menus[menuName];

            if (!Utils.HasPermission(player, menuConfig.Permission, menuConfig.Team))
            {
                player.PrintToChat(Config.Prefix + Localizer["NoPermission"]);
                return;
            }

            OpenCustomMenu(player, menuName, menuConfig);
        }
    }

    public static void OpenCustomMenu(CCSPlayerController player, string menuName, Config_Menu menuConfig)
    {
        IMenu Menu = MenuManager.MenuByType(menuConfig.Type, menuName, Instance);

        if (menuConfig.ExitButton)
            Menu.ExitButton = true;

        foreach (var optiondata in menuConfig.Options)
        {
            var optionName = optiondata.Key;
            var option = optiondata.Value;

            if (Utils.HasPermission(player, option.Permission, option.Team))
            {
                Menu.AddItem(optionName, (player, menuOption) =>
                {
                    if (option.Confirm)
                    {
                        IMenu confirmMenu = MenuManager.MenuByType(menuConfig.Type, Localizer["ConfirmTitle"], Instance);

                        confirmMenu.AddItem(Localizer["ConfirmAccept"], (player, confirmMenuOption) =>
                        {
                            ExecuteOption(player, optionName, option, Menu);
                        });

                        confirmMenu.AddItem(Localizer["ConfirmDecline"], (player, confirmMenuOption) =>
                        {
                            Menu.Display(player, menuConfig.DisplayTime);
                        });

                        confirmMenu.Display(player, 0);
                    }
                    else ExecuteOption(player, optionName, option, Menu);
                }, option.Disabled ? DisableOption.DisableHideNumber : DisableOption.None);
            }
        }

        Menu.Display(player, menuConfig.DisplayTime);
    }

    static void ExecuteOption(CCSPlayerController player, string optionName, Config_Command option, IMenu Menu)
    {
        if (!Cooldowns.ContainsKey(player.Slot))
            Cooldowns[player.Slot] = new();

        if (CommandCooldown(player, option))
        {
            player.PrintToChat(Config.Prefix + Localizer["Cooldown"]);
            return;
        }

        if (option.Message)
            player.PrintToChat(Config.Prefix + Localizer["Selected", optionName]);

        if (!string.IsNullOrEmpty(option.SoundEvent))
        {
            RecipientFilter filter = [player];
            player.EmitSound(option.SoundEvent, filter);
        }

        if (option.CloseMenu)
            MenuManager.CloseActiveMenu(player);

        else Menu.Display(player, Menu.MenuTime);

        foreach (var command in option.Command)
            player.ExecuteClientCommandFromServer(command);

        if (option.Cooldown > 0)
        {
            Cooldowns[player.Slot][option] = DateTime.Now.AddSeconds(option.Cooldown);
            Instance.AddTimer(option.Cooldown, () =>
            {
                if (Cooldowns.ContainsKey(player.Slot) && Cooldowns[player.Slot].ContainsKey(option))
                {
                    Cooldowns[player.Slot].Remove(option);

                    if (Cooldowns[player.Slot].Count <= 0)
                        Cooldowns.Remove(player.Slot);
                }
            }, TimerFlags.STOP_ON_MAPCHANGE);
        }
    }

    static bool CommandCooldown(CCSPlayerController? player, Config_Command option)
    {
        if (player == null || !Cooldowns.ContainsKey(player.Slot))
            return false;

        if (Cooldowns[player.Slot].TryGetValue(option, out var cooldownEndTime))
        {
            if (DateTime.Now < cooldownEndTime)
                return true;
        }

        return false;
    }
}