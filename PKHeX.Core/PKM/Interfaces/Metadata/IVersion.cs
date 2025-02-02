namespace PKHeX.Core;

/// <summary>
/// Interface that exposes a <see cref="Version"/> to see which version the data originated in.
/// </summary>
public interface IVersion
{
    /// <summary>
    /// The version the data originated in.
    /// </summary>
    GameVersion Version { get; }
}

public static partial class Extensions
{
    private static bool CanBeReceivedBy(this IVersion version, GameVersion game) => version.Version.Contains(game);

    public static GameVersion GetCompatibleVersion(this IVersion version, GameVersion prefer)
    {
        if (version.CanBeReceivedBy(prefer) || version.Version == GameVersion.Any)
            return prefer;
        return version.GetSingleVersion();
    }

    public static GameVersion GetSingleVersion(this IVersion version)
    {
        var v = version.Version;
        if (v.IsValidSavedVersion())
            return v;
        return v.GetSingleVersion();
    }

    public static GameVersion GetSingleVersion(this GameVersion lump)
    {
        const int max = (int)GameUtil.HighestGameID;
        var rnd = Util.Rand;
        while (true) // this isn't optimal, but is low maintenance
        {
            var game = (GameVersion)rnd.Next(1, max);
            if (!lump.Contains(game))
                continue;
            if (game == GameVersion.BU)
                continue; // Ignore this one; only can be Japanese language.
            return game;
        }
    }
}
