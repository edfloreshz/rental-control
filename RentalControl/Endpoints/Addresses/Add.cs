using Carter;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Addresses;

public class Add : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/address",
            async (ISender sender, AddAddressCommand addAddressCommand) => await sender.Send(addAddressCommand));
    }

    public record AddAddressCommand(
        string Street,
        string Number,
        string Neighborhood,
        string City,
        string State,
        string ZipCode,
        string Country) : IRequest<Results<Ok<Models.Update.Address>, ProblemHttpResult>>;

    public class Handler(Client client)
        : IRequestHandler<AddAddressCommand, Results<Ok<Models.Update.Address>, ProblemHttpResult>>
    {
        public async ValueTask<Results<Ok<Models.Update.Address>, ProblemHttpResult>> Handle(
            AddAddressCommand addAddressCommand, CancellationToken cancellationToken)
        {
            var address = addAddressCommand.Adapt<Entities.Address>();

            var addressResponse = await client.Table<Entities.Address>()
                .Insert(address, cancellationToken: cancellationToken);

            if (addressResponse.Model is null)
                return TypedResults.Problem("Failed to create address",
                    statusCode: StatusCodes.Status500InternalServerError);

            return TypedResults.Ok(addressResponse.Model.Adapt<Models.Update.Address>());
        }
    }
}