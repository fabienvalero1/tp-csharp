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
    /// Human Player Board
    /// </summary>
    public BoardCell[,] HumanPlayerBoard { get; set; } = new BoardCell[10, 10];

	/// <summary>
    /// AI Player Board
    /// </summary>
    public BoardCell[,] AIPlayerBoard { get; set; } = new BoardCell[10, 10];
    
    /// <summary>
    /// Players in the game
    /// </summary>
    public List<Player> Players { get; set; } = new List<Player>();

	/// <summary>
    /// Game Board Size
    /// </summary>
	public const int BoardSize = 10;
    
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
        HumanPlayerBoard = InitializeBoardCell(true);
        AIPlayerBoard = InitializeBoardCell(false);
    }

    /// <summary>
    /// Initialize a new game board with ships placed randomly
    /// </summary>
	/// <returns>
	/// The initialized game board
	/// </returns>
    public BoardCell [,] InitializeBoardCell(bool isHuman)
    {
		BoardCell[,] board = new BoardCell[BoardSize, BoardSize];

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = new BoardCell
                {
                    Row = i,
                    Column = j,
                    Ship = null,
                    IsHit = false
                };
            }
        }
        
        Random random = new Random();
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

		Player owner = isHuman ? Players[0] : Players[1];

        for (int i = 0; i < BoardSize; i++)
        {
            char symbol = (char)('A' + random.Next(0, 6));
            int size = random.Next(1, 5);
            char orientation = random.Next(0, 2) == 0 ? 'H' : 'V';

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
                    
                    if (board[checkRow, checkCol].Ship != null)
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
                        board[placeRow, placeCol].Ship = battleShip;
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

		return board;
    }
}