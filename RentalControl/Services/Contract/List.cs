using Carter;
using Mapster;
using Mediator;
using Supabase.Postgrest;

namespace RentalControl.Services.Contract;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/contracts", (ISender sender) => sender.Send(new GetContractsQuery()));
    }

    public record GetContractsQuery : IRequest<IEnumerable<Models.Contract>>;

    public class Handler(Client client) : IRequestHandler<GetContractsQuery, IEnumerable<Models.Contract>>
    {
        public async ValueTask<IEnumerable<Models.Contract>> Handle(GetContractsQuery request, CancellationToken cancellationToken)
        {
            var contracts = await client
                .Table<Entities.Contract>()
                .Get(cancellationToken);
            return contracts.Models.Adapt<IEnumerable<Models.Contract>>();
        }
    }
}