using CSharpFunctionalExtensions;
using Mapster;
using Supabase.Postgrest;

namespace RentalControl.Services;

public class ContractService(Client client)
{
    public async ValueTask<Result<Models.Get.Contract[]>> GetContracts(CancellationToken cancellationToken)
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

    public async ValueTask<Result<Models.Get.Contract>> GetContract(Guid id, CancellationToken cancellationToken)
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

    public async ValueTask<Result<Models.Get.Contract>> CreateContract(Models.Update.Contract contract,
        CancellationToken cancellationToken)
    {
        try
        {
            var newContract = await client
                .Table<Entities.Contract>()
                .Insert(contract.Adapt<Entities.Contract>(), cancellationToken: cancellationToken);
            return newContract.Model is null
                ? Result.Failure<Models.Get.Contract>("Failed to create contract")
                : newContract.Model.Adapt<Models.Get.Contract>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Contract>(e.Message);
        }
    }

    public async ValueTask<Result<Models.Get.Contract>> UpdateContract(Guid id, Models.Update.Contract contract,
        CancellationToken cancellationToken)
    {
        try
        {
            var updatedContract = await client
                .Table<Entities.Contract>()
                .Where(x => x.Id == id)
                .Update(contract.Adapt<Entities.Contract>(), cancellationToken: cancellationToken);
            return updatedContract.Model is null
                ? Result.Failure<Models.Get.Contract>("Failed to update contract")
                : updatedContract.Model.Adapt<Models.Get.Contract>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Contract>(e.Message);
        }
    }

    public async ValueTask<Result> DeleteContract(Guid id, CancellationToken cancellationToken)
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