using Carter;
using Mapster;
using Mediator;
using Supabase.Postgrest;

namespace RentalControl.Services.Addresses;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/addresses", (ISender sender) => sender.Send(new GetAddressQuery()));
    }

    public record GetAddressQuery : IRequest<IEnumerable<Models.Address>>;
    
    public class Handler(Client client) : IRequestHandler<GetAddressQuery, IEnumerable<Models.Address>>
    {
        public async ValueTask<IEnumerable<Models.Address>> Handle(GetAddressQuery request, CancellationToken cancellationToken)
        {
            var contracts = await client
                .Table<Entities.Address>()
                .Get(cancellationToken);
            return contracts.Models.Adapt<IEnumerable<Models.Address>>();
        }
    }
}