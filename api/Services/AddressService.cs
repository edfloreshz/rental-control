using CSharpFunctionalExtensions;
using Mapster;
using RentalControl.Interfaces;
using Supabase.Postgrest;

namespace RentalControl.Services;

public class AddressService(Client client)
    : ICrudService<Models.Get.Address, Models.Create.Address, Models.Update.Address>
{
    public async ValueTask<Result<Models.Get.Address[]>> GetAll(CancellationToken cancellationToken)
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

    public async ValueTask<Result<Models.Get.Address>> Get(Guid id, CancellationToken cancellationToken)
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

    public async ValueTask<Result<Models.Get.Address>> Create(Models.Create.Address data,
        CancellationToken cancellationToken)
    {
        try
        {
            var newAddress = await client
                .Table<Entities.Address>()
                .Insert(data.Adapt<Entities.Address>(), cancellationToken: cancellationToken);
            return newAddress.Model is null
                ? Result.Failure<Models.Get.Address>("Failed to create address")
                : newAddress.Model.Adapt<Models.Get.Address>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Address>(e.Message);
        }
    }

    public async ValueTask<Result<Models.Get.Address>> Update(Models.Update.Address data,
        CancellationToken cancellationToken)
    {
        try
        {
            var updatedAddress = await client
                .Table<Entities.Address>()
                .Where(x => x.Id == data.Id)
                .Update(data.Adapt<Entities.Address>(), cancellationToken: cancellationToken);
            return updatedAddress.Model is null
                ? Result.Failure<Models.Get.Address>("Failed to update address")
                : updatedAddress.Model.Adapt<Models.Get.Address>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Address>(e.Message);
        }
    }

    public async ValueTask<Result> Delete(Guid id, CancellationToken cancellationToken)
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