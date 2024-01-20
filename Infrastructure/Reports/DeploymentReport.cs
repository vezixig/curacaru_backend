namespace Curacaru.Backend.Infrastructure.Reports;

using Core.Entities;
using Core.Enums;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

internal static class DeploymentReport
{
    public static Document Create(Company company, Customer customer, InsuranceStatus insuranceStatus)
    {
        Document document = new();
        document.Info.Title = "Einsatznachweise";

        DefineStyles(document);

        CreatePage(document);
        AddHeader(document.Sections[0], company);

        document.Sections[0].AddParagraph("Einsatznachweis Monat/Jahr: ___________________________", "H1");

        if (insuranceStatus != InsuranceStatus.SelfPay) AddServiceType(document);

        AddCustomerInfo(document, customer, insuranceStatus);
        AddTimeTable(document, insuranceStatus);
        AddSignatures(document, company, insuranceStatus);
        return document;
    }

    private static void AddCustomerInfo(Document document, Customer customer, InsuranceStatus insuranceStatus)
    {
        document.Sections[0].AddParagraph();
        document.Sections[0].AddParagraph("Name des Kunden: ").AddText($"{customer.FirstName} {customer.LastName}");
        if (insuranceStatus != InsuranceStatus.SelfPay)
        {
            document.Sections[0].AddParagraph("Leistunsgsträger: ").AddText(customer.Insurance?.Name ?? "");
            document.Sections[0].AddParagraph("Versichertennummer: ").AddText(customer.InsuredPersonNumber);
            document.Sections[0].AddParagraph("Pflegegrad: ").AddText(customer.CareLevel.ToString());
        }

        document.Sections[0].AddParagraph("Geburtsdatum:").AddText(customer.BirthDate.ToString("dd.MM.yyyy"));
        document.Sections[0].AddParagraph("Anschrift: ").AddText($"{customer.Street} · {customer.ZipCode} {customer.ZipCity?.City ?? ""}");
        document.Sections[0].AddParagraph();
    }

    private static void AddHeader(Section section, Company company)
    {
        // PageHeight Top/Bottom because of landscape
        var columnWidth = (section.PageSetup.PageHeight - section.PageSetup.TopMargin - section.PageSetup.BottomMargin) / 2;

        var headerTable = section.Headers.Primary.AddTable();
        headerTable.Borders.Visible = false;
        var leftColumn = headerTable.AddColumn(columnWidth);
        leftColumn.Format.Alignment = ParagraphAlignment.Left;
        var rightColumn = headerTable.AddColumn(columnWidth);
        rightColumn.Format.Alignment = ParagraphAlignment.Left;

        var row1 = headerTable.AddRow();
        row1.Cells[0].AddParagraph().AddFormattedText($"Anbieter: {(company.Name == "" ? company.OwnerName : company.Name)}", "Table");
        row1.Cells[1].AddParagraph().AddFormattedText($"Steuernummer: {company.TaxNumber}", "Table");

        var row2 = headerTable.AddRow();
        row2.Cells[0].AddParagraph().AddFormattedText($"Adresse: {company.Street} · {company.ZipCode} {company.ZipCity?.City}", "Table");
        row2.Cells[1].AddParagraph().AddFormattedText($"IK-Nummer: {company.InstitutionCode}", "Table");
    }

    private static void AddServiceType(Document document)
    {
        var table = document.Sections[0].AddTable();
        table.Format.SpaceBefore = "0.3cm";
        table.Format.SpaceAfter = "0.3cm";
        table.Borders.Visible = true;
        table.AddColumn("2.82cm");
        table.AddColumn("6.19cm");
        table.AddColumn("6.44cm");
        table.AddColumn("9.33cm");

        var row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Leistungsart", TextFormat.Bold);

        row.Cells[1].AddParagraph("[ ] Entlastungsbetrag § 45b SGB XI");
        row.Cells[2].AddParagraph("[ ] Verhinderungspflege § 39 SGB XI");
        row.Cells[3].AddParagraph("[ ] ___% Pflegesachleistungen § 36 SGB XI (max. 40%)");
    }

