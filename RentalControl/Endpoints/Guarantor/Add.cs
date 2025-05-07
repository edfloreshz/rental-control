using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using RentalControl.Services;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Guarantor;

public class Add : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/guarantor", async (ISender sender, Command command) =>
        {
            var result = await sender.Send(command);
            return result.ToCreatedHttpResult();
        });
    }

    public record Command(Guid AddressId, string Name, string Email, string Phone) : IRequest<Result<Models.Get.Guarantor>>;

    public class Handler(GuarantorService guarantorService) : IRequestHandler<Command, Result<Models.Get.Guarantor>>
    {
        public async ValueTask<Result<Models.Get.Guarantor>> Handle(Command command, CancellationToken cancellationToken)
        {
            var guarantor = command.Adapt<Models.Update.Guarantor>();
            return await guarantorService.CreateGuarantor(guarantor, cancellationToken);
        }
    }
}