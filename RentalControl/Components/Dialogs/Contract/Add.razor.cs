using CSharpFunctionalExtensions;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RentalControl.Components.Models;

namespace RentalControl.Components.Dialogs.Contract;

public partial class Add(ISender sender, IDialogService dialogService) : ComponentBase
{
    [CascadingParameter]
    public required IMudDialogInstance MudDialog { get; set; }
    private MudForm Form { get; set; } = new();
    private Models.Contract Model { get; set; } = new();
    private Models.Address[] Addresses { get; set; } = [];
    private Models.Tenant[] Tenants { get; set; } = [];
    private bool Success { get; set; }
    private string[] Errors { get; set; } = [];
    
    protected override async Task OnInitializedAsync()
    {
        var addressesResult = await sender.Send(new Endpoints.Addresses.List.Query());
        var tenantsResult = await sender.Send(new Endpoints.Tenant.List.Query());
        
        addressesResult.Match(SetAddresses, Console.WriteLine);
        tenantsResult.Match(SetTenants, Console.WriteLine);
    }
    
    private void SetAddresses(RentalControl.Models.Get.Address[] items)
    {
        Addresses = items.Adapt<Models.Address[]>();
    }
    
    private void SetTenants(RentalControl.Models.Get.Tenant[] items)
    {
        Tenants = items.Adapt<Models.Tenant[]>();
    }

    private void Submit() => MudDialog.Close(DialogResult.Ok(true));

    private void Cancel() => MudDialog.Cancel();
    
    private Task<IDialogReference> OpenAddTenantDialog()
    {
        return dialogService.ShowAsync<Dialogs.Tenant.Add>("Agregar Arrendatario");
    }

    private Task<IDialogReference> OpenAddAddressDialog()
    {
        return dialogService.ShowAsync<Dialogs.Address.Add>("Agregar Dirección");
    }
}