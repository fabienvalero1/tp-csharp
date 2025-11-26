namespace BattleShip.Models;

public abstract class SingletonBase<T> where T : class, new()
{
    /// <summary>
    /// The singleton instance
    /// </summary>
    private static readonly Lazy<T> _instance = new(() => new T());
    
    /// <summary>
    /// The singleton instance accessor
    /// </summary>
    public static T Instance => _instance.Value;
    
    /// <summary>
    /// The protected constructor to prevent external instantiation
    /// </summary>
    protected SingletonBase() { }
}