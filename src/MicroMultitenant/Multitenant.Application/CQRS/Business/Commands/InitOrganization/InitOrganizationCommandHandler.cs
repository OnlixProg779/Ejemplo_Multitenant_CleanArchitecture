using MediatR;
using Multitenant.Application.Contracts.Services;
using Multitenant.Application.CQRS.Business.Commands.InitOrganization.Resources;

namespace Multitenant.Application.CQRS.Business.Commands.InitOrganization
{
    public class InitOrganizationCommandHandler : IRequestHandler<InitOrganizationCommand, InitOrganizationResponse>
    {
        private readonly IApplyBusinessMigrations _applyBussinesMigrations;

        public InitOrganizationCommandHandler(IApplyBusinessMigrations applyBussinesMigrations)
        {
            _applyBussinesMigrations = applyBussinesMigrations;
        }

        public async Task<InitOrganizationResponse> Handle(InitOrganizationCommand request, CancellationToken cancellationToken)
        {


            await _applyBussinesMigrations.ApplyMigrations(request.OrganizationName);
            // TODO: dejar un log para saber que usuario creo dicho tenant

            return new InitOrganizationResponse();
        }
    }
}
