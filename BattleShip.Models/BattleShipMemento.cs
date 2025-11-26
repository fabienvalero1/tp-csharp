namespace BattleShip.Models;

/// <summary>
/// BattleShip Memento
/// </summary>
public class BattleShipMemento
{
    /// <summary>
    /// BattleShip Game State
    /// </summary>
    private BattleShipGameState _state;
    
    /// <summary>
    /// Get the state of the BattleShip game
    /// </summary>
    public BattleShipGameState GetState()
    {
        return _state;
    }
    
    /// <summary>
    /// Instantiate BattleShip Memento
    /// </summary>
    /// <param name="state"></param>
    public BattleShipMemento(BattleShipGameState state)
    {
        _state = state.Clone();
    }
}