using Carter;
using Mapster;
using Mediator;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Guarantor;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/guarantor", (ISender sender) => sender.Send(new Query()));
    }

    public record Query : IRequest<Models.Get.Guarantor[]>;
    
    public class Handler(Client client) : IRequestHandler<Query, Models.Get.Guarantor[]>
    {
        public async ValueTask<Models.Get.Guarantor[]> Handle(Query request, CancellationToken cancellationToken)
        {
            var contracts = await client
                .Table<Entities.Guarantor>()
                .Get(cancellationToken);
            return contracts.Models.Adapt<Models.Get.Guarantor[]>();
        }
    }
}