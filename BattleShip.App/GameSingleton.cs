namespace BattleShip.App;

using BattleShip.Models;

public class GameSingleton : SingletonBase<GameSingleton>
{
    public bool GameStarted { get; private set; } = false;
    public Guid Id { get; private set; }

    public char[,] PlayerBoard { get; private set; }

    public bool?[,] OpponentBoard { get; private set; }

    public void CreateNewGame(Guid id)
    {
        GameStarted = true;
        Id = id;
        PlayerBoard = InitializePlayerBoard(10, 10);
        OpponentBoard = InitializeOpponentBoard(10, 10);
    }

    private static char[,] InitializePlayerBoard(int rows, int columns)
    {
        char[,] grid = new char[rows, columns];
        foreach (int i in Enumerable.Range(0, grid.GetLength(0)))
        {
            foreach (int j in Enumerable.Range(0, grid.GetLength(1)))
            {
                grid[i, j] = '\0';
            }
        }
        return grid;
    }

    private static bool?[,] InitializeOpponentBoard(int rows, int columns)
    {
        bool?[,] grid = new bool?[rows, columns];
        foreach (int i in Enumerable.Range(0, grid.GetLength(0)))
        {
            foreach (int j in Enumerable.Range(0, grid.GetLength(1)))
            {
                grid[i, j] = null;
            }
        }
        return grid;
    }
}
