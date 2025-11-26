using System;
namespace BattleShip.Models;

public class Player
{
    /// <summary>
    /// Player Id
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Player Name
    /// </summary>
    public string Name { get; set; } = string.Empty;
}