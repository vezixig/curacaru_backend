namespace Curacaru.Backend.Infrastructure.Reports;

using System.Globalization;
using Core.Entities;
using Core.Enums;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;

internal static class InvoiceDocument
{
    public static Document Create(Company company, Invoice invoice)
    {
        Document document = new();
        document.Info.Title = "Abtretungserklärung";

        DefineStyles(document);
        CreatePage(document);

        AddCompanyData(document, company);

        var section = document.Sections[0];
        AddAddress(company, invoice, section);

        var p = section.AddParagraph("Rechnung", "H1");
        p.Format.SpaceBefore = "3cm";

        var frame = section.AddTextFrame();
        frame.FillFormat.Color = Colors.LightGray;
        frame.LineFormat.Style = LineStyle.Single;
        frame.LineFormat.Width = 0.75;

        frame.Width = document.Sections[0].PageSetup.PageWidth - document.Sections[0].PageSetup.LeftMargin - document.Sections[0].PageSetup.RightMargin;
        frame.Height = "1.4cm";
        frame.MarginLeft = "0.2cm";
        frame.MarginTop = "0.2cm";

        frame.AddParagraph().AddFormattedText("Rechnung-Nr: ", TextFormat.Bold).AddFormattedText(invoice.InvoiceNumber, TextFormat.NotBold);
        frame.AddParagraph().AddFormattedText("Rechnungsdatum: ", TextFormat.Bold).AddFormattedText(invoice.InvoiceDate.ToString("dd.MM.yyyy"), TextFormat.NotBold);

        if (invoice.DeploymentReport.ClearanceType != ClearanceType.SelfPayment)
        {
            section.AddParagraph("Name des Kunden: ").AddText(invoice.DeploymentReport.Customer.FullName);
            section.AddParagraph("Versichertennummer: ").AddText(invoice.DeploymentReport.Customer.InsuredPersonNumber);
            section.AddParagraph("Pflegegrad: ").AddText(invoice.DeploymentReport.CareLevel.ToString());
            section.AddParagraph("Geburtsdatum: ").AddText(invoice.DeploymentReport.Customer.BirthDate.ToString("dd.MM.yyyy"));
            section.AddParagraph("Anschrift: ")
                .AddText(
                    $"{invoice.DeploymentReport.Customer.Street} · {invoice.DeploymentReport.Customer.ZipCity?.ZipCode} {invoice.DeploymentReport.Customer.ZipCity?.City}");
        }

        if (invoice.DeploymentReport.ClearanceType != ClearanceType.SelfPayment && invoice.DeploymentReport.CustomerInsuranceStatus == InsuranceStatus.Statutory)
        {
            section.AddParagraph("Sehr geehrte Damen und Herren,").Format.SpaceBefore = "0.5cm";
        }
        else
        {
            var preSalutation = invoice.DeploymentReport.Customer.Salutation == Gender.Female ? "geehrte" : "geehrter";
            var salutation = invoice.DeploymentReport.Customer.Salutation.ToFriendlyString();
            section.AddParagraph($"Sehr {preSalutation} {salutation} {invoice.DeploymentReport.Customer.LastName},").Format.SpaceBefore = "0.5cm";
        }

        var monthName = new DateTime(2000, invoice.DeploymentReport.Month, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("de"));
        p = section.AddParagraph($"wir erlauben uns für {monthName} {invoice.DeploymentReport.Year} folgende Betreuungsleistung in Rechnung zu stellen:");
        p.Format.SpaceBefore = "0.2cm";
        p.Format.SpaceAfter = "0.3cm";

        AddCostsTable(invoice, document);

        p = section.AddParagraph();
        p.Format.SpaceBefore = "0.3cm";
        p.AddText("Nicht umsatzsteuerpflichtig nach §4 Abs. 16 Satz g) UstG.");

        p = section.AddParagraph("Wir bitten Sie, den o.g.Rechnungsbetrag auf das folgende Konto zu überweisen. Zahlbar innerhalb von ");
        p.Format.SpaceBefore = "0.2cm";
        p.AddFormattedText("14 Tagen", TextFormat.Underline);
        p.AddFormattedText(" Bitte geben Sie als ", TextFormat.NoUnderline);
        p.AddFormattedText("Verwendungszweck", TextFormat.Underline);
        p.AddFormattedText(" unbedingt die ", TextFormat.NoUnderline);
        p.AddFormattedText("Rechnungsnummer", TextFormat.Underline);
        p.AddFormattedText(" an. Vielen Dank!", TextFormat.NoUnderline);

        p = section.AddParagraph();
        p.Format.SpaceBefore = "0.2cm";
        p.Format.SpaceAfter = "0.1cm";
        p.AddText("Bankverbindung:");

        section.AddParagraph().AddFormattedText("IBAN ", TextFormat.Bold).AddFormattedText(company.Iban, TextFormat.NotBold);
        section.AddParagraph().AddFormattedText("BIC ", TextFormat.Bold).AddFormattedText(company.Bic, TextFormat.NotBold);

        p = section.AddParagraph();
        p.Format.SpaceBefore = "0.2cm";
        p.AddText("Sollten Sie noch Rückfragen haben, nehmen Sie gerne Kontakt zu uns auf.");

        p = section.AddParagraph();
        p.Format.SpaceBefore = "0.4cm";
        p.Format.SpaceAfter = "0.1cm";

        p.AddText("Mit freundlichen Grüßen.");

        section.AddImage(invoice.Signature![15..].Replace("base64,", "base64:")).Height = 40;
        section.AddParagraph(invoice.SignedEmployee.FullName);

        p = section.Footers.Primary.AddParagraph();
        p.Format.Alignment = ParagraphAlignment.Center;
        p.Format.Font.Size = 9;
        p.AddFormattedText("Ein unterschriebener Einsatznachweis ist dieser Rechnung beigefügt.", TextFormat.Italic);

        return document;
    }

