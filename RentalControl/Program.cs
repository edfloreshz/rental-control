using Carter;
using MudBlazor.Services;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RentalControl.Components;
using Scalar.AspNetCore;
using Supabase.Postgrest;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.Legal);
        page.Margin(1, Unit.Centimetre);
        page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

        page.Header().Text("CONTRATO DE ARRENDAMIENTO")
            .ExtraBold()
            .FontSize(10)
            .AlignCenter();

        page.Content()
            .PaddingVertical(10)
            .Column(x =>
            {
                x.Spacing(5);
                x.Item()
                    .Text(
                        "CONTRATO DE ARRENDAMIENTO QUE CELEBRAN POR UNA PARTE: EDUARDO FLORES VILLEGAS, (PROPIETARIO) QUE EN LO SUCESIVO SERA DENOMINADO “EL ARRENDADOR” Y POR LA OTRA PARTE: MARIA DE LOS ANGELES AMADOR JUAREZ QUE EN LO SUCESIVO SERA DENOMINADO “EL ARRENDATARIO”, AL TENOR DE LAS SIGUIENTES DECLARACIONES Y CLAUSULAS:");
                x.Item()
                    .Text("DECLARACIONES".ToUpper())
                    .ExtraBold()
                    .AlignCenter();
                x.Item()
                    .Text(
                        "1.- Declara “EL ARRENDADOR” que es el legítimo propietario y se encuentra en posesión del Inmueble ubicado en: Mazatán No. 439 Esquina Simón Bley Col. CNOP, Hermosillo, Sonora. que en adelante será llamado “EL INMUEBLE”, que es su deseo dar en Arrendamiento “EL INMUEBLE” bajo los términos y condiciones que se mencionan en el presente contrato");
                x.Item()
                    .Text(
                        "2.- “EL ARRENDATARIO: MARIA DE LOS ANGELES AMADOR JUAREZ declara que es una persona con capacidad suficiente para obligarse en los términos del presente contrato, y que tiene su domicilio particular en: Ave. San Pedro No. 478 entre Lopez del castillo y Olivares, Col. Los Jardines, Hermosillo. Cel. 6624 29-37-39, que es su deseo Arrendar “EL INMUEBLE” en los términos y condiciones que se mencionan en este contrato y que recibe de Conformidad “EL INMUEBLE” con todas sus instalaciones completas y en servicio a plena satisfacción.");
                x.Item()
                    .Text("REFERENCIAS".ToUpper())
                    .ExtraBold()
                    .AlignCenter();
                x.Item()
                    .Text(
                        "CON VIRTUD DE LO MANIFESTADO EN LAS ANTERIORES DECLARACIONES, CONVIENEN SUJETARSE A LAS SIGUIENTES:");
                x.Item()
                    .Text("CLÁUSULAS".ToUpper())
                    .ExtraBold()
                    .AlignCenter();
                x.Item()
                    .Text(
                        "PRIMERA: “EL ARRENDADOR” otorga en arrendamiento el EL INMUEBLE a EL ARRENDATARIO, y éste lo recibe a su entera satisfacción, para local de: TORTILLERIA, en buen estado con todas sus instalaciones funcionando y en servicio.");
                x.Item()
                    .Text(
                        "SEGUNDA: La renta mensual que el ARRENDATARIO deberá pagar a partir del día: fecha desde la cual estará vigente este contrato, es la Cantidad de:$ (4,350.00 Son Cuatro mil Trescientos Cincuenta pesos 00/100 MN más un mes de Depósito. Quedando en el entendido de que la renta se pagará cada mes, íntegra y puntualmente el día señalado aún cuando el ARRENDATARIO lo ocupe una parte del mes (o incluso si no lo ocupa)");
                x.Item()
                    .Text(
                        "TERCERA.- Queda expresamente pactado que las rentas se incrementarán automáticamente de forma acumulativa en forma anual, ajustandose las mismas a la variación que haya sufrido el Indice Nacional de precios al consumidor que publica el Banco Nacional de México a través del diario oficial de la federación o en el salario mínimo respecto a los últimos doce meses inmediatos anteriores al mes en que deba realizarse el ajuste al precio de la renta, el que sea mayor.");
                x.Item()
                    .Text(
                        "CUARTA: La vigencia del presente contrato será de: 1 Año plazo convenido por ambas partes, a partir del día 01 de ABRIL de 2025 al 31 de MARZO de 2026 . Al término de dicho plazo de vigencia, EL ARRENDATARIO se obliga a hacer entrega a EL ARRENDADOR el INMUEBLE arrendado, en las condiciones en las cuales lo recibio, todo en buen estado (pisos, paredes, pintura, muebles de baño, cristales, cortinas metálicas etc.) y estando al corriente en todos los pagos de Servicios, tales como Luz (electricidad) y Agua, de los cuales deberá entregar AL ARRENDADOR, los recibos correspondientes totalmente pagados. En caso de que EL ARRENDATARIO no entregará el INMUEBLE a EL ARRENDADOR, al término del presente contrato, EL ARRENDATA- RIO pagará a EL ARRENDADOR, a partir del siguiente mes por concepto de renta mensual, la cantidad pactada, más un incremento del 6 % mensual por el número de meses que transcurran hasta la firma de renovación del contrato.");
                x.Item()
                    .Text(
                        "QUINTA: A efecto de garantizar todas y cada una de las obligaciones que se derivan del presente contrato, EL ARRENDATARIO hace entrega al momento de la firma del mismo, La Cantidad de $ ( 6,700.00 Son Seis Mil Setecientos pesos 00/100 MN por concepto de Depósito en garantía. Suma que se obliga EL ARRENDADOR a devolver a EL ARRENDATARIO a más tardar en 7 (siete) días después de la desocupación del INMUEBLE, siempre y cuando EL ARRENDATARIO lo entregue en el mismo estado en que lo recibió, y previa comprobación (con recibos pagados) de que no existe ningún adeudo derivado de los servicios de Luz y Agua potable, quedando aclarado que el mes de depósito, no se utilizará como pago de renta, es única y exclusivamente para garantizar reparaciones o adeudos pendientes del ARREDATARIO y se regresará después de verificar que no exista ningún pendiente por liquidar.");
                x.Item()
                    .Text(
                        "RESCISION DE CONTRATO: EL ARRENDADOR podrá rescindir el presente Contrato, SIN NECESIDAD DE DECLARACION JUDICIAL, por simple Notificación por escrito, por una o más de las siguientes causas:");
                x.Item().Text("1.- Si EL ARRENDATARIO se RETRASA en el pago de 1 a 2 meses consecutivos de renta.");
                x.Item().Text("2.- Por causar daños al INMUEBLE.");
                x.Item().Text(
                    "3.- Si le son suspendidos al INMUEBLE los servicios de Luz o Agua por falta de pago de parte del ARRENDATARIO.");
                x.Item().Text("4.- Por Subarrendar el INMUEBLE.");
                x.Item().Text("5.- Si el ARRENDATARIO deja de ser solvente.");
                x.Item().Text("6.- Por incumplimiento de cualquiera de las cláusulas del presente contrato.");
                x.Item()
                    .Text(
                        "SEXTA: EL ARRENDADOR NO SE HACE responsable por deterioro o pérdida de los bienes muebles que el ARRENDATARIO tenga en el INMUEBLE en cualquiera de los siguientes casos: robo, incendio, terremoto, inundación, etc. ni por lesiones fisicas ocasionadas a personas dentro del inmueble.");
                x.Item()
                    .Text(
                        "SEPTIMA: A la fecha del vencimiento del presente contrato, previa a la desocupación del INMUEBLE, EL ARRENDADOR hará una inspección del mismo, para verificar el estado en el que se encuentre, de existir desperfectos causados por EL ARRENDATARIO, éste se obliga a efectuar las reparaciones pertinentes de forma inmediata, de lo contrario EL ARRENDADOR podrá hacerlos con el Depósito en garantía, siempre y cuando cubra el importe total de dichas reparaciones.");
                x.Item()
                    .Text(
                        "IMPORTANTE: En caso de entregar el local antes de la fecha de vencimiento de su contrato, SE PIERDE EL MES DE DEPOSITO y se tiene que entregar el local en las condiciones en que lo recibió, PINTADO Y RESANADO por FUERA Y POR DENTRO en color claro (blanco o beige).");
                x.Item()
                    .Text(
                        "Entregar RECIBO DE LUZ dado de BAJA y SIN ADEUDO a la fecha de entrega, y estar al corriente en pago de agua.");
                x.Item()
                    .Text(
                        "EL DIA DE PAGO ES EL DIA PRIMERO DE CADA MES, EN CASO DE RETRASO EN EL PAGO DE LA RENTA SE COBRARAN 150.00 DE RECARGOS POR MES (A partir del día 3 se cobra el recargo)")
                    .FontColor(Colors.Red.Medium);
            });
    });
}).ShowInCompanion();

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMediator();
builder.Services.AddCarter();
builder.Services.AddMudServices();

builder.Services.AddSingleton(new Client(builder.Configuration["Postgrest:Url"]!));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapCarter();

app.Run();