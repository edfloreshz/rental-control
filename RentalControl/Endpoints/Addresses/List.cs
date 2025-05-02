using Carter;
using Mapster;
using Mediator;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Addresses;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/address", (ISender sender) => sender.Send(new Query()));
    }

    public record Query : IRequest<Models.Get.Address[]>;
    
    public class Handler(Client client) : IRequestHandler<Query, Models.Get.Address[]>
    {
        public async ValueTask<Models.Get.Address[]> Handle(Query query, CancellationToken cancellationToken)
        {
            var contracts = await client
                .Table<Entities.Address>()
                .Get(cancellationToken);
            return contracts.Models.Adapt<Models.Get.Address[]>();
        }
    }
}