    private static void AddAddress(Company company, Invoice invoice, Section section)
    {
        section.AddParagraph(" ");
        var sender = section.AddParagraph();
        sender.Style = "PSender";
        sender.Format.SpaceBefore = "1.5cm";
        sender.AddFormattedText($"{company.Name} · {company.Street} · {company.ZipCode} {company.ZipCity?.City}", TextFormat.Underline);

        if (invoice.DeploymentReport is { CustomerInsuranceStatus: InsuranceStatus.Statutory, Insurance: not null })
        {
            var insuranceName = section.AddParagraph();
            insuranceName.Format.SpaceBefore = "0.5cm";
            insuranceName.AddText(invoice.DeploymentReport.Insurance.Name);

            section.AddParagraph(invoice.DeploymentReport.Insurance.Street);
            section.AddParagraph(invoice.DeploymentReport.Insurance.ZipCity?.ZipCode + " " + invoice.DeploymentReport.Insurance.ZipCity?.City);
        }
        else
        {
            var customerName = section.AddParagraph();
            customerName.Format.SpaceBefore = "0.5cm";
            customerName.AddText(invoice.DeploymentReport.Customer.FullName);

            section.AddParagraph(invoice.DeploymentReport.Customer.Street);
            section.AddParagraph(invoice.DeploymentReport.Customer.ZipCity?.ZipCode + " " + invoice.DeploymentReport.Customer.ZipCity?.City);
        }
    }

