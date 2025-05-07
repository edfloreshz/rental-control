using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Guarantor;

public class Update : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/guarantor", async ([AsParameters] Command command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result.ToOkHttpResult();
            })
            .WithTags("Guarantors");
    }

    public record Command(Guid Id, Models.Update.Guarantor Guarantor) : IRequest<Result<Models.Get.Guarantor>>;

    public class Handler(GuarantorService service) : IRequestHandler<Command, Result<Models.Get.Guarantor>>
    {
        public async ValueTask<Result<Models.Get.Guarantor>> Handle(Command command, CancellationToken cancellationToken)
        {
            var guarantor = command.Guarantor.Adapt<Models.Update.Guarantor>();
            return await service.UpdateGuarantor(command.Id, guarantor, cancellationToken: cancellationToken);
        }
    }
}