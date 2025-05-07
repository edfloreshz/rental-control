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
        app.MapGet("/api/v1/address/{id}", async ([AsParameters] Command command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result.ToOkHttpResult();
            })
            .WithTags("Addresses");
    }

    public record Command(Guid Id) : IRequest<Result<Models.Get.Address>>;

    public class Handler(AddressService service) : IRequestHandler<Command, Result<Models.Get.Address>>
    {
        public async ValueTask<Result<Models.Get.Address>> Handle(Command command, CancellationToken cancellationToken)
        {
            return await service.GetAddress(command.Id, cancellationToken: cancellationToken);
        }
    }
}