    private static void AddCompanyData(Document document, Company company)
    {
        var section = document.Sections[0].Headers.Primary;

        var table = section.AddTable();
        var columnWidth = (document.Sections[0].PageSetup.PageWidth - document.Sections[0].PageSetup.LeftMargin - document.Sections[0].PageSetup.RightMargin) / 2;
        table.Columns.AddColumn().Width = columnWidth;
        table.Columns.AddColumn().Width = columnWidth;

        var row = table.AddRow();
        row.Cells[0].MergeRight = 1;
        row.Cells[0].AddParagraph().AddFormattedText(company.Name);

        row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText(string.IsNullOrEmpty(company.OwnerName) ? company.Street : company.OwnerName);
        var p = row.Cells[1].AddParagraph();
        p.Format.Alignment = ParagraphAlignment.Right;
        p.AddFormattedText("Steuernummer: ", TextFormat.Bold);
        p.AddFormattedText(company.TaxNumber, TextFormat.NotBold);

        row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText(string.IsNullOrEmpty(company.OwnerName) ? $"{company.ZipCode} {company.ZipCity?.City}" : company.Street);
        p = row.Cells[1].AddParagraph();
        p.Format.Alignment = ParagraphAlignment.Right;
        p.AddFormattedText("IK-Nummer: ", TextFormat.Bold);
        p.AddFormattedText(company.TaxNumber, TextFormat.NotBold);

        row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText(string.IsNullOrEmpty(company.OwnerName) ? "" : $"{company.ZipCode} {company.ZipCity?.City}");
    }

    private static void AddCostsTable(Invoice invoice, Document document)
    {
        var table = document.Sections[0].AddTable();
        table.Rows.LeftIndent = "0.2cm";
        table.Borders.Style = BorderStyle.Single;
        table.Borders.Width = 0.75;

        table.Columns.AddColumn().Width = document.GetPageWidth() / 5 * 2;
        table.Columns.AddColumn().Width = document.GetPageWidth() / 5 * 3;
        table.Rows.Height = "0.7cm";
        table.Rows.VerticalAlignment = VerticalAlignment.Center;

        if (invoice.DeploymentReport.ClearanceType != ClearanceType.SelfPayment)
        {
            var clearanceRow = table.AddRow();
            clearanceRow.Cells[0].AddParagraph().AddFormattedText("Leistungsart:", TextFormat.Bold);
            clearanceRow.Cells[1].AddParagraph().AddText($"{invoice.DeploymentReport.ClearanceType.ToFriendlyString()}");
        }

        var row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Stunden (gesamt):", TextFormat.Bold);
        row.Cells[1].AddParagraph().AddText($"{invoice.WorkedHours:#0.00} Std.");

        row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Stundenlohn:", TextFormat.Bold);
        row.Cells[1].AddParagraph().AddText($"{invoice.HourlyRate:#0.00} €");

        row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Anfahrtskosten:", TextFormat.Bold);
        row.Cells[1].AddParagraph().AddText(invoice.TotalRideCosts == 0 ? "-" : $"{invoice.TotalRideCosts:#0.00} €");

        row = table.AddRow();
        var cell = row.Cells[0];
        cell.Shading.Color = Colors.LightGray;
        cell.AddParagraph().AddFormattedText("Rechnungsbetrag:", TextFormat.Bold);
        cell = row.Cells[1];
        cell.Shading.Color = Colors.LightGray;
        cell.AddParagraph().AddText($"{invoice.InvoiceTotal:#0.00} €");
    }

    private static void CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = "2cm";
        section.PageSetup.Orientation = Orientation.Portrait;
    }

    private static void DefineStyles(Document document)
    {
        // Get the predefined style Normal.
        var style = document.Styles["Normal"];

        style!.Font.Name = "Arial";
        style.ParagraphFormat.SpaceAfter = "0.0cm";

        style = document.Styles.AddStyle("PSender", "Normal");
        style.Font.Size = 8;
        style.ParagraphFormat.SpaceBefore = "2.5cm";

        style = document.Styles.AddStyle("Table", "Normal");
        style.Font.Size = 11;

        style = document.Styles.AddStyle("H1", "Normal");
        style.Font.Size = 15;
        style.Font.Bold = true;
        style.ParagraphFormat.SpaceBefore = "0.5cm";
        style.ParagraphFormat.SpaceAfter = "0.5cm";

        style = document.Styles.AddStyle("ThCenter", "Table");
        style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        style.Font.Bold = true;

        style = document.Styles.AddStyle("ThRight", "Table");
        style.ParagraphFormat.Alignment = ParagraphAlignment.Right;
        style.Font.Bold = true;

        style = document.Styles.AddStyle("PClose", "Normal");
        style.ParagraphFormat.SpaceAfter = "0.0cm";
    }
}