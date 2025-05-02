using Carter;
using Mapster;
using Mediator;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Contract;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/contract", (ISender sender) => sender.Send(new Query()));
    }

    public record Query : IRequest<Models.Get.Contract[]>;
    
    public class Handler(Client client) : IRequestHandler<Query, Models.Get.Contract[]>
    {
        public async ValueTask<Models.Get.Contract[]> Handle(Query request, CancellationToken cancellationToken)
        {
            var contracts = await client
                .Table<Entities.Contract>()
                .Get(cancellationToken);
            return contracts.Models.Adapt<Models.Get.Contract[]>();
        }
    }
}