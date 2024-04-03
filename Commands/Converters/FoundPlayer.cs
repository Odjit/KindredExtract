using KindredExtract.Models;
using VampireCommandFramework;

namespace KindredExtract.Commands.Converters;

/// <summary>
/// Use this type as a parameter to match a player by steamid first, then by name.
/// The resulting value will be a <see cref="PlayerData"/> struct.
/// </summary>
/// <param name="Value">Contains <see cref="PlayerData"/></param>
public record FoundPlayer(PlayerData Value);

/// <summary>
/// Use this type as a parameter to match a player by steamid first, then by name but the player must be online
/// The resulting value will be a <see cref="PlayerData"/> struct.
/// </summary>
/// <param name="Value">Contains <see cref="PlayerData"/></param>
public record OnlinePlayer(PlayerData Value);

internal class FoundPlayerConverter : CommandArgumentConverter<FoundPlayer>
{
	public override FoundPlayer Parse(ICommandContext ctx, string input)
	{
		var player = HandleFindPlayerData(ctx, input, requireOnline: false);
		return new FoundPlayer(player);
	}

	public static PlayerData HandleFindPlayerData(ICommandContext ctx, string input, bool requireOnline)
	{
		Core.Log.LogDebug($"FoundPlayerConverter.Parse({input})");
		var isLong = ulong.TryParse(input, out var numeric);
		Core.Log.LogDebug($"\tisSteam64: {isLong} {numeric}");
		if (isLong && Core.Players.TryFindSteam(numeric, out var player) && (!requireOnline || player.IsOnline))
		{
			Core.Log.LogDebug($"\tFound by steamid: {player}");
			return player;
		}
		Core.Log.LogDebug($"\tNot found by steamid, trying name.");
		if (Core.Players.TryFindName(input.ToLower(), out var playerByName) && (!requireOnline || playerByName.IsOnline)) // match on lowercase
		{
			Core.Log.LogDebug($"\tFound by name: {playerByName}");
			return playerByName;
		}
		Core.Log.LogDebug($"\tNot found by name, throwing error.");
		throw ctx.Error($"Player {input} not found.");
	}
}
internal class OnlinePlayerConverter : CommandArgumentConverter<OnlinePlayer>
{
	public override OnlinePlayer Parse(ICommandContext ctx, string input)
	{
		var player = FoundPlayerConverter.HandleFindPlayerData(ctx, input, requireOnline: false);
		return new OnlinePlayer(player);
	}
}
