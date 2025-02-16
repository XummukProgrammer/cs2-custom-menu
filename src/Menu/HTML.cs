using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Menu;

public static partial class Menu
{
    public static class HTML
    {
        public static void Open(CCSPlayerController player, string menuId)
        {
            var menuConfig = Config.Menus[menuId];

            CenterHtmlMenu Menu = new (menuConfig.Title, Instance);

            foreach (var option in menuConfig.Options)
            {
                if (string.IsNullOrEmpty(option.Permission) || AdminManager.PlayerHasPermissions(player, option.Permission))
                {
                    Menu.AddMenuOption(option.Title, (player, menuOption) =>
                    {
                        if (option.Confirm)
                        {
                            CenterHtmlMenu confirmMenu = new (Localizer["ConfirmTitle"], Instance);

                            confirmMenu.AddMenuOption(Localizer["ConfirmAccept"], (player, confirmMenuOption) => {
                                ExecuteOption(player, option);
                            });

                            confirmMenu.AddMenuOption(Localizer["ConfirmDecline"], (player, confirmMenuOption) => {
                                MenuManager.OpenCenterHtmlMenu(Instance, player, Menu);
                            });

                            MenuManager.OpenCenterHtmlMenu(Instance, player, confirmMenu);
                        }
                        else ExecuteOption(player, option);
                    });
                }
            }

            MenuManager.OpenCenterHtmlMenu(Instance, player, Menu);
        }
    }
}