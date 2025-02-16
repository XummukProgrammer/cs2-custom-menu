using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;

public class Plugin : BasePlugin, IPluginConfig<Config>
{
    public override string ModuleName => "Custom Menu";
    public override string ModuleVersion => "1.0.7";
    public override string ModuleAuthor => "exkludera";

    public static Plugin Instance { get; set; } = new();
    public static Dictionary<string, string> commandMenuId = new Dictionary<string, string>();

    public override void Load(bool hotReload)
    {
        Instance = this;

        foreach (var menu in Config.Menus)
        {
            var commands = menu.Value.Command.ToLower();

            var menuId = menu.Key;

            foreach (var command in commands.Split(','))
            {
                commandMenuId[command.Trim()] = menuId;
                AddCommand(command.Trim(), menu.Value.Title, Menu.Open);
            }
        }
    }

    public override void Unload(bool hotReload)
    {
        foreach (var menu in Config.Menus)
        {
            var commands = menu.Value.Command.ToLower();

            foreach (var command in commands.Split(','))
                RemoveCommand(command.Trim(), Menu.Open);
        }
    }

    public Config Config { get; set; } = new Config();
    public void OnConfigParsed(Config config) {
        Config = config;
        Config.Prefix = StringExtensions.ReplaceColorTags(Config.Prefix);
    }
}
