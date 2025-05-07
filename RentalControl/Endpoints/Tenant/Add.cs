using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using RentalControl.Services;
using Supabase.Postgrest;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace RentalControl.Endpoints.Tenant;

public class Add : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/tenant", async (ISender sender, Command command) =>
        {
            var result = await sender.Send(command);
            return result.ToCreatedHttpResult();
        });
    }

    public record Command(Guid AddressId, string Name, string Email, string Phone) : IRequest<Result<Models.Get.Tenant>>;

    public class Handler(TenantService service) : IRequestHandler<Command, Result<Models.Get.Tenant>>
    {
        public async ValueTask<Result<Models.Get.Tenant>> Handle(Command command, CancellationToken cancellationToken)
        {
            var tenant = command.Adapt<Models.Update.Tenant>();
            return await service.CreateTenant(tenant, cancellationToken: cancellationToken);
        }
    }
}