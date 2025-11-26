using BattleShip.Models;
using BattleShip.API.lib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/start/{playerName}", (string playerName) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(playerName))
        {
            return Results.BadRequest("Player name cannot be empty.");
        }

        Player humanPlayer = new Player { Name = playerName };
        
        BattleShipSingleton.Instance.CreateBattleShipGame(humanPlayer);
        
        return Results.Ok(new { GameId = BattleShipSingleton.Instance.Id });
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error starting game for player: {PlayerName}", playerName);
        return Results.Problem(
            detail: "An error occurred while starting the game.",
            statusCode: 500
        );
    }
})
.WithName("PostStart");

app.MapPost("fire/{row}/{column}/{gameId}", (int row, int column, string gameId) =>
{
    try
    {
        if (!Guid.TryParse(gameId, out Guid parsedGameId))
        {
            return Results.BadRequest("Invalid Game ID format.");
        }

        if (parsedGameId != BattleShipSingleton.Instance.Id)
        {
            return Results.BadRequest("Invalid Game ID.");
        }

        if (row < 0 || row >= BattleShipSingleton.Instance.Board.GetLength(0) ||
            column < 0 || column >= BattleShipSingleton.Instance.Board.GetLength(1))
        {
            return Results.BadRequest("Invalid coordinates.");
        }

        var cell = BattleShipSingleton.Instance.Board[row, column];
        
        if (cell == null)
        {
            return Results.Problem("Board cell is not initialized.", statusCode: 500);
        }

        if (cell.IsHit)
        {
            return Results.Ok(new { Message = "Already fired at this location.", CellState = cell.State.ToString() });
        }

        // Player's turn
        cell.IsHit = true;
        string playerMessage;
        
        if (cell.Ship != null)
        {
            playerMessage = $"Hit! You hit {cell.Ship.Owner.Name}'s ship ({cell.Ship.Symbol})";
        }
        else
        {
            playerMessage = "Miss!";
        }

        // AI's turn (automatic response)
        var aiResponse = AIHelper.AIFire();

        return Results.Ok(new 
        { 
            PlayerTurn = new 
            {
                Row = row,
                Column = column,
                Message = playerMessage,
                CellState = cell.State.ToString()
            },
            AITurn = aiResponse
        });
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error firing at position ({Row}, {Column})", row, column);
        return Results.Problem(
            detail: "An error occurred while processing the fire command.",
            statusCode: 500
        );
    }
})
.WithName("PostFire");

app.Run();