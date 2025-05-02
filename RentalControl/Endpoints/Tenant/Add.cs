using Carter;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Tenant;

public class Add : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/tenant",
            async (ISender sender, AddTenantCommand addTenantCommand) => await sender.Send(addTenantCommand));
    }

    public record AddTenantCommand(Guid AddressId, string Name, string Email, string Phone)
        : IRequest<Results<Ok<Models.Update.Tenant>, ProblemHttpResult>>;

    public class Handler(Client client)
        : IRequestHandler<AddTenantCommand, Results<Ok<Models.Update.Tenant>, ProblemHttpResult>>
    {
        public async ValueTask<Results<Ok<Models.Update.Tenant>, ProblemHttpResult>> Handle(
            AddTenantCommand addTenantCommand,
            CancellationToken cancellationToken)
        {
            var tenant = addTenantCommand.Adapt<Entities.Tenant>();

            var tenantResponse = await client.Table<Entities.Tenant>()
                .Insert(tenant, cancellationToken: cancellationToken);

            if (tenantResponse.Model is null)
                return TypedResults.Problem("Failed to create tenant",
                    statusCode: StatusCodes.Status500InternalServerError);

            return TypedResults.Ok(tenantResponse.Model.Adapt<Models.Update.Tenant>());
        }
    }
}