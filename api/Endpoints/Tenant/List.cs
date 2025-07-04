using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Tenant;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/tenant", async (ISender sender) => (await sender.Send(new Query())).ToOkHttpResult())
            .WithTags("Tenants");
    }

    public record Query : IRequest<Result<Models.Get.Tenant[]>>;

    public class Handler(TenantService service) : IRequestHandler<Query, Result<Models.Get.Tenant[]>>
    {
        public async ValueTask<Result<Models.Get.Tenant[]>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await service.GetAll(cancellationToken);
        }
    }
}
