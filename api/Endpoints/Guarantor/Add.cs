using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Guarantor;

public class Add : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/guarantor",
                async ([AsParameters] Command command, ISender sender) =>
                (await sender.Send(command)).ToCreatedHttpResult())
            .WithTags("Guarantors");
    }

    public record Command(Models.Create.Guarantor Guarantor) : IRequest<Result<Models.Get.Guarantor>>;

    public class Handler(GuarantorService guarantorService) : IRequestHandler<Command, Result<Models.Get.Guarantor>>
    {
        public async ValueTask<Result<Models.Get.Guarantor>> Handle(Command command,
            CancellationToken cancellationToken)
        {
            var guarantor = command.Guarantor.Adapt<Models.Create.Guarantor>();
            return await guarantorService.Create(guarantor, cancellationToken);
        }
    }
}
