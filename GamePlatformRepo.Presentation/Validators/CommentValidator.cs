using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GamePlatformRepo.Models;

namespace GamePlatformRepo.Validators
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            base.RuleFor((comment) => comment.Title)
                .NotEmpty()
                    .WithMessage("Title can not be empty!")
                .MaximumLength(50)
                    .WithMessage($"Length of title can not be bigger than 50 characters!");
            base.RuleFor((game) => game.Description)
                .MaximumLength(250)
                    .WithMessage($"Length of description can not be bigger than 250 characters!");
            base.RuleFor((game) => game.Author)
                .NotEmpty()
                    .WithMessage("Author can not be empty!")
                .MaximumLength(50)
                    .WithMessage($"Length of author can not be bigger than 50 characters!");
        }   
    }
}