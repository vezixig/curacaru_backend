namespace Curacaru.Backend.Infrastructure.Services.Implementations;

using System.Globalization;
using Core.Entities;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;
using Reports;
using WorkingTimeReport = Reports.WorkingTimeReport;

public class ReportService : IReportService
{
    public byte[] CreateAssignmentDeclaration(Company company, AssignmentDeclaration assignmentDeclaration)
    {
        InitFont();
        var document = DeclarationOfAssignment.Create(company, assignmentDeclaration);
        return RenderDocument(document);
    }

    public byte[] CreateDeploymentReport(Company company, DeploymentReport report)
    {
        InitFont();
        var document = DeploymentReportDocument.Create(company, report);
        return RenderDocument(document);
    }

    public byte[] CreateInvoiceDocument(Company company, Invoice invoice)
    {
        InitFont();
        var document = InvoiceDocument.Create(company, invoice);
        return RenderDocument(document);
    }

    public byte[] GenerateWorkingHoursReport(Core.Entities.WorkingTimeReport report, List<Appointment> appointments)
    {
        InitFont();
        var document = WorkingTimeReport.Create(report, appointments);
        return RenderDocument(document);
    }

    private static void InitFont()
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de");

        if (Capabilities.Build.IsCoreBuild) GlobalFontSettings.FontResolver ??= new FailsafeFontResolver();
    }

    private static byte[] RenderDocument(Document document)
    {
        PdfDocumentRenderer pdfRenderer = new() { Document = document };
        pdfRenderer.RenderDocument();

        MemoryStream outputStream = new();
        pdfRenderer.Save(outputStream, true);
        return outputStream.ToArray();
    }
}