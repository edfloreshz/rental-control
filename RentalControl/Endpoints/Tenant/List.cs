using Carter;
using Mapster;
using Mediator;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Tenant;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/tenant", (ISender sender) => sender.Send(new Query()));
    }
    
    public record Query : IRequest<Models.Get.Tenant[]>;

    public class Handler(Client client) : IRequestHandler<Query, Models.Get.Tenant[]>
    {
        public async ValueTask<Models.Get.Tenant[]> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var contracts = await client
                .Table<Entities.Tenant>()
                .Get(cancellationToken);
            return contracts.Models.Adapt<Models.Get.Tenant[]>();
        }
    }
}