namespace BattleShip.App;

using BattleShip.Models;
using System.Text.Json;

public class GameSingleton : SingletonBase<GameSingleton>
{
    public bool GameStarted { get; private set; } = false;
    public Guid Id { get; private set; }

    public char[,] PlayerBoard { get; private set; }

    public bool?[,] OpponentBoard { get; private set; }

    public void CreateNewGame(Guid id, JsonElement JsonPlayerBoard)
    {
        GameStarted = true;
        Id = id;
        PlayerBoard = InitializePlayerBoard(JsonPlayerBoard);
        OpponentBoard = InitializeOpponentBoard(PlayerBoard.GetLength(0), PlayerBoard.GetLength(1));
    }

    private static char[,] InitializePlayerBoard(JsonElement JsonPlayerBoard)
    {
        int rows = JsonPlayerBoard.GetArrayLength();
        int columns = JsonPlayerBoard[0].GetArrayLength();
        char[,] grid = new char[rows, columns];
        foreach (int row in Enumerable.Range(0, JsonPlayerBoard.GetArrayLength()))
        {
            foreach (int column in Enumerable.Range(0, JsonPlayerBoard[row].GetArrayLength()))
            {
                JsonElement ship = JsonPlayerBoard[row][column].GetProperty("ship");
                grid[row, column] = ship.ValueKind != JsonValueKind.Null ? ship.GetProperty("symbol").GetString()[0] : '\0';
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
