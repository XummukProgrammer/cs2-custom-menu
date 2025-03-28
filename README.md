# cs2-custom-menu
**a plugin to create customizable menus**
> supports ChatMenu/ConsoleMenu/CenterHtmlMenu/WasdMenu/ScreenMenu
>
> permission based or team based menus and commands
>
> every menu and menu option has a bunch of settings, <a href="#config-example">see examples</a>

<br>

<details>
	<summary>showcase</summary>
	<video src="https://github.com/user-attachments/assets/07574910-1b56-48e4-90de-39342743bdaa">
</details>

<br>

## information:

### requirements
- [MetaMod](https://github.com/alliedmodders/metamod-source)
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp)
- [CS2MenuManager](https://github.com/schwarper/CS2MenuManager)
<br>

## example config
<a name="config-example"></a>
**Message** - Default: `true` (sends no permission & selecting message) <br>

**Type** - Default: `"CenterHtmlMenu"` (ChatMenu/ConsoleMenu/CenterHtmlMenu/WasdMenu/ScreenMenu) <br>
**Command** - Defualt: `[""]` (you can use multiple by splitting them with `,`) <br>
**Permission** - Default: `[""]` (empty for no check, also support groups) <br>
**Team** - Default: `""` (T for Terrorist, CT for CounterTerrorist or empty for both) <br>
**ExitButton** - Default: `true` (if false menu wont have exit button) <br>
**DisplayTime** - Default: `0` (time in seconds menu will be open, 0 = no limit) <br>

**Sound** - Default: `""` (use vsnd like sounds/buttons/blip1.vsnd) <br>
**CloseMenu** - Default: `false` (close the menu on select) <br>
**Confirm** - Default: `false` (opens a confirmation menu on select) <br>
**Message** - Default: `false` (sends message with which option player selected) <br>
**Disabled** - Default: `false` (disables the option in menu) <br>
**Cooldown** - Default: `0` (how many seconds until the command can be used again) <br>

```json
{
  "Prefix": "{green}[Menu]{default}",
  "Menus": {
    "Example Menu 1": {
      "Type": "ChatMenu",
      "Command": ["css_menu1"],
      "Options": {
        "Example Option": {
          "Command": ["say test"]
        }
      }
    },
    "Example Menu 2": {
      "Type": "CenterHtmlMenu",
      "Command": ["css_menu2"],
      "Permission": ["@css/reservation"],
      "Team": "T",
      "ExitButton": false,
      "DisplayTime": 10,
      "Options": {
        "Example Option 1": {
          "Command": ["say test"]
        },
        "Example Option 2": {
          "Command": ["css_example2"],
          "Permission": ["@css/root"],
          "Team": "T",
          "SoundEvent": "UIPanorama.inventory_item_pickup",
          "CloseMenu": true,
          "Confirm": true,
          "Message": true,
          "Disabled": false,
          "Cooldown": 5
        }
      }
    }
  }
}
```

<br> <a href="https://ko-fi.com/exkludera" target="blank"><img src="https://cdn.ko-fi.com/cdn/kofi5.png" height="48px" alt="Buy Me a Coffee at ko-fi.com"></a>
