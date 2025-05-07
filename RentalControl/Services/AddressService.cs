using CSharpFunctionalExtensions;
using Mapster;
using Supabase.Postgrest;

namespace RentalControl.Services;

public class AddressService(Client client)
{
    public async ValueTask<Result<Models.Get.Address[]>> GetAddresses(CancellationToken cancellationToken)
    {
        try
        {
            var contracts = await client
                .Table<Entities.Address>()
                .Get(cancellationToken);
            return contracts.Models.Adapt<Models.Get.Address[]>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Address[]>(e.Message);
        }
    }

    public async ValueTask<Result<Models.Get.Address>> GetAddress(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var contract = await client
                .Table<Entities.Address>()
                .Where(x => x.Id == id)
                .Single(cancellationToken);
            return contract is null
                ? Result.Failure<Models.Get.Address>("Address not found")
                : contract.Adapt<Models.Get.Address>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Address>(e.Message);
        }
    }

    public async ValueTask<Result<Models.Get.Address>> CreateAddress(Models.Update.Address contract,
        CancellationToken cancellationToken)
    {
        try
        {
            var newAddress = await client
                .Table<Entities.Address>()
                .Insert(contract.Adapt<Entities.Address>(), cancellationToken: cancellationToken);
            return newAddress.Model is null
                ? Result.Failure<Models.Get.Address>("Failed to create address")
                : newAddress.Model.Adapt<Models.Get.Address>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Address>(e.Message);
        }
    }

    public async ValueTask<Result<Models.Get.Address>> UpdateAddress(Guid id, Models.Update.Address contract,
        CancellationToken cancellationToken)
    {
        try
        {
            var updatedAddress = await client
                .Table<Entities.Address>()
                .Where(x => x.Id == id)
                .Update(contract.Adapt<Entities.Address>(), cancellationToken: cancellationToken);
            return updatedAddress.Model is null
                ? Result.Failure<Models.Get.Address>("Failed to update address")
                : updatedAddress.Model.Adapt<Models.Get.Address>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Address>(e.Message);
        }
    }

    public async ValueTask<Result> DeleteAddress(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await client
                .Table<Entities.Address>()
                .Where(x => x.Id == id)
                .Delete(cancellationToken: cancellationToken);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Address>(e.Message);
        }
    }
}