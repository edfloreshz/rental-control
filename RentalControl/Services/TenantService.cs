using CSharpFunctionalExtensions;
using Mapster;
using RentalControl.Interfaces;
using Supabase.Postgrest;

namespace RentalControl.Services;

public class TenantService(Client client) : ICrudService<Models.Get.Tenant, Models.Create.Tenant, Models.Update.Tenant>
{
    public async ValueTask<Result<Models.Get.Tenant[]>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var tenants = await client
                .Table<Entities.Tenant>()
                .Get(cancellationToken);
            return tenants.Models.Adapt<Models.Get.Tenant[]>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Tenant[]>(e.Message);
        }
    }
    
    public async ValueTask<Result<Models.Get.Tenant>> Get(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var tenant = await client
                .Table<Entities.Tenant>()
                .Where(x => x.Id == id)
                .Single(cancellationToken);
            return tenant is null
                ? Result.Failure<Models.Get.Tenant>("Tenant not found")
                : tenant.Adapt<Models.Get.Tenant>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Tenant>(e.Message);
        }
    }
    
    public async ValueTask<Result<Models.Get.Tenant>> Create(Models.Create.Tenant data, CancellationToken cancellationToken)
    {
        try
        {
            var newTenant = await client
                .Table<Entities.Tenant>()
                .Insert(data.Adapt<Entities.Tenant>(), cancellationToken: cancellationToken);
            return newTenant.Model is null
                ? Result.Failure<Models.Get.Tenant>("Failed to create tenant")
                : newTenant.Adapt<Models.Get.Tenant>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Tenant>(e.Message);
        }
    }
    
    public async ValueTask<Result<Models.Get.Tenant>> Update(Models.Update.Tenant data, CancellationToken cancellationToken)
    {
        try
        {
            var updatedTenant = await client
                .Table<Entities.Tenant>()
                .Where(x => x.Id == data.Id)
                .Update(data.Adapt<Entities.Tenant>(), cancellationToken: cancellationToken);
            return updatedTenant.Model is null
                ? Result.Failure<Models.Get.Tenant>("Failed to update tenant")
                : updatedTenant.Adapt<Models.Get.Tenant>();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Tenant>(e.Message);
        }
    }
    
    public async ValueTask<Result> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await client
                .Table<Entities.Tenant>()
                .Where(x => x.Id == id)
                .Delete(cancellationToken: cancellationToken);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure<Models.Get.Tenant>(e.Message);
        }
    }
}