```csharp
app.MapGet("invoice-report", async (InvoiceFactory invoiceFactory) => {
    Invoice invoie = invoiceFactory.Create();

    var html = await RazorTemplateEngine.RenderAsync(
        "Views/InvoiceReport.cshtml", invoice
    );

    ChromePdfRenderer renderer = new();

    using var pdfDoc = renderer.RenderHtmlAsPdf(html);

    return Results.File(pdfDoc.BinaryData, "application/pdf", $"invoice-{invoice.ID}.pdf");
});
```