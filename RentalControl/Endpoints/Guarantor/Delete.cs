using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Guarantor;

public class Delete : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/guarantor/{id}", async ([AsParameters] Command command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result.ToNoContentHttpResult();
            })
            .WithTags("Guarantors");
    }
    
    public record Command(Guid Id) : IRequest<Result>;
    
    public class Handler(GuarantorService service) : IRequestHandler<Command, Result>
    {
        public async ValueTask<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            return await service.DeleteGuarantor(command.Id, cancellationToken: cancellationToken);
        }
    }
}