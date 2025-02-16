using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Menu;

public static partial class Menu
{
    public static class Chat
    {
        public static void Open(CCSPlayerController player, string menuId)
        {
            var menuConfig = Config.Menus[menuId];

            ChatMenu Menu = new (menuConfig.Title);

            foreach (var option in menuConfig.Options)
            {
                if (string.IsNullOrEmpty(option.Permission) || AdminManager.PlayerHasPermissions(player, option.Permission))
                {
                    Menu.AddMenuOption(option.Title, (player, menuOption) =>
                    {
                        if (option.Confirm)
                        {
                            ChatMenu confirmMenu = new (Localizer["ConfirmTitle"]);

                            confirmMenu.AddMenuOption(Localizer["ConfirmAccept"], (player, confirmMenuOption) => {
                                ExecuteOption(player, option);
                            });

                            confirmMenu.AddMenuOption(Localizer["ConfirmDecline"], (player, confirmMenuOption) => {
                                MenuManager.OpenChatMenu(player, Menu);
                            });

                            MenuManager.OpenChatMenu(player, confirmMenu);
                        }
                        else ExecuteOption(player, option);
                    });
                }
            }

            MenuManager.OpenChatMenu(player, Menu);
        }
    }
}