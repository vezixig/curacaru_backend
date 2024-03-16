namespace Curacaru.Backend.Infrastructure.Reports;

using Core.Entities;
using MigraDoc.DocumentObjectModel;

internal static class DeclarationOfAssignment
{
    public static Document Create(Company company, AssignmentDeclaration data)
    {
        Document document = new();
        document.Info.Title = "Abtretungserklärung";

        DefineStyles(document);

        CreatePage(document);
        var section = document.Sections[0];
        section.AddParagraph("Abtretungserklärung", "H1");

        AddCustomerData(document, data);

        section.AddParagraph("Hiermit erteile ich mein Einverständnis, dass das folgende Angebot zur Unterstützung im Alltag:");

        AddCompanyData(document, company);

        section.AddParagraph(
            @$"die Leistungen gemäß §45b SGB XI für das Jahr {data.Year} direkt mit meiner Pflegekasse abrechnen darf.
Mit Vorlage einer Rechnung inklusive eines von mir unterschriebenen Einsatznachweises ist das o.g. Angebot zur Unterstützung im Alltag berechtigt, die erbrachten Leistungen abzurechnen.
Ich erkläre mich zudem damit einverstanden, dass das o.g. Angebot bei meiner Pflegekasse Auskünfte über die zur Verfügung stehenden Leistungen gemäß §45b SGB XI einholen darf.
Ich habe eine Kopie dieser Abtretungserklärung erhalten.
");

        AddSignatureArea(document, data);

        section.AddParagraph("", "PClose").AddFormattedText("Hinweis:", TextFormat.Bold);
        section.AddParagraph(
            @"Ihnen steht bei Ihrer Pflegeversicherung nur ein begrenztes Budget zur Verfügung. Der Entlastungsbetrag beträgt 125 Euro pro Monat. Sollte dieses Budget für die vereinbarten und erbrachten Leistungen nicht ausreichen, so verpflichten Sie sich, die Rechnung oder etwaige Differenzbeträge (Zuzahlungen) privat zu begleichen. 
Die Abtretungserklärung entbindet Sie nicht von Ihrer Zahlungspflicht gegenüber dem o.g. Angebot zur Unterstützung im Alltag.
Die Abtretungserklärung ist keine Vollmacht und bezieht sich ausschließlich auf die Abrechnung der Leistungen nach §45b SGB XI.");

        return document;
    }

    private static void AddCompanyData(Document document, Company company)
    {
        var section = document.Sections[0];

        AddLine("Name", company.Name);
        AddLine(
            "Adresse",
            $"{(!string.IsNullOrEmpty(company.OwnerName) ? $"{company.OwnerName} · " : "")}{company.Street} · {company.ZipCode} · {company.ZipCity?.City}");
        AddLine("Angebots-ID", company.ServiceId);
        AddLine("Datum der Anerkennung", company.RecognitionDate.ToString("dd.MM.yyyy"));
        AddLine("IK-Nummer", company.InstitutionCode);
        section.LastParagraph!.Format.SpaceAfter = "0.3cm";
#pragma warning disable S3626
        return;
#pragma warning restore S3626

        void AddLine(string title, string text)
        {
            var paragraph = section.AddParagraph();
            paragraph.Style = "PClose";
            paragraph.AddFormattedText($"{title}: ", TextFormat.Bold);
            paragraph.AddText(text);
        }
    }

    private static void AddCustomerData(Document document, AssignmentDeclaration declaration)
    {
        var section = document.Sections[0];
        var table = section.AddTable();
        var columnWidth = (section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin) / 4;
        table.AddColumn(columnWidth * 2);
        table.AddColumn(columnWidth);
        table.AddColumn(columnWidth);

        var row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Name, Vorname", TextFormat.Bold);
        row.Cells[0].AddParagraph($"{declaration.CustomerLastName}, {declaration.CustomerFirstName}");
        row.Cells[2].AddParagraph().AddFormattedText("Geburtsdatum", TextFormat.Bold);
        row.Cells[2].AddParagraph(declaration.DateOfBirth.ToString("dd.MM.yyyy"));

        row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Straße, Hausnr.", TextFormat.Bold);
        row.Cells[0].AddParagraph(declaration.CustomerStreet);
        row.Cells[1].AddParagraph().AddFormattedText("PLZ", TextFormat.Bold);
        row.Cells[1].AddParagraph(declaration.CustomerZipCode ?? "");
        row.Cells[2].AddParagraph().AddFormattedText("Ort", TextFormat.Bold);
        row.Cells[2].AddParagraph(declaration.CustomerZipCity?.City ?? "");

        row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Versichertennummer", TextFormat.Bold);
        row.Cells[0].AddParagraph(declaration.InsuredPersonNumber);
        row.Cells[1].MergeRight = 1;
        row.Cells[1]
            .AddParagraph()
            .AddFormattedText("Hinweis: ", TextFormat.Bold)
            .AddFormattedText("Sie finden Ihre persönliche Versichertennummer auf Ihrer Krankenkassen-Karte. ", TextFormat.NotBold)
            .AddText("Die Versichertennummer beginnt mit einem Buchstaben und hat danach neun Zahlen.");

        row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Name der Krankenkasse", TextFormat.Bold);
        row.Cells[0].AddParagraph(declaration.InsuranceName ?? "");
        row.Cells[1].MergeRight = 1;
        row.Cells[1]
            .AddParagraph()
            .AddFormattedText("Hinweis: ", TextFormat.Bold)
            .AddFormattedText("Hier tragen Sie bitte Namen und Postanschrift Ihrer Krankenkasse ein.", TextFormat.NotBold);

        row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Straße, Hausnr.", TextFormat.Bold);
        row.Cells[0].AddParagraph(declaration.InsuranceStreet ?? "");
        row.Cells[1].AddParagraph().AddFormattedText("PLZ", TextFormat.Bold);
        row.Cells[1].AddParagraph(declaration.InsuranceZipCode ?? "");
        row.Cells[2].AddParagraph().AddFormattedText("Ort", TextFormat.Bold);
        row.Cells[2].AddParagraph(declaration.InsuranceZipCity?.City ?? "");
    }

    private static void AddSignatureArea(Document document, AssignmentDeclaration data)
    {
        var table = document.Sections[0].AddTable();
        table.AddColumn("3.75cm");
        table.AddColumn("0.5cm");
        table.AddColumn("3.75cm");
        table.AddColumn("0.5cm");
        table.AddColumn("8.0cm");
        var row = table.AddRow();
        var cell = row.Cells[0].AddParagraph();
        cell.Format.SpaceAfter = 0;
        cell.AddFormattedText("Ort", TextFormat.Bold).AddLineBreak();
        cell.AddFormattedText(data.SignatureCity, TextFormat.NotBold);

        cell = row.Cells[2].AddParagraph();
        cell.Format.SpaceAfter = 0;
        cell.AddFormattedText("Datum", TextFormat.Bold).AddLineBreak();
        cell.AddFormattedText(data.SignatureDate.ToString("dd.MM.yyyy"), TextFormat.NotBold);

        cell = row.Cells[4].AddParagraph();
        cell.Format.SpaceAfter = 0;

        cell.AddFormattedText("Unterschrift", TextFormat.Bold).AddLineBreak();
        cell.AddImage(data.Signature![15..].Replace("base64,", "base64:")).Height = 40;
    }

    private static void CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.Orientation = Orientation.Portrait;
    }

    private static void DefineStyles(Document document)
    {
        // Get the predefined style Normal.
        var style = document.Styles["Normal"];

        style!.Font.Name = "Arial";
        style.ParagraphFormat.SpaceAfter = "0.5cm";

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
        style.ParagraphFormat.SpaceAfter = "0.1cm";
    }
}