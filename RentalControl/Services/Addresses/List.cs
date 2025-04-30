using Carter;
using Mapster;
using Mediator;
using Supabase.Postgrest;

namespace RentalControl.Services.Addresses;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/addresses", (ISender sender) => sender.Send(new Query()));
    }

    public record Query : IRequest<IEnumerable<Models.Address>>;
    
    public class Handler(Client client) : IRequestHandler<Query, IEnumerable<Models.Address>>
    {
        public async ValueTask<IEnumerable<Models.Address>> Handle(Query request, CancellationToken cancellationToken)
        {
            var contracts = await client
                .Table<Entities.Address>()
                .Get(cancellationToken);
            return contracts.Models.Adapt<IEnumerable<Models.Address>>();
        }
    }
}