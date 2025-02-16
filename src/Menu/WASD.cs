using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Modules.Admin;
using WASDSharedAPI;

public static partial class Menu
{
    public static class WASD
    {
        public static IWasdMenuManager WasdManager = new PluginCapability<IWasdMenuManager>("wasdmenu:manager").Get()!;

        public static void Open(CCSPlayerController player, string menuId)
        {
            var menuConfig = Config.Menus[menuId];

            IWasdMenu Menu = WasdManager.CreateMenu(menuConfig.Title);

            foreach (var option in menuConfig.Options)
            {
                if (string.IsNullOrEmpty(option.Permission) || AdminManager.PlayerHasPermissions(player, menuConfig.Permission))
                {
                    Menu.Add(option.Title, (player, menuOption) =>
                    {
                        if (option.Confirm)
                        {
                            IWasdMenu confirmMenu = WasdManager.CreateMenu(Localizer["ConfirmTitle"]);

                            confirmMenu.Add(Localizer["ConfirmAccept"], (player, confirmMenuOption) =>
                            {
                                ExecuteOption(player, option);
                            });

                            confirmMenu.Add(Localizer["ConfirmDecline"], (player, confirmMenuOption) =>
                            {
                                WasdManager.OpenMainMenu(player, Menu);
                            });

                            WasdManager.OpenMainMenu(player, confirmMenu);
                        }
                        else ExecuteOption(player, option);
                    });
                }
            }

            WasdManager.OpenMainMenu(player, Menu);
        }
    }
}