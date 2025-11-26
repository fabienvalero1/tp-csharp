namespace BattleShip.Models;

/// <summary>
/// BattleShip Model
/// </summary>
public class BattleShip
{
    /// <summary>
    /// BattleShip Id
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();
    
    /// <summary>
    /// BattleShip Symbol
    /// </summary>
    public char Symbol { get; set; } // Symbol from 'A' to 'F'
    
    /// <summary>
    /// BattleShip Size
    /// </summary>
    public int Size { get; set; } // Size from 1 to 5
    
    /// <summary>
    /// BattleShip Orientation
    /// </summary>
    public char Orientation { get; set; } // 'H' for Horizontal, 'V' for Vertical
    
    /// <summary>
    /// BattleShip Owner
    /// </summary>
    public Player Owner { get; set; }
}
