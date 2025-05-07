using CSharpFunctionalExtensions;
using Mapster;
using Supabase.Postgrest;

namespace RentalControl.Services;

public class GuarantorService(Client client)
{
    public async ValueTask<Result<Models.Get.Guarantor[]>> GetGuarantors(CancellationToken cancellationToken)
    {
        try
        {
            var guarantors = await client
                .Table<Entities.Guarantor>()
                .Get(cancellationToken);
            return guarantors.Models.Adapt<Models.Get.Guarantor[]>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Guarantor[]>(e.Message);
        }
    }
    
    public async ValueTask<Result<Models.Get.Guarantor>> GetGuarantor(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var guarantor = await client
                .Table<Entities.Guarantor>()
                .Where(x => x.Id == id)
                .Single(cancellationToken);
            return guarantor is null
                ? Result.Failure<Models.Get.Guarantor>("Guarantor not found")
                : guarantor.Adapt<Models.Get.Guarantor>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Guarantor>(e.Message);
        }
    }
    
    public async ValueTask<Result<Models.Get.Guarantor>> CreateGuarantor(Models.Update.Guarantor contract, CancellationToken cancellationToken)
    {
        try
        {
            var newGuarantor = await client
                .Table<Entities.Guarantor>()
                .Insert(contract.Adapt<Entities.Guarantor>(), cancellationToken: cancellationToken);
            return newGuarantor.Model is null
                ? Result.Failure<Models.Get.Guarantor>("Failed to create guarantor")
                : newGuarantor.Model.Adapt<Models.Get.Guarantor>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Guarantor>(e.Message);
        }
    }
    
    public async ValueTask<Result<Models.Get.Guarantor>> UpdateGuarantor(Guid id, Models.Update.Guarantor contract, CancellationToken cancellationToken)
    {
        try
        {
            var updatedGuarantor = await client
                .Table<Entities.Guarantor>()
                .Where(x => x.Id == id)
                .Update(contract.Adapt<Entities.Guarantor>(), cancellationToken: cancellationToken);
            return updatedGuarantor.Model is null
                ? Result.Failure<Models.Get.Guarantor>("Failed to update guarantor")
                : updatedGuarantor.Model.Adapt<Models.Get.Guarantor>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Guarantor>(e.Message);
        }
    }
    
    public async ValueTask<Result> DeleteGuarantor(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await client
                .Table<Entities.Guarantor>()
                .Where(x => x.Id == id)
                .Delete(cancellationToken: cancellationToken);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Guarantor>(e.Message);
        }
    }
}