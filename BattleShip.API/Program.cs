using BattleShip.Models;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddValidatorsFromAssemblyContaining<StartValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<FireValidator>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/start", (Start start, IValidator<Start> validator) =>
{
    try
    {
		var validationResult = validator.Validate(start);
    	if (!validationResult.IsValid)
    	{
        	return Results.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
    	}

        Player humanPlayer = new Player { Name = start.PlayerName };
        
        BattleShipSingleton.Instance.CreateBattleShipGame(humanPlayer);
        
        return Results.Ok(new { GameId = BattleShipSingleton.Instance.Id });
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error starting game for player: {PlayerName}", start.PlayerName);
        return Results.Problem(
            detail: "An error occurred while starting the game.",
            statusCode: 500
        );
    }
})
.WithName("PostStart");

app.MapPost("fire", (Fire fire, IValidator<Fire> validator) =>
{
    try
    {
		var validationResult = validator.Validate(fire);
    	if (!validationResult.IsValid)
    	{
        	return Results.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
    	}

        var cell = BattleShipSingleton.Instance.AIPlayerBoard[fire.Row, fire.Column];
        
        if (cell == null)
        {
            return Results.Problem("Board cell is not initialized.", statusCode: 500);
        }

        if (cell.IsHit)
        {
            return Results.Ok(new { 
                Message = "Already fired at this location.", 
                CellState = cell.State.ToString(),
                Winner = 0
            });
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

        // Check for winner after player's shot
        int winnerAfterPlayerTurn = AIHelper.CheckWinner();
        
        // If player wins, don't let AI play
        if (winnerAfterPlayerTurn == 1)
        {
            return Results.Ok(new 
            { 
                PlayerTurn = new 
                {
                    Row = fire.Row,
                    Column = fire.Column,
                    Message = playerMessage,
                    CellState = cell.State.ToString(),
                    Winner = winnerAfterPlayerTurn
                },
                AITurn = (object?)null,
                Winner = winnerAfterPlayerTurn
            });
        }

        // AI's turn (automatic response)
        var aiResponse = AIHelper.AIFire();
        
        // Get final winner status (could be -1 if AI won on their turn)
        int finalWinner = AIHelper.CheckWinner();

        return Results.Ok(new 
        { 
            PlayerTurn = new 
            {
                Row = fire.Row,
                Column = fire.Column,
                Message = playerMessage,
                CellState = cell.State.ToString(),
                Winner = winnerAfterPlayerTurn
            },
            AITurn = aiResponse,
            Winner = finalWinner
        });
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error firing at position ({Row}, {Column})", fire.Row, fire.Column);
        return Results.Problem(
            detail: "An error occurred while processing the fire command.",
            statusCode: 500
        );
    }
})
.WithName("PostFire");

app.Run();
