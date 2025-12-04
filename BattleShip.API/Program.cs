using BattleShip.Models;
using BattleShip.API.lib;
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
        
        var board = BattleShipSingleton.Instance.HumanPlayerBoard;

        var serializableBoard = Enumerable.Range(0, board.GetLength(0))
            .Select(i => Enumerable.Range(0, board.GetLength(1))
                .Select(j => board[i, j])
                .ToList()
            ).ToList();

        return Results.Ok(new {
            GameId = BattleShipSingleton.Instance.Id,
            PlayerBoard = serializableBoard,
        });
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
                Row = fire.Row,
                Column = fire.Column,
                Message = playerMessage,
                CellState = cell.State.ToString()
            },
            AITurn = aiResponse
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