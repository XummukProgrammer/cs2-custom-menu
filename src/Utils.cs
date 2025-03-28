using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Utils;

internal class Utils
{
    public static bool HasPermission(CCSPlayerController player, List<string> Permissions, string Team)
    {
        return PermissionCheck(player, Permissions) && AllowedTeam(player, Team);
    }

    public static bool PermissionCheck(CCSPlayerController player, List<string> Permissions)
    {
        if (Permissions.Count <= 0)
            return true;

        foreach (string perm in Permissions)
        {
            if (perm.StartsWith("@") && AdminManager.PlayerHasPermissions(player, perm))
                return true;
            if (perm.StartsWith("#") && AdminManager.PlayerInGroup(player, perm))
                return true;
        }

        return false;
    }

    public static bool AllowedTeam(CCSPlayerController player, string Team)
    {
        Team = Team.ToLower();

        bool isTeamValid =
            (Team == "t" || Team == "terrorist") && player.Team == CsTeam.Terrorist ||
            (Team == "ct" || Team == "counterterrorist") && player.Team == CsTeam.CounterTerrorist ||
            (Team == "" || Team == "both" || Team == "all");

        return isTeamValid;
    }
}