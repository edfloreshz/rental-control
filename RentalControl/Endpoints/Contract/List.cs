using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using RentalControl.Services;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Contract;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/contract", async (ISender sender) => (await sender.Send(new Query())).ToOkHttpResult())
            .WithTags("Contracts");
    }

    public record Query : IRequest<Result<Models.Get.Contract[]>>;

    public class Handler(ContractService contractService) : IRequestHandler<Query, Result<Models.Get.Contract[]>>
    {
        public async ValueTask<Result<Models.Get.Contract[]>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await contractService.GetAll(cancellationToken);
        }
    }
}