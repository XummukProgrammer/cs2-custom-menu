using CounterStrikeSharp.API.Core;

public class Config_Menu
{
    public string Type { get; set; } = "CenterHtmlMenu";
    public List<string> Command { get; set; } = [];
    public List<string> Permission { get; set; } = [];
    public string Team { get; set; } = "";
    public bool ExitButton { get; set; } = true;
    public int DisplayTime { get; set; } = 0;
    public Dictionary<string, Config_Command> Options { get; set; } = new();
}

public class Config_Command
{
    public List<string> Command { get; set; } = [];
    public List<string> Permission { get; set; } = [];
    public string Team { get; set; } = "";

    public string SoundEvent { get; set; } = "";
    public bool CloseMenu { get; set; } = false;
    public bool Confirm { get; set; } = false;
    public bool Message { get; set; } = false;
    public bool Disabled { get; set; } = false;
    public int Cooldown { get; set; } = 0;
}

public class Config : BasePluginConfig
{
    public string Prefix { get; set; } = "{green}[Menu]{default}";
    public Dictionary<string, Config_Menu> Menus { get; set; } = new();
}