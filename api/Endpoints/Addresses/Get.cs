using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Addresses;

public class Get : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/address/{id:guid}",
                async (Guid id, ISender sender) => (await sender.Send(new Command(id))).ToOkHttpResult())
            .WithTags("Addresses");
    }

    public record Command(Guid Id) : IRequest<Result<Models.Get.Address>>;

    public class Handler(AddressService service) : IRequestHandler<Command, Result<Models.Get.Address>>
    {
        public async ValueTask<Result<Models.Get.Address>> Handle(Command command, CancellationToken cancellationToken)
        {
            return await service.Get(command.Id, cancellationToken: cancellationToken);
        }
    }
}