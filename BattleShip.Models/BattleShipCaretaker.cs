namespace BattleShip.Models;

/// <summary>
/// BattleShip Caretaker
/// </summary>
public class BattleShipCaretaker
{
    /// <summary>
    /// Memento to hold the state of the game
    /// </summary>
    private readonly List<BattleShipMemento> _history = new List<BattleShipMemento>();
    
    /// <summary>
    /// Add a memento to the history
    /// </summary>
    public void AddMemento(BattleShipMemento memento)
    {
        _history.Add(memento);
    }

    /// <summary>
    /// Get a memento from the history by index
    /// </summary>
    public BattleShipMemento GetMemento(int index)
    {
        if (index >= 0 && index < _history.Count)
            return _history[index];
        return null;
    }

    /// <summary>
    /// Get the last saved memento
    /// </summary>
    public BattleShipMemento GetLastMemento()
    {
        if (_history.Count == 0)
            return null;
        return _history[_history.Count - 1];
    }
}