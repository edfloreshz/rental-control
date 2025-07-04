using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using QuestPDF.Fluent;
using RentalControl.Services;

namespace RentalControl.Endpoints.Contract;

public class GeneratePdf() : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/contract/pdf",
                async ([AsParameters] Command command, ISender sender) =>
                (await sender.Send(command)).ToCreatedHttpResult())
            .WithTags("Contracts");
    }

    public record Command(Guid Id) : IRequest<Result<Document>>;

    public class Handler(ContractService contractService) : IRequestHandler<Command, Result<Document>>
    {
        public async ValueTask<Result<Document>> Handle(Command command, CancellationToken cancellationToken)
        {
            return await contractService.GeneratePdf(command.Id, cancellationToken);
        }
    }
}
