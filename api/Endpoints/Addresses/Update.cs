using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Addresses;

public class Update : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/address",
                async ([AsParameters] Command command, ISender sender) => (await sender.Send(command)).ToOkHttpResult())
            .WithTags("Addresses");
    }

    public record Command(Models.Update.Address Address) : IRequest<Result<Models.Get.Address>>;

    public class Handler(AddressService service) : IRequestHandler<Command, Result<Models.Get.Address>>
    {
        public async ValueTask<Result<Models.Get.Address>> Handle(Command command, CancellationToken cancellationToken)
        {
            var address = command.Address.Adapt<Models.Update.Address>();
            return await service.Update(address, cancellationToken: cancellationToken);
        }
    }
}