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
        app.MapGet("/api/v1/contract/{id:guid}", 
                async (Guid id, ISender sender) => (await sender.Send(new Command(id))).ToOkHttpResult())
            .WithTags("Contracts");
    }

    public record Command(Guid Id) : IRequest<Result<Models.Get.Contract>>;

    public class Handler(ContractService service) : IRequestHandler<Command, Result<Models.Get.Contract>>
    {
        public async ValueTask<Result<Models.Get.Contract>> Handle(Command command, CancellationToken cancellationToken)
        {
            return await service.Get(command.Id, cancellationToken: cancellationToken);
        }
    }
}