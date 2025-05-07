using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using RentalControl.Services;
using Supabase.Postgrest;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace RentalControl.Endpoints.Tenant;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/tenant", async (ISender sender) =>
            {
                var result = await sender.Send(new Query());
                return result.ToOkHttpResult();
            })
            .WithTags("Tenants");
    }

    public record Query : IRequest<Result<Models.Get.Tenant[]>>;

    public class Handler(TenantService service) : IRequestHandler<Query, Result<Models.Get.Tenant[]>>
    {
        public async ValueTask<Result<Models.Get.Tenant[]>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await service.GetTenants(cancellationToken);
        }
    }
}