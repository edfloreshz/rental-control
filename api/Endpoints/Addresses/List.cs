using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Addresses;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/address", async (ISender sender) => (await sender.Send(new Query())).ToOkHttpResult())
            .WithTags("Addresses");
    }

    public record Query : IRequest<Result<Models.Get.Address[]>>;

    public class Handler(AddressService addressService) : IRequestHandler<Query, Result<Models.Get.Address[]>>
    {
        public async ValueTask<Result<Models.Get.Address[]>> Handle(Query query, CancellationToken cancellationToken)
        {
            return await addressService.GetAll(cancellationToken);
        }
    }
}
