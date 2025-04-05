using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;

public class Plugin : BasePlugin, IPluginConfig<Config>
{
    public override string ModuleName => "Custom Menu";
    public override string ModuleVersion => "1.0.9";
    public override string ModuleAuthor => "exkludera";

    public static Plugin Instance { get; set; } = new();
    public static Dictionary<string, string> commandMenuId = new Dictionary<string, string>();

    public override void Load(bool hotReload)
    {
        Instance = this;

        foreach (var menu in Config.Menus)
        {
            foreach (var command in menu.Value.Command)
            {
                commandMenuId[command] = menu.Key;
                AddCommand(command, menu.Key, Menu.Open);
            }
        }
    }

    public override void Unload(bool hotReload)
    {
        foreach (var menu in Config.Menus)
        {
            foreach (var command in menu.Value.Command)
                RemoveCommand(command, Menu.Open);
        }
    }

    public Config Config { get; set; } = new Config();
    public void OnConfigParsed(Config config) {
        Config = config;
        Config.Prefix = StringExtensions.ReplaceColorTags(Config.Prefix);
    }
}