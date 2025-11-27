namespace BattleShip.Models;

/// <summary>
/// Fire Request
/// </summary>
public class Fire
{
    /// <summary>
    /// Row
    /// </summary>
    public int Row { get; set; }
    
    /// <summary>
    /// Column
    /// </summary>
    public int Column { get; set; }
    
    /// <summary>
    /// Game Id
    /// </summary>
    public string GameId { get; set; }
}