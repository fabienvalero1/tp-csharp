namespace BattleShip.Models;
using FluentValidation;

/// <summary>
/// Fire Validator
/// </summary>
public class FireValidator : AbstractValidator<Fire>
{
    /// <summary>
    /// FireValidator Constructor
    /// </summary>
    public FireValidator()
    {
        RuleFor(x => x.Row)
            .NotEmpty().WithMessage("Row is required.")
            .InclusiveBetween(0, BattleShipSingleton.BoardSize - 1)
            .WithMessage($"Row must be between 0 and {BattleShipSingleton.BoardSize - 1}.");
        RuleFor(x => x.Column)
            .NotEmpty().WithMessage("Column is required.")
            .InclusiveBetween(0, BattleShipSingleton.BoardSize - 1)
            .WithMessage($"Column must be between 0 and {BattleShipSingleton.BoardSize - 1}.");
        RuleFor(x => x.GameId)
            .NotEmpty().WithMessage("Game ID is required.")
            .Must(x => Guid.TryParse(x, out Guid parsed) && parsed == BattleShipSingleton.Instance.Id)
            .WithMessage("Game ID must be a valid GUID.");
    }
}