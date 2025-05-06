using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RentalControl.Models.Get;

namespace RentalControl.Services;

public class ContractGenerationService(Contract contract, string landlord)
{
    private Contract Contract { get; set; } = contract;

    public Document GenerateContract()
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Legal);
                page.Margin(1, Unit.Centimetre);

                page.Header().Text("CONTRATO DE ARRENDAMIENTO").ExtraBold().FontSize(16).AlignCenter();
                page.Content()
                    .Text($"CONTRATO de arrendamiento que celebran por una parte: {landlord}, en su carácter de arrendador y por la otra parte: {Contract.Tenant.Name}, en su carácter de arrendatario, al tenor de las siguientes declaraciones y cláusulas:".ToUpper())
                    .FontSize(12);
            });
        });
    }
}