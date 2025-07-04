using CSharpFunctionalExtensions;
using Mapster;
using RentalControl.Interfaces;
using Supabase.Postgrest;

namespace RentalControl.Services;

public class GuarantorService(Client client) : ICrudService<Models.Get.Guarantor, Models.Create.Guarantor, Models.Update.Guarantor>
{
    public async ValueTask<Result<Models.Get.Guarantor[]>> GetAll(CancellationToken cancellationToken)
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
    
    public async ValueTask<Result<Models.Get.Guarantor>> Get(Guid id, CancellationToken cancellationToken)
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
    
    public async ValueTask<Result<Models.Get.Guarantor>> Create(Models.Create.Guarantor data, CancellationToken cancellationToken)
    {
        try
        {
            var newGuarantor = await client
                .Table<Entities.Guarantor>()
                .Insert(data.Adapt<Entities.Guarantor>(), cancellationToken: cancellationToken);
            return newGuarantor.Model is null
                ? Result.Failure<Models.Get.Guarantor>("Failed to create guarantor")
                : newGuarantor.Model.Adapt<Models.Get.Guarantor>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Guarantor>(e.Message);
        }
    }
    
    public async ValueTask<Result<Models.Get.Guarantor>> Update(Models.Update.Guarantor data, CancellationToken cancellationToken)
    {
        try
        {
            var updatedGuarantor = await client
                .Table<Entities.Guarantor>()
                .Where(x => x.Id == data.Id)
                .Update(data.Adapt<Entities.Guarantor>(), cancellationToken: cancellationToken);
            return updatedGuarantor.Model is null
                ? Result.Failure<Models.Get.Guarantor>("Failed to update guarantor")
                : updatedGuarantor.Model.Adapt<Models.Get.Guarantor>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Guarantor>(e.Message);
        }
    }
    
    public async ValueTask<Result> Delete(Guid id, CancellationToken cancellationToken)
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