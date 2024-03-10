namespace Curacaru.Backend.Infrastructure.Services.Implementations;

using Core.Entities;
using Core.Enums;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;
using Reports;
using WorkingTimeReport = Reports.WorkingTimeReport;

public class ReportService : IReportService
{
    public byte[] CreateAssignmentDeclaration(Company company, Customer customer, int year)
    {
        InitFont();
        var document = DeclarationOfAssignment.Create(company, customer, year);
        return RenderDocument(document);
    }

    public byte[] CreateDeploymentReport(Company company, Customer customer, InsuranceStatus insuranceStatus)
    {
        InitFont();
        var document = DeploymentReport.Create(company, customer, insuranceStatus);
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