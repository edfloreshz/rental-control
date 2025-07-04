using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Tenant;

public class Get : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/tenant/{id:guid}",
                async (Guid id, ISender sender) => (await sender.Send(new Command(id))).ToOkHttpResult())
            .WithTags("Tenants");
    }

    public record Command(Guid Id) : IRequest<Result<Models.Get.Tenant>>;

    public class Handler(TenantService service) : IRequestHandler<Command, Result<Models.Get.Tenant>>
    {
        public async ValueTask<Result<Models.Get.Tenant>> Handle(Command command, CancellationToken cancellationToken)
        {
            return await service.Get(command.Id, cancellationToken);
        }
    }
}