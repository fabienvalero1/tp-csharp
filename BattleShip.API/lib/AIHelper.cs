namespace BattleShip.Models;

/// <summary>
/// AI Helper Class
/// </summary>
public static class AIHelper
{
    /// <summary>
    /// Checks if all ships of a player have been sunk
    /// </summary>
    /// <param name="board">The board to check</param>
    /// <returns>True if all ships are sunk, false otherwise</returns>
    public static bool AreAllShipsSunk(BoardCell[,] board)
    {
        for (int row = 0; row < BattleShipSingleton.BoardSize; row++)
        {
            for (int col = 0; col < BattleShipSingleton.BoardSize; col++)
            {
                var cell = board[row, col];
                // If there's a ship cell that hasn't been hit, ships are not all sunk
                if (cell.Ship != null && !cell.IsHit)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Determines the winner based on board states
    /// </summary>
    /// <returns>1 if player wins, -1 if AI wins, 0 if no winner yet</returns>
    public static int CheckWinner()
    {
        // Check if all AI ships are sunk (player wins)
        if (AreAllShipsSunk(BattleShipSingleton.Instance.AIPlayerBoard))
        {
            return 1;
        }
        
        // Check if all human ships are sunk (AI wins)
        if (AreAllShipsSunk(BattleShipSingleton.Instance.HumanPlayerBoard))
        {
            return -1;
        }
        
        // No winner yet
        return 0;
    }

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
            int aiRow = random.Next(0, BattleShipSingleton.BoardSize);
            int aiColumn = random.Next(0, BattleShipSingleton.BoardSize);

            var aiCell = BattleShipSingleton.Instance.HumanPlayerBoard[aiRow, aiColumn];

            // Only fire at cells that haven't been hit yet
            if (!aiCell.IsHit)
            {
                aiCell.IsHit = true;
                
                // Check for winner after AI's shot
                int winner = CheckWinner();

                if (aiCell.Ship != null)
                {
                    return new
                    {
                        Row = aiRow,
                        Column = aiColumn,
                        Message = $"AI Hit! AI hit your ship ({aiCell.Ship.Symbol})",
                        CellState = aiCell.State.ToString(),
                        ShipSymbol = aiCell.Ship.Symbol,
                        Winner = winner
                    };
                }
                else
                {
                    return new
                    {
                        Row = aiRow,
                        Column = aiColumn,
                        Message = "AI Miss!",
                        CellState = aiCell.State.ToString(),
                        Winner = winner
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
            CellState = "Error",
            Winner = 0
        };
    }
}