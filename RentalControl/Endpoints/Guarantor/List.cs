using Carter;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Mapster;
using Mediator;
using RentalControl.Services;
using Supabase.Postgrest;

namespace RentalControl.Endpoints.Guarantor;

public class List : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/guarantor", async (ISender sender) => (await sender.Send(new Query())).ToOkHttpResult())
            .WithTags("Guarantors");
    }

    public record Query : IRequest<Result<Models.Get.Guarantor[]>>;

    public class Handler(GuarantorService guarantorService) : IRequestHandler<Query, Result<Models.Get.Guarantor[]>>
    {
        public async ValueTask<Result<Models.Get.Guarantor[]>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            return await guarantorService.GetAll(cancellationToken);
        }
    }
}