    private static void AddSignatures(Document document, Company company, InsuranceStatus insuranceStatus)
    {
        // add empty paragraph to lazily create space
        document.Sections[0].AddParagraph();

        if (insuranceStatus == InsuranceStatus.Statutory)
            document.Sections[0].AddParagraph($"Ich habe meine Leistungen gemäß §45b SGB XI an {company.Name} abgetreten.");

        var info = document.Sections[0].AddParagraph("Hiermit bestätige ich den Einsatz und die Richtigkeit der oben angegebenen Einsatzzeit.");
        info.Format.SpaceAfter = "1.5cm";

        var table = document.Sections[0].AddTable();
        table.AddColumn("10.0cm");
        table.AddColumn("2.0cm");
        table.AddColumn("10.cm");

        var row = table.AddRow();

        row.Cells[0].AddParagraph("Ort, Datum, Unterschrift");
        row.Cells[0].AddParagraph().AddFormattedText("-leistungserbringende Person-", TextFormat.Italic);
        row.Cells[0].Borders.Top.Visible = true;

        row.Cells[2].AddParagraph("Ort, Datum, Unterschrift").Format.Font.Size = "9";
        row.Cells[2].AddParagraph().AddFormattedText("-Kunde/Kundin-", TextFormat.Italic).Size = "9";
        row.Cells[2].Borders.Top.Visible = true;
    }

    private static void AddTimeTable(Document document, InsuranceStatus insuranceStatus)
    {
        var tableDates = document.Sections[0].AddTable();
        tableDates.Borders.Visible = true;
        tableDates.Rows.Height = "0.85cm";
        tableDates.AddColumn("3.5cm");
        tableDates.AddColumn("4.0cm");
        tableDates.AddColumn("2.2cm");
        tableDates.AddColumn("8.0cm");
        tableDates.AddColumn("3.5cm");
        tableDates.AddColumn("3.5cm");

        var headerRow = tableDates.AddRow();
        headerRow.Height = "0.4cm";
        headerRow.Cells[0].AddParagraph("Datum").Style = "ThCenter";
        headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
        headerRow.Cells[0].MergeDown = 1;

        headerRow.Cells[1].AddParagraph("Uhrzeit, von - bis").Style = "ThCenter";
        headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Center;
        headerRow.Cells[1].MergeDown = 1;

        headerRow.Cells[2].AddParagraph("Std.").Style = "ThCenter";
        headerRow.Cells[2].VerticalAlignment = VerticalAlignment.Center;
        headerRow.Cells[2].MergeDown = 1;

        headerRow.Cells[3].AddParagraph("Name der leistungserbringenden Person").Style = "ThCenter";
        headerRow.Cells[3].VerticalAlignment = VerticalAlignment.Center;
        headerRow.Cells[3].MergeDown = 1;

        headerRow.Cells[4].AddParagraph("Unterschrift je Einsatz").Style = "ThCenter";
        headerRow.Cells[4].VerticalAlignment = VerticalAlignment.Center;
        headerRow.Cells[4].Borders.Bottom.Visible = false;

        headerRow.Cells[4].MergeRight = 1;

        var subHeaderRow = tableDates.AddRow();
        subHeaderRow.Height = "0.4cm";
        subHeaderRow.Cells[4].AddParagraph("Mitarbeiter").Style = "ThCenter";
        subHeaderRow.Cells[4].VerticalAlignment = VerticalAlignment.Center;
        subHeaderRow.Cells[4].Borders.Top.Visible = false;
        subHeaderRow.Cells[4].Borders.Right.Visible = false;

        subHeaderRow.Cells[5].AddParagraph("Kunde").Style = "ThCenter";
        subHeaderRow.Cells[5].VerticalAlignment = VerticalAlignment.Center;
        subHeaderRow.Cells[5].Borders.Top.Visible = false;
        subHeaderRow.Cells[5].Borders.Left.Visible = false;

        tableDates.AddRow();
        tableDates.AddRow();
        tableDates.AddRow();
        tableDates.AddRow();
        if (insuranceStatus == InsuranceStatus.SelfPay)
        {
            tableDates.AddRow();
            tableDates.AddRow();
            tableDates.AddRow();
        }

        var footerRow = tableDates.AddRow();
        footerRow.Cells[0].AddParagraph("Gesamtstunden").Style = "ThRight";
        footerRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;

        footerRow.Cells[0].MergeRight = 1;
        footerRow.Cells[0].Borders.Left.Visible = false;
        footerRow.Cells[0].Borders.Bottom.Visible = false;
        footerRow.Cells[0].Borders.Bottom.Visible = false;

        footerRow.Cells[3].MergeRight = 2;
        footerRow.Cells[3].Borders.Bottom.Visible = false;
        footerRow.Cells[5].Borders.Right.Visible = false;
    }

    private static void CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.Orientation = Orientation.Landscape;
    }

    private static void DefineStyles(Document document)
    {
        // Get the predefined style Normal.
        var style = document.Styles["Normal"];

        style.Font.Name = "Arial";
        style.ParagraphFormat.SpaceAfter = "0.08cm";

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
    }
}