namespace BattleShip.Models;

/// <summary>
/// BattleShip Originator
/// </summary>
public class BattleShipOriginator
{
    /// <summary>
    /// BattleShip Game State
    /// </summary>
    private BattleShipGameState _state;
    
    /// <summary>
    /// Instantiate BattleShip Originator
    /// </summary>
    /// <param name="memento"></param>
    public void SetMemento(BattleShipMemento memento)
    {
        _state = memento.GetState();
    }
    
    /// <summary>
    /// Create Memento
    /// </summary>
    /// <returns></returns>
    public BattleShipMemento CreateMemento()
    {
        return new BattleShipMemento(_state);
    }
}