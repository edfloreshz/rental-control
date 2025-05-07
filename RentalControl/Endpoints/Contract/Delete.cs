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
        app.MapDelete("/api/v1/contract/{id}", async ([AsParameters] Command command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result.ToNoContentHttpResult();
            })
            .WithTags("Contracts");
    }

    public record Command(Guid Id) : IRequest<Result>;

    public class Handler(ContractService service) : IRequestHandler<Command, Result>
    {
        public async ValueTask<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            return await service.DeleteContract(command.Id, cancellationToken: cancellationToken);
        }
    }
}