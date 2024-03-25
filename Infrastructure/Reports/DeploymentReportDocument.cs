namespace Curacaru.Backend.Infrastructure.Reports;

using System.Globalization;
using Core.Entities;
using Core.Enums;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

internal static class DeploymentReportDocument
{
    public static Document Create(Company company, DeploymentReport report)
    {
        Document document = new();
        document.Info.Title = "Einsatznachweise";

        DefineStyles(document);

        CreatePage(document);
        AddHeader(document.Sections[0], company);

        document.Sections[0]
            .AddParagraph($"Einsatznachweis {CultureInfo.GetCultureInfo("de", "DE").DateTimeFormat.GetMonthName(report.Month)} {report.Year}", "H1")
            .Format.SpaceAfter = "0.3cm";

        if (report.ClearanceType != ClearanceType.SelfPayment) AddServiceType(document, report);

        AddCustomerInfo(document, report);
        AddTimeTable(document, report);
        AddSignatures(document, company, report);

        var footer = document.Sections[0].Footers.Primary.AddParagraph("Seite ");
        footer.Format.Alignment = ParagraphAlignment.Center;
        footer.AddPageField();
        footer.AddText(" von ");
        footer.AddNumPagesField();

        return document;
    }

    private static void AddCustomerInfo(Document document, DeploymentReport report)
    {
        if (report.ClearanceType == ClearanceType.SelfPayment)
        {
            var tableSelfPay = document.Sections[0].AddTable();
            tableSelfPay.AddColumn("20.0cm");
            var rowSelfPay = tableSelfPay.AddRow();

            var pTop = rowSelfPay.Cells[0].AddParagraph($"Name des Kunden: {report.Customer.FirstName} {report.Customer.LastName}");
            rowSelfPay.Cells[0].AddParagraph("Geburtsdatum: ").AddText(report.Customer.BirthDate.ToString("dd.MM.yyyy"));
            var pBottom = rowSelfPay.Cells[0].AddParagraph($"Anschrift: {report.Customer.Street} · {report.Customer.ZipCode} {report.Customer.ZipCity?.City ?? ""}");
            pBottom.Format.SpaceAfter = "0.3cm";
            return;
        }

        var table = document.Sections[0].AddTable();
        table.AddColumn("10.0cm");
        table.AddColumn("10.0cm");
        var row = table.AddRow();
        var p = row.Cells[0].AddParagraph($"Name des Kunden: {report.Customer.FirstName} {report.Customer.LastName}");
        p.Format.SpaceBefore = "0.3cm";

        if (report.ClearanceType != ClearanceType.SelfPayment)
        {
            row.Cells[0].AddParagraph("Krankenkasse: ").AddText(report.Customer.Insurance?.Name ?? "");
            p = row.Cells[0].AddParagraph($"Versichertennummer: {report.Customer.InsuredPersonNumber}");
            p.Format.SpaceAfter = "0.3cm";
        }

        p = row.Cells[1].AddParagraph($"Pflegegrad: {report.Customer.CareLevel}");
        p.Format.SpaceBefore = "0.3cm";
        row.Cells[1].AddParagraph("Geburtsdatum: ").AddText(report.Customer.BirthDate.ToString("dd.MM.yyyy"));
        p = row.Cells[1].AddParagraph($"Anschrift: {report.Customer.Street} · {report.Customer.ZipCode} {report.Customer.ZipCity?.City ?? ""}");
        p.Format.SpaceAfter = "0.3cm";
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
        row1.Cells[0].AddParagraph().AddFormattedText($"Anbieter: {(string.IsNullOrEmpty(company.Name) ? company.OwnerName : company.Name)}", "Table");
        row1.Cells[1].AddParagraph().AddFormattedText($"Steuernummer: {company.TaxNumber}", "Table");

        var row2 = headerTable.AddRow();
        row2.Cells[0]
            .AddParagraph()
            .AddFormattedText(
                $"Adresse: {(!string.IsNullOrEmpty(company.OwnerName) ? $"{company.OwnerName} · " : "")} {company.Street} · {company.ZipCode} {company.ZipCity?.City}",
                "Table");
        row2.Cells[1].AddParagraph().AddFormattedText($"IK-Nummer: {company.InstitutionCode}", "Table");
    }

    private static void AddServiceType(Document document, DeploymentReport report)
    {
        var clearanceTypeString = report.ClearanceType switch
        {
            ClearanceType.ReliefAmount => "Entlastungsbetrag § 45b SGB XI",
            ClearanceType.PreventiveCare => "Verhinderungspflege § 39 SGB XI",
            ClearanceType.CareBenefit => "Pflegesachleistungen § 36 SGB XI (max. 40%)",
            _ => throw new ArgumentOutOfRangeException(nameof(report))
        };

        document.Sections[0].AddParagraph(clearanceTypeString, "H2");

        //var table = document.Sections[0].AddTable();
        //table.Format.SpaceBefore = "0.3cm";
        //table.Format.SpaceAfter = "0.3cm";
        //table.Borders.Visible = true;
        //table.AddColumn("2.82cm");
        //table.AddColumn("6.19cm");
        //table.AddColumn("6.44cm");
        //table.AddColumn("9.33cm");

        //var row = table.AddRow();
        //row.Cells[0].AddParagraph().AddFormattedText("Leistungsart", TextFormat.Bold);

        //row.Cells[1].AddParagraph($"[{(report.ClearanceType == ClearanceType.ReliefAmount ? "X" : " ")}] Entlastungsbetrag § 45b SGB XI");
        //row.Cells[2].AddParagraph($"[{(report.ClearanceType == ClearanceType.PreventiveCare ? "X" : " ")}] Verhinderungspflege § 39 SGB XI");
        //row.Cells[3].AddParagraph($"[{(report.ClearanceType == ClearanceType.CareBenefit ? "X" : " ")}] ___% Pflegesachleistungen § 36 SGB XI (max. 40%)");
    }

    private static void AddSignatures(Document document, Company company, DeploymentReport report)
    {
        // add empty paragraph to lazily create space
        document.Sections[0].AddParagraph();

        if (report.ClearanceType != ClearanceType.SelfPayment && report.Customer.InsuranceStatus == InsuranceStatus.Statutory)
            document.Sections[0].AddParagraph($"Ich habe meine Leistungen gemäß §45b SGB XI an {company.Name} abgetreten.");

        var info = document.Sections[0].AddParagraph("Hiermit bestätige ich den Einsatz und die Richtigkeit der oben angegebenen Einsatzzeit.");
        info.Format.SpaceAfter = "0.7cm";

        var table = document.Sections[0].AddTable();
        table.KeepTogether = true;
        table.AddColumn("10.0cm");
        table.AddColumn("2.0cm");
        table.AddColumn("10.cm");

        var row = table.AddRow();
        row.KeepWith = 1;
        row.Cells[0].AddParagraph($"{report.SignatureCity}, {report.SignatureDate:dd.MM.yyyy}");
        row.Cells[0].AddImage(report.SignatureEmployee![15..].Replace("base64,", "base64:")).Height = 55;
        row.Cells[2].AddParagraph($"{report.SignatureCity}, {report.SignatureDate:dd.MM.yyyy}");
        row.Cells[2].AddImage(report.SignatureCustomer![15..].Replace("base64,", "base64:")).Height = 55;

        row = table.AddRow();
        row.Cells[0].AddParagraph("Ort, Datum, Unterschrift");
        row.Cells[0].AddParagraph().AddFormattedText("-leistungserbringende Person-", TextFormat.Italic);
        row.Cells[0].Borders.Top.Visible = true;
        row.Cells[2].AddParagraph("Ort, Datum, Unterschrift").Format.Font.Size = "9";
        row.Cells[2].AddParagraph().AddFormattedText("-Kunde/Kundin-", TextFormat.Italic).Size = "9";
        row.Cells[2].Borders.Top.Visible = true;
    }

    private static void AddTimeTable(Document document, DeploymentReport report)
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

        headerRow.Cells[3].AddParagraph("Name der leistungserbringenden Person").Style = "ThLeft";
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

        for (var i = 0; i < 6; i++)
            foreach (var appointment in report.Appointments.OrderBy(o => o.Date).ThenBy(o => o.TimeStart))
            {
                var row = tableDates.AddRow();
                row.VerticalAlignment = VerticalAlignment.Center;
                row.Cells[0].AddParagraph(appointment.Date.ToString("dd.MM.yyyy")).Style = "TdCenter";
                row.Cells[1].AddParagraph($"{appointment.TimeStart:HH:mm} - {appointment.TimeEnd:HH:mm}").Style = "TdCenter";
                row.Cells[2].AddParagraph($"{(appointment.TimeEnd - appointment.TimeStart).TotalHours:0.00}").Style = "TdCenter";

                row.Cells[3].AddParagraph(appointment.EmployeeReplacement?.FullName ?? appointment.Employee.FullName);
                var signatureEmployee = row.Cells[4].AddParagraph().AddImage(appointment.SignatureEmployee![15..].Replace("base64,", "base64:"));
                signatureEmployee.Height = 25;
                signatureEmployee.Width = 100;

                var signatureCustomer = row.Cells[5].AddParagraph().AddImage(appointment.SignatureCustomer![15..].Replace("base64,", "base64:"));
                signatureCustomer.Height = 25;
                signatureCustomer.Width = 100;
            }

        var footerRow = tableDates.AddRow();
        footerRow.VerticalAlignment = VerticalAlignment.Center;
        footerRow.Cells[0].AddParagraph("Gesamtstunden").Style = "ThRight";

        footerRow.Cells[0].MergeRight = 1;
        footerRow.Cells[0].Borders.Left.Visible = false;
        footerRow.Cells[0].Borders.Bottom.Visible = false;
        footerRow.Cells[0].Borders.Bottom.Visible = false;

        footerRow.Cells[2].AddParagraph(report.Appointments.Sum(o => (o.TimeEnd - o.TimeStart).TotalHours).ToString("#.00")).Style = "TdCenter";

        footerRow.Cells[3].MergeRight = 2;
        footerRow.Cells[3].Borders.Bottom.Visible = false;
        footerRow.Cells[5].Borders.Right.Visible = false;
    }

    private static void CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.TopMargin = "2.7cm";
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.Orientation = Orientation.Landscape;
    }

    private static void DefineStyles(Document document)
    {
        // Get the predefined style Normal.
        var style = document.Styles["Normal"];

        style!.Font.Name = "Arial";
        style.ParagraphFormat.SpaceAfter = "0.08cm";

        style = document.Styles.AddStyle("Table", "Normal");
        style.Font.Size = 11;

        style = document.Styles.AddStyle("H1", "Normal");
        style.Font.Size = 15;
        style.Font.Bold = true;

        style = document.Styles.AddStyle("H2", "Normal");
        style.Font.Size = 15;
        style.Font.Bold = false;

        style = document.Styles.AddStyle("ThLeft", "Table");
        style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
        style.Font.Bold = true;

        style = document.Styles.AddStyle("ThCenter", "Table");
        style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        style.Font.Bold = true;

        style = document.Styles.AddStyle("TdCenter", "Table");
        style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

        style = document.Styles.AddStyle("ThRight", "Table");
        style.ParagraphFormat.Alignment = ParagraphAlignment.Right;
        style.Font.Bold = true;
    }
}