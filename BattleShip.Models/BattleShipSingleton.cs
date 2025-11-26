namespace BattleShip.Models;

/// <summary>
/// BattleShip Singleton to hold the state of the game
/// </summary>
public class BattleShipSingleton : SingletonBase<BattleShipSingleton>
{
    /// <summary>
    /// Game Id
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// BattleShips in the game
    /// </summary>
    public List<BattleShip> BattleShips { get; set; } = new List<BattleShip>();
    
    /// <summary>
    /// Board of the game
    /// </summary>
    public BoardCell[,] Board { get; set; } = new BoardCell[10, 10];
    
    /// <summary>
    /// Players in the game
    /// </summary>
    public List<Player> Players { get; set; } = new List<Player>();
    
    /// <summary>
    /// The method to create a new BattleShip game
    /// </summary>
    public void CreateBattleShipGame(Player humanPlayer)
    {
        BattleShips.Clear();
        Players.Clear();
        Players.Add(humanPlayer);
        Players.Add(new Player { Name = "Computer" });
        Id = Guid.NewGuid();
        Board = new BoardCell[10, 10];
        
        Initialization();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public void Initialization()
    {
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int j = 0; j < Board.GetLength(1); j++)
            {
                Board[i, j] = new BoardCell
                {
                    Row = i,
                    Column = j,
                    Ship = null,
                    IsHit = false
                };
            }
        }
        
        Random random = new Random();
        int rows = Board.GetLength(0);
        int cols = Board.GetLength(1);

        for (int i = 0; i < 10; i++)
        {
            char symbol = (char)('A' + random.Next(0, 6));
            int size = random.Next(1, 5);
            char orientation = random.Next(0, 2) == 0 ? 'H' : 'V';
            Player owner = Players[i % Players.Count];

            int startRow, startCol;
            bool placed = false;
            int attempts = 0;
            const int maxAttempts = 100;

            // Keep trying to place the ship until it fits without overlapping
            while (!placed && attempts < maxAttempts)
            {
                if (orientation == 'H')
                {
                    startRow = random.Next(0, rows);
                    startCol = random.Next(0, cols - size + 1);
                }
                else
                {
                    startRow = random.Next(0, rows - size + 1);
                    startCol = random.Next(0, cols);
                }

                // Check if the position is available (no overlapping ships)
                bool canPlace = true;
                for (int j = 0; j < size; j++)
                {
                    int checkRow = orientation == 'H' ? startRow : startRow + j;
                    int checkCol = orientation == 'H' ? startCol + j : startCol;
                    
                    if (Board[checkRow, checkCol].Ship != null)
                    {
                        canPlace = false;
                        break;
                    }
                }

                if (canPlace)
                {
                    // Create the battleship
                    var battleShip = new BattleShip
                    {
                        Symbol = symbol,
                        Size = size,
                        Orientation = orientation,
                        Owner = owner,
                    };

                    // Place the ship on the board
                    for (int j = 0; j < size; j++)
                    {
                        int placeRow = orientation == 'H' ? startRow : startRow + j;
                        int placeCol = orientation == 'H' ? startCol + j : startCol;
                        Board[placeRow, placeCol].Ship = battleShip;
                    }

                    BattleShips.Add(battleShip);
                    placed = true;
                }

                attempts++;
            }

            if (!placed)
            {
                System.Diagnostics.Debug.WriteLine($"Could not place ship {i} after {maxAttempts} attempts");
            }
        }
    }
}