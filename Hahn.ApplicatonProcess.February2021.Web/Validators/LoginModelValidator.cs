using FluentValidation;
using Hahn.ApplicatonProcess.February2021.Domain.Models;

namespace Hahn.ApplicatonProcess.February2021.Web.Validators
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
