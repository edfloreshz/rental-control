using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Guarantor;

public class Get :ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/guarantor/{id}", async ([AsParameters] Command command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result.ToOkHttpResult();
            })
            .WithTags("Guarantors");
    }
    
    public record Command(Guid Id) : IRequest<Result<Models.Get.Guarantor>>;
    
    public class Handler(GuarantorService service) : IRequestHandler<Command, Result<Models.Get.Guarantor>>
    {
        public async ValueTask<Result<Models.Get.Guarantor>> Handle(Command command, CancellationToken cancellationToken)
        {
            return await service.GetGuarantor(command.Id, cancellationToken: cancellationToken);
        }
    }
}