namespace BattleShip.Models;

/// <summary>
/// Board Cell
/// </summary>
public class BoardCell
{
    /// <summary>
    /// Row Index
    /// </summary>
    public int Row { get; set; }
    
    /// <summary>
    /// Column Index
    /// </summary>
    public int Column { get; set; }
    
    /// <summary>
    /// The ship occupying this cell (null if empty)
    /// </summary>
    public BattleShip? Ship { get; set; }
    
    /// <summary>
    /// Whether this cell has been attacked
    /// </summary>
    public bool IsHit { get; set; }
    
    /// <summary>
    /// Cell display state
    /// </summary>
    public CellState State => 
        IsHit ? (Ship != null ? CellState.Hit : CellState.Miss) :
        Ship != null ? CellState.Ship : CellState.Water;
}

/// <summary>
/// Cell State Enum
/// </summary>
public enum CellState
{
    Water,    // Empty, not attacked
    Ship,     // Contains ship, not attacked
    Hit,      // Ship hit
    Miss      // Water attacked
}