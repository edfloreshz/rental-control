using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mediator;
using RentalControl.Services;

namespace RentalControl.Endpoints.Tenant;

public class Delete : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/tenant/{id:guid}",
                async (Guid id, ISender sender) => (await sender.Send(new Command(id))).ToNoContentHttpResult())
            .WithTags("Tenants");
    }

    public record Command(Guid Id) : IRequest<Result>;

    public class Handler(TenantService service) : IRequestHandler<Command, Result>
    {
        public async ValueTask<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            return await service.Delete(request.Id, cancellationToken);
        }
    }
}