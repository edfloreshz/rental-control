using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using RentalControl.Services;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Contract;

public class Add : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/contract", async (ISender sender, Command command) =>
            {
                var result = await sender.Send(command);
                return result.ToCreatedHttpResult();
            })
            .WithTags("Contracts");
    }

    public record Command(Models.Create.Contract Contract) : IRequest<Result<Models.Get.Contract>>;

    public class Handler(ContractService contractService) : IRequestHandler<Command, Result<Models.Get.Contract>>
    {
        public async ValueTask<Result<Models.Get.Contract>> Handle(Command command, CancellationToken cancellationToken)
        {
            var contract = command.Contract.Adapt<Models.Create.Contract>();
            return await contractService.CreateContract(contract, cancellationToken);
        }
    }
}