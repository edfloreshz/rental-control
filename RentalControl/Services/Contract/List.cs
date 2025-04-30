using Carter;
using Mapster;
using Mediator;
using Supabase.Postgrest;

namespace RentalControl.Services.Contract;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/contracts", (ISender sender) => sender.Send(new Query()));
    }

    public record Query : IRequest<IEnumerable<Models.Contract>>;

    public class Handler(Client client) : IRequestHandler<Query, IEnumerable<Models.Contract>>
    {
        public async ValueTask<IEnumerable<Models.Contract>> Handle(Query request, CancellationToken cancellationToken)
        {
            var contracts = await client
                .Table<Entities.Contract>()
                .Get(cancellationToken);
            return contracts.Models.Adapt<IEnumerable<Models.Contract>>();
        }
    }
}