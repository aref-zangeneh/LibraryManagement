using FluentValidation;
using LibraryManagement.Application.Commands;

namespace LibraryManagement.Application.Validators
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(r => r.Title)
                .NotEmpty().WithMessage("Title must be filled out.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(r => r.Author)
                .NotEmpty().WithMessage("Author must be filled out.")
                .MaximumLength(100).WithMessage("Author must not exceed 100 characters.");

            RuleFor(r => r.PublishedYear)
                .GreaterThanOrEqualTo(0).WithMessage("PublishedYear must be a positive number")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage($"PublishedYear cannot be greater than {DateTime.Now.Year}");

            RuleFor(r => r.Genre)
                .NotEmpty().WithMessage("Genre must be filled out")
                .MaximumLength(100).WithMessage("Genre cannot exceed 100 characters");
        }
    }
}
