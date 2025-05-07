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
        });
    }

    public record Command(
        Guid TenantId,
        Guid AddressId,
        decimal Deposit,
        decimal Rent,
        string Business,
        DateTime StartDate,
        DateTime EndDate,
        Models.Update.ContractStatus Status,
        Models.Update.ContractType Type) : IRequest<Result<Models.Get.Contract>>;

    public class Handler(ContractService contractService) : IRequestHandler<Command, Result<Models.Get.Contract>>
    {
        public async ValueTask<Result<Models.Get.Contract>> Handle(Command command, CancellationToken cancellationToken)
        {
            var contract = command.Adapt<Models.Update.Contract>();
            return await contractService.CreateContract(contract, cancellationToken);
        }
    }
}