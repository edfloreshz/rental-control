using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Tenant;

public class Update : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/tenant",
                async ([AsParameters] Command command, ISender sender) => (await sender.Send(command)).ToOkHttpResult())
            .WithTags("Tenants");
    }

    public record Command(Models.Update.Tenant Tenant) : IRequest<Result<Models.Get.Tenant>>;

    public class Handler(TenantService service) : IRequestHandler<Command, Result<Models.Get.Tenant>>
    {
        public async ValueTask<Result<Models.Get.Tenant>> Handle(Command command, CancellationToken cancellationToken)
        {
            var tenant = command.Tenant.Adapt<Models.Update.Tenant>();
            return await service.Update(tenant, cancellationToken: cancellationToken);
        }
    }
}