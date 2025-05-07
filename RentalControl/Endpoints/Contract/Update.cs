using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Contract;

public class Update : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/contract", async ([AsParameters] Command command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result.ToOkHttpResult();
            })
            .WithTags("Contracts");
    }

    public record Command(Guid Id, Models.Update.Contract Contract) : IRequest<Result<Models.Get.Contract>>;

    public class Handler(ContractService service) : IRequestHandler<Command, Result<Models.Get.Contract>>
    {
        public async ValueTask<Result<Models.Get.Contract>> Handle(Command command, CancellationToken cancellationToken)
        {
            var contract = command.Contract.Adapt<Models.Update.Contract>();
            return await service.UpdateContract(command.Id, contract, cancellationToken: cancellationToken);
        }
    }
}