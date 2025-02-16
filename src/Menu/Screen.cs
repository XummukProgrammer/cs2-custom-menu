using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CS2ScreenMenuAPI;

public static partial class Menu
{
    public static class Screen
    {
        public static void Open(CCSPlayerController player, string menuId)
        {
            var menuConfig = Config.Menus[menuId];

            ScreenMenu Menu = new ScreenMenu(menuConfig.Title, Instance);

            foreach (var option in menuConfig.Options)
            {
                if (string.IsNullOrEmpty(option.Permission) || AdminManager.PlayerHasPermissions(player, menuConfig.Permission))
                {
                    Menu.AddOption(option.Title, (player, menuOption) =>
                    {
                        if (option.Confirm)
                        {
                            ScreenMenu confirmMenu = new ScreenMenu(Localizer["ConfirmTitle"], Instance);

                            confirmMenu.AddOption(Localizer["ConfirmAccept"], (player, confirmMenuOption) =>
                            {
                                ExecuteOption(player, option);
                            });

                            confirmMenu.AddOption(Localizer["ConfirmDecline"], (player, confirmMenuOption) =>
                            {
                                MenuAPI.OpenMenu(Instance, player, Menu);
                            });

                            MenuAPI.OpenMenu(Instance, player, confirmMenu);
                        }
                        else ExecuteOption(player, option);
                    });
                }
            }

            MenuAPI.OpenMenu(Instance, player, Menu);
        }
    }
}