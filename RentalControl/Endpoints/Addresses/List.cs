using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using RentalControl.Services;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Addresses;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/address", async (ISender sender) =>
            {
                var result = await sender.Send(new Query());
                return result.ToOkHttpResult();
            })
            .WithTags("Addresses");
    }

    public record Query : IRequest<Result<Models.Get.Address[]>>;

    public class Handler(AddressService addressService) : IRequestHandler<Query, Result<Models.Get.Address[]>>
    {
        public async ValueTask<Result<Models.Get.Address[]>> Handle(Query query, CancellationToken cancellationToken)
        {
            return await addressService.GetAddresses(cancellationToken);
        }
    }
}