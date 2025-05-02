using Carter;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Guarantor;

public class Add : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/guarantor",
            async (ISender sender, Command command) => await sender.Send(command));
    }

    public record Command(Guid AddressId, string Name, string Email, string Phone)
        : IRequest<Results<Ok<Models.Update.Guarantor>, ProblemHttpResult>>;

    public class Handler(Client client)
        : IRequestHandler<Command, Results<Ok<Models.Update.Guarantor>, ProblemHttpResult>>
    {
        public async ValueTask<Results<Ok<Models.Update.Guarantor>, ProblemHttpResult>> Handle(Command command,
            CancellationToken cancellationToken)
        {
            var guarantor = command.Adapt<Entities.Guarantor>();

            var guarantorResponse = await client.Table<Entities.Guarantor>()
                .Insert(guarantor, cancellationToken: cancellationToken);

            if (guarantorResponse.Model is null)
                return TypedResults.Problem("Failed to create guarantor",
                    statusCode: StatusCodes.Status500InternalServerError);

            return TypedResults.Ok(guarantorResponse.Model.Adapt<Models.Update.Guarantor>());
        }
    }
}