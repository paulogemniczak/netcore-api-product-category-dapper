using FluentValidation;
using FluentValidation.Results;
using Gemniczak.AppService.Dtos;

namespace Gemniczak.API.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryDto>
    {
        public override ValidationResult Validate(ValidationContext<CategoryDto> context)
        {
            return (context.InstanceToValidate == null)
                ? new ValidationResult(new[] { new ValidationFailure("Category", "Category cannot be null.") })
                : base.Validate(context);
        }

        public CategoryValidator()
        {
            When(x => x != null, () =>
            {
                RuleFor(x => x.CategoryName).NotEmpty().WithMessage("'Category name' is required.");
                RuleFor(x => x.CategoryName).MaximumLength(100).WithMessage("Maximum number of characters for the 'Category name' is 100.");
            });
        }
    }
}
