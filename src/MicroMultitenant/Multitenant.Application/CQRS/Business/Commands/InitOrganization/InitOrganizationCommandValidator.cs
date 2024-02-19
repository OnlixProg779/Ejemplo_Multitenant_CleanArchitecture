using FluentValidation;

namespace Multitenant.Application.CQRS.Business.Commands.InitOrganization
{
    public class InitOrganizationCommandValidator : AbstractValidator<InitOrganizationCommand>
    {
        public InitOrganizationCommandValidator() {
            RuleFor(a => a.OrganizationName).NotNull().NotEmpty();
        }
    }
}
