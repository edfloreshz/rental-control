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
        app.MapGet("/api/v1/guarantor/{id:guid}", 
                async (Guid id, ISender sender) => (await sender.Send(new Command(id))).ToOkHttpResult())
            .WithTags("Guarantors");
    }
    
    public record Command(Guid Id) : IRequest<Result<Models.Get.Guarantor>>;
    
    public class Handler(GuarantorService service) : IRequestHandler<Command, Result<Models.Get.Guarantor>>
    {
        public async ValueTask<Result<Models.Get.Guarantor>> Handle(Command command, CancellationToken cancellationToken)
        {
            return await service.Get(command.Id, cancellationToken: cancellationToken);
        }
    }
}