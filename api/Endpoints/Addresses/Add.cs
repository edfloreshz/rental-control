using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Addresses;

public class Add : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/address",
                async ([AsParameters] Command command, ISender sender) =>
                (await sender.Send(command)).ToCreatedHttpResult())
            .WithTags("Addresses");
    }

    public record Command(Models.Create.Address Address) : IRequest<Result<Models.Get.Address>>;

    public class Handler(AddressService addressService) : IRequestHandler<Command, Result<Models.Get.Address>>
    {
        public async ValueTask<Result<Models.Get.Address>> Handle(Command command, CancellationToken cancellationToken)
        {
            var address = command.Address.Adapt<Models.Create.Address>();
            return await addressService.Create(address, cancellationToken);
        }
    }
}
