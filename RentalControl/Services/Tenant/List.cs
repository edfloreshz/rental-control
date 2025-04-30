using Carter;
using Mapster;
using Mediator;
using Supabase.Postgrest;

namespace RentalControl.Services.Tenant;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/tenants", (ISender sender) => sender.Send(new Query()));
    }

    public record Query : IRequest<IEnumerable<Models.Tenant>>;
    
    public class Handler(Client client) : IRequestHandler<Query, IEnumerable<Models.Tenant>>
    {
        public async ValueTask<IEnumerable<Models.Tenant>> Handle(Query request, CancellationToken cancellationToken)
        {
            var contracts = await client
                .Table<Entities.Tenant>()
                .Get(cancellationToken);
            return contracts.Models.Adapt<IEnumerable<Models.Tenant>>();
        }
    }
}