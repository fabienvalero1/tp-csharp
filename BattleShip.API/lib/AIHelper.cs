using BattleShip.Models;
namespace BattleShip.API.lib;

/// <summary>
/// AI Helper Class
/// </summary>
public static class AIHelper
{
    /// <summary>
    /// AI Fire Logic
    /// </summary>
    /// <returns>
    /// An object containing the result of the AI's firing action
    /// </returns>
    public static object AIFire()
    {
        Random random = new Random();
        int maxAttempts = 100;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            int aiRow = random.Next(0, BattleShipSingleton.Instance.Board.GetLength(0));
            int aiColumn = random.Next(0, BattleShipSingleton.Instance.Board.GetLength(1));

            var aiCell = BattleShipSingleton.Instance.Board[aiRow, aiColumn];

            // Only fire at cells that haven't been hit yet
            if (!aiCell.IsHit)
            {
                aiCell.IsHit = true;

                if (aiCell.Ship != null)
                {
                    return new
                    {
                        Row = aiRow,
                        Column = aiColumn,
                        Message = $"AI Hit! AI hit your ship ({aiCell.Ship.Symbol})",
                        CellState = aiCell.State.ToString(),
                        ShipSymbol = aiCell.Ship.Symbol
                    };
                }
                else
                {
                    return new
                    {
                        Row = aiRow,
                        Column = aiColumn,
                        Message = "AI Miss!",
                        CellState = aiCell.State.ToString()
                    };
                }
            }

            attempts++;
        }

        return new
        {
            Row = -1,
            Column = -1,
            Message = "AI could not find a valid target",
            CellState = "Error"
        };
    }
}