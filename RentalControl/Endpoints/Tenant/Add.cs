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
        app.MapPost("/api/v1/tenant",
                async ([AsParameters] Command command, ISender sender) =>
                (await sender.Send(command)).ToCreatedHttpResult())
            .WithTags("Tenants");
    }

    public record Command(Models.Create.Tenant Tenant) : IRequest<Result<Models.Get.Tenant>>;

    public class Handler(TenantService service) : IRequestHandler<Command, Result<Models.Get.Tenant>>
    {
        public async ValueTask<Result<Models.Get.Tenant>> Handle(Command command, CancellationToken cancellationToken)
        {
            var tenant = command.Tenant.Adapt<Models.Create.Tenant>();
            return await service.Create(tenant, cancellationToken: cancellationToken);
        }
    }
}