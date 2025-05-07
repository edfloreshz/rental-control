using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using RentalControl.Services;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Addresses;

public class Add : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/address",
            async (ISender sender, Command command) =>
            {
                var result = await sender.Send(command);
                return result.ToCreatedHttpResult();
            });
    }

    public record Command(
        string Street,
        string Number,
        string Neighborhood,
        string City,
        string State,
        string ZipCode,
        string Country) : IRequest<Result<Models.Get.Address>>;

    public class Handler(AddressService addressService) : IRequestHandler<Command, Result<Models.Get.Address>>
    {
        public async ValueTask<Result<Models.Get.Address>> Handle(Command command, CancellationToken cancellationToken)
        {
            var address = command.Adapt<Models.Update.Address>();
            return await addressService.CreateAddress(address, cancellationToken);
        }
    }
}