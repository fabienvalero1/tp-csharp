namespace BattleShip.Models;
using FluentValidation;

/// <summary>
/// Start Validator
/// </summary>
public class StartValidator : AbstractValidator<Start>
{
    /// <summary>
    /// StartValidator Constructor
    /// </summary>
    public StartValidator()
    {
        RuleFor(x => x.PlayerName)
            .NotEmpty().WithMessage("PlayerName is required.")
            .MinimumLength(3).WithMessage("PlayerName must be at least 3 characters long.")
            .MaximumLength(20).WithMessage("PlayerName cannot exceed 20 characters.");
    }
}