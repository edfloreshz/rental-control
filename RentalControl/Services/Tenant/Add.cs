using Carter;
using Mapster;
using Mediator;
using Supabase.Postgrest;

namespace RentalControl.Services.Tenant;

public class Add : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/tenant", async (ISender sender, Command command) => await sender.Send(command));
    }

    public record Command(Tenant Tenant, Address Address) : IRequest<IResult>;

    public record Tenant(string Name, string Email, string Phone);

    public record Address(
        string Street,
        string Number,
        string Neighborhood,
        string City,
        string State,
        string ZipCode,
        string Country);

    public class Handler(Client client) : IRequestHandler<Command, IResult>
    {
        public async ValueTask<IResult> Handle(Command command, CancellationToken cancellationToken)
        {
            var (tenantCommand, addressCommand) = command;

            var tenant = tenantCommand.Adapt<Entities.Tenant>();
            var address = addressCommand.Adapt<Entities.Address>();

            var addressResponse = await client.Table<Entities.Address>()
                .Insert(address, cancellationToken: cancellationToken);

            if (addressResponse.Model is null)
                return Results.Problem("Failed to create address",
                    statusCode: StatusCodes.Status500InternalServerError);

            tenant.AddressId = addressResponse.Model.Id;

            var tenantResponse = await client.Table<Entities.Tenant>()
                .Insert(tenant, cancellationToken: cancellationToken);

            if (tenantResponse.Model is null) 
                return Results.Problem("Failed to create tenant",
                    statusCode: StatusCodes.Status500InternalServerError);

            return Results.Ok();
        }
    }
}