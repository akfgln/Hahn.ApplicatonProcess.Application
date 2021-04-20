using FluentValidation;
using Hahn.ApplicatonProcess.February2021.Domain.Models;

namespace Hahn.ApplicatonProcess.February2021.Web.Validators
{
    public class AssetModelValidator : AbstractValidator<AssetModel>
    {
        public AssetModelValidator()
        {
            RuleFor(x => x.AssetName)
               .NotEmpty()
               .WithMessage("AssetName is required")
               .MinimumLength(5)
               .WithMessage("AssetName must be at least 5 Characters");

            RuleFor(x => x.EMailAdressOfDepartment)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress();

            RuleFor(x => x.Department)
                .NotEmpty()
                .WithMessage("Department is required")
                .IsInEnum()
                .WithMessage("Department must be in enum");
        }
    }
}
