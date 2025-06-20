using CSharpFunctionalExtensions;
using Mapster;
using QuestPDF.Fluent;
using RentalControl.Interfaces;
using Supabase.Postgrest;

namespace RentalControl.Services;

public class ContractService(Client client)
    : ICrudService<Models.Get.Contract, Models.Create.Contract, Models.Update.Contract>
{
    public async ValueTask<Result<Document>> GeneratePdf(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("PDF generation is not implemented yet.");
    }
    
    public async ValueTask<Result<Models.Get.Contract>> Get(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var contract = await client
                .Table<Entities.Contract>()
                .Where(x => x.Id == id)
                .Single(cancellationToken);
            return contract is null
                ? Result.Failure<Models.Get.Contract>("Contract not found")
                : contract.Adapt<Models.Get.Contract>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Contract>(e.Message);
        }
    }

    public async ValueTask<Result<Models.Get.Contract[]>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var contracts = await client
                .Table<Entities.Contract>()
                .Get(cancellationToken);
            return contracts.Models.Adapt<Models.Get.Contract[]>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Contract[]>(e.Message);
        }
    }

    public async ValueTask<Result<Models.Get.Contract>> Create(Models.Create.Contract data,
        CancellationToken cancellationToken)
    {
        try
        {
            var newContract = await client
                .Table<Entities.Contract>()
                .Insert(data.Adapt<Entities.Contract>(), cancellationToken: cancellationToken);
            return newContract.Model is null
                ? Result.Failure<Models.Get.Contract>("Failed to create contract")
                : newContract.Model.Adapt<Models.Get.Contract>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Contract>(e.Message);
        }
    }

    public async ValueTask<Result<Models.Get.Contract>> Update(Models.Update.Contract data,
        CancellationToken cancellationToken)
    {
        try
        {
            var updatedContract = await client
                .Table<Entities.Contract>()
                .Where(x => x.Id == data.Id)
                .Update(data.Adapt<Entities.Contract>(), cancellationToken: cancellationToken);
            return updatedContract.Model is null
                ? Result.Failure<Models.Get.Contract>("Failed to update contract")
                : updatedContract.Model.Adapt<Models.Get.Contract>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Contract>(e.Message);
        }
    }

    public async ValueTask<Result> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await client
                .Table<Entities.Contract>()
                .Where(x => x.Id == id)
                .Delete(cancellationToken: cancellationToken);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Contract>(e.Message);
        }
    }
}