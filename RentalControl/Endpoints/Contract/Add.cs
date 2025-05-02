using Carter;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Contract;

public class Add : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/contract",
            async (ISender sender, AddContractCommand addContractCommand) => await sender.Send(addContractCommand));
    }

    public record AddContractCommand(
        Guid TenantId,
        Guid AddressId,
        decimal Deposit,
        decimal Rent,
        string Business,
        DateTime StartDate,
        DateTime EndDate,
        Models.Update.ContractStatus Status,
        Models.Update.ContractType Type) : IRequest<Results<Ok<Models.Update.Contract>, ProblemHttpResult>>;


    public class Handler(Client client)
        : IRequestHandler<AddContractCommand, Results<Ok<Models.Update.Contract>, ProblemHttpResult>>
    {
        public async ValueTask<Results<Ok<Models.Update.Contract>, ProblemHttpResult>> Handle(
            AddContractCommand addContractCommand,
            CancellationToken cancellationToken)
        {
            var contract = addContractCommand.Adapt<Entities.Contract>();

            var contractResponse = await client.Table<Entities.Contract>()
                .Insert(contract, cancellationToken: cancellationToken);

            if (contractResponse.Model is null)
                return TypedResults.Problem("Failed to create contract",
                    statusCode: StatusCodes.Status500InternalServerError);

            return TypedResults.Ok(contractResponse.Model.Adapt<Models.Update.Contract>());
        }
    }
}