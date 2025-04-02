using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePlatformRepo.Models;
using FluentValidation;

namespace GamePlatformRepo.Validators
{
    public class GameValidator : AbstractValidator<Game>
    {
        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
        public GameValidator()
        {
            base.RuleFor((game) => game.Name)
                .NotEmpty()
                    .WithMessage("Name can not be empty!")
                .MaximumLength(50)
                    .WithMessage($"Length of name can not be bigger than 50 characters!");

            base.RuleFor((game) => game.Price)
                .LessThan(100)
                .GreaterThanOrEqualTo(0);
            base.RuleFor((game) => game.Description)
                .MaximumLength(250)
                    .WithMessage($"Length of description can not be bigger than 250 characters!");
             base.RuleFor((game) => game.DateOfPublication)
                .Must(BeAValidDate)
                .WithMessage($"Date of Publication Must be a valid date");
        } 
    }
}