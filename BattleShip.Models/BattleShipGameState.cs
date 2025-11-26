namespace BattleShip.Models;

/// <summary>
/// BattleShip Game State
/// </summary>
public class BattleShipGameState
{
    /// <summary>
    /// BattleShipGameState Id
    /// </summary>
    private Guid Id { get; set; }
    
    /// <summary>
    /// Board State
    /// </summary>
    private char[,] Board { get; set; } = new char[10, 10];
    
    /// <summary>
    /// List of BattleShips
    /// </summary>
    private List<BattleShip> BattleShips { get; set; } = new List<BattleShip>();
    
    /// <summary>
    /// Clone the BattleShip Game State
    /// </summary>
    /// <returns>
    /// A cloned instance of BattleShipGameState
    /// </returns>
    public BattleShipGameState Clone()
    {
        BattleShipGameState clone = new BattleShipGameState();
        clone.Id = this.Id;
        clone.Board = (char[,])this.Board.Clone();
        clone.BattleShips = new List<BattleShip>(this.BattleShips);
        return clone;
    }
}