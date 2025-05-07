using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Contract;

public class Get : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/contract/{id}", async ([AsParameters] Command command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result.ToOkHttpResult();
            })
            .WithTags("Contracts");
    }

    public record Command(Guid Id) : IRequest<Result<Models.Get.Contract>>;

    public class Handler(ContractService service) : IRequestHandler<Command, Result<Models.Get.Contract>>
    {
        public async ValueTask<Result<Models.Get.Contract>> Handle(Command command, CancellationToken cancellationToken)
        {
            return await service.GetContract(command.Id, cancellationToken: cancellationToken);
        }
    }
}