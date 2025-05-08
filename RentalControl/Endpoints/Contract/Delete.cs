using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Contract;

public class Delete : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/contract/{id:guid}",
                async (Guid id, ISender sender) => (await sender.Send(new Command(id))).ToNoContentHttpResult())
            .WithTags("Contracts");
    }

    public record Command(Guid Id) : IRequest<Result>;

    public class Handler(ContractService service) : IRequestHandler<Command, Result>
    {
        public async ValueTask<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            return await service.Delete(command.Id, cancellationToken: cancellationToken);
        }
    }
}