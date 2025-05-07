using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Tenant;

public class Delete : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/tenant/{id}", async ([AsParameters] Query query, ISender sender) =>
            {
                var result = await sender.Send(query);
                return result.ToNoContentHttpResult();
            })
            .WithTags("Tenants");
    }
    
    public record Query(Guid Id) : IRequest<Result>;
    
    public class Handler(TenantService service) : IRequestHandler<Query, Result>
    {
        public async ValueTask<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            return await service.DeleteTenant(request.Id, cancellationToken);
        }
    }
}