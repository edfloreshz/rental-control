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
        app.MapGet("/api/v1/tenant/{id}", async ([AsParameters] Query query, ISender sender) =>
            {
                var result = await sender.Send(query);
                return result.ToOkHttpResult();
            })
            .WithTags("Tenants");
    }

    public record Query(Guid Id) : IRequest<Result<Models.Get.Tenant>>;

    public class Handler(TenantService service) : IRequestHandler<Query, Result<Models.Get.Tenant>>
    {
        public async ValueTask<Result<Models.Get.Tenant>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await service.GetTenant(request.Id, cancellationToken);
        }
    }
}