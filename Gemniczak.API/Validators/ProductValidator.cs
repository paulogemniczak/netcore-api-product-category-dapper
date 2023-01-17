using FluentValidation;
using FluentValidation.Results;
using Gemniczak.AppService.Dtos;

namespace Gemniczak.API.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public override ValidationResult Validate(ValidationContext<ProductDto> context)
        {
            return (context.InstanceToValidate == null)
                ? new ValidationResult(new[] { new ValidationFailure("Product", "Product cannot be null.") })
                : base.Validate(context);
        }

        public ProductValidator()
        {
            When(x => x != null, () =>
            {
                RuleFor(x => x.ProductName).NotEmpty().WithMessage("'Product name' is required.");
                RuleFor(x => x.ProductName).MaximumLength(100).WithMessage("Maximum number of characters for the 'Product name' is 100.");
                RuleFor(x => x.ProductCategory).NotNull().WithMessage("'Product category' is required.");
                RuleFor(x => x.ProductCategory.CategoryId).NotNull().WithMessage("'Category Id' is required.");
                RuleFor(x => x.ProductCategory.CategoryId).GreaterThan(0).WithMessage("'Category Id' must be greater than 0.");
            });
        }
    }
}
