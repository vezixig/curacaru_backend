namespace Curacaru.Backend.Infrastructure.Reports;

using Core.Entities;
using MigraDoc.DocumentObjectModel;

internal static class DeclarationOfAssignment
{
    public static Document Create(Company company, Customer customer, int year)
    {
        Document document = new();
        document.Info.Title = "Abtretungserklärung";

        DefineStyles(document);

        CreatePage(document);
        var section = document.Sections[0];
        section.AddParagraph("Abtretungserklärung", "H1");

        AddCustomerData(document, customer);

        section.AddParagraph("Hiermit erteile ich mein Einverständnis, dass das folgende Angebot zur Unterstützung im Alltag:");

        AddCompanyData(document, company);

        section.AddParagraph(
            @$"die Leistungen gemäß §45b SGB XI für das Jahr {year} direkt mit meiner Pflegekasse abrechnen darf.
Mit Vorlage einer Rechnung inklusive eines von mir unterschriebenen Einsatznachweises ist das o.g. Angebot zur Unterstützung im Alltag berechtigt, die erbrachten Leistungen abzurechnen.
Ich erkläre mich zudem damit einverstanden, dass das o.g. Angebot bei meiner Pflegekasse Auskünfte über die zur Verfügung stehenden Leistungen gemäß §45b SGB XI einholen darf.
Ich habe eine Kopie dieser Abtretungserklärung erhalten.
");

        AddSignatureArea(document);

        section.AddParagraph("", "PClose").AddFormattedText("Hinweis:", TextFormat.Bold);
        section.AddParagraph(
            @"Ihnen steht bei Ihrer Pflegeversicherung nur ein begrenztes Budget zur Verfügung. Der Entlastungsbetrag beträgt 125 Euro pro Monat. Sollte dieses Budget für die vereinbarten und erbrachten Leistungen nicht ausreichen, so verpflichten Sie sich, die Rechnung oder etwaige Differenzbeträge (Zuzahlungen) privat zu begleichen. \r\nDie Abtretungserklärung entbindet Sie nicht von Ihrer Zahlungspflicht gegenüber dem o.g. Angebot zur Unterstützung im Alltag.
                             Die Abtretungserklärung ist keine Vollmacht und bezieht sich ausschließlich auf die Abrechnung der Leistungen nach §45b SGB XI.");

        return document;
    }

    private static void AddCompanyData(Document document, Company company)
    {
        var section = document.Sections[0];

        AddLine("Name", company.Name);
        AddLine("Adresse", $"{company.OwnerName} {company.Street} {company.ZipCode} {company.ZipCity?.City}");
        AddLine("Angebots-ID", company.ServiceId);
        AddLine("Datum der Anerkennung", company.RecognitionDate.ToString("dd.MM.yyyy"));
        AddLine("IK-Nummer", company.InstitutionCode);
        section.AddParagraph("", "PClose");
        return;

        void AddLine(string title, string text)
        {
            var paragraph = section.AddParagraph();
            paragraph.Style = "PClose";
            paragraph.AddFormattedText($"{title}: ", TextFormat.Bold);
            paragraph.AddText(text);
        }
    }

    private static void AddCustomerData(Document document, Customer customer)
    {
        var section = document.Sections[0];
        var table = section.AddTable();
        var columnWidth = (section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin) / 4;
        table.AddColumn(columnWidth * 2);
        table.AddColumn(columnWidth);
        table.AddColumn(columnWidth);

        var row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Name, Vorname", TextFormat.Bold);
        row.Cells[0].AddParagraph($"{customer.LastName}, {customer.FirstName}");
        row.Cells[2].AddParagraph().AddFormattedText("Geburtsdatum", TextFormat.Bold);
        row.Cells[2].AddParagraph(customer.BirthDate.ToString("dd.MM.yyyy"));

        row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Straße, Hausnr.", TextFormat.Bold);
        row.Cells[0].AddParagraph(customer.Street);
        row.Cells[1].AddParagraph().AddFormattedText("PLZ", TextFormat.Bold);
        row.Cells[1].AddParagraph(customer.ZipCode ?? "");
        row.Cells[2].AddParagraph().AddFormattedText("Ort", TextFormat.Bold);
        row.Cells[2].AddParagraph(customer.ZipCity?.City ?? "");

        row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Versichertennummer", TextFormat.Bold);
        row.Cells[0].AddParagraph(customer.InsuredPersonNumber);
        row.Cells[1].MergeRight = 1;
        row.Cells[1]
            .AddParagraph()
            .AddFormattedText("Hinweis: ", TextFormat.Bold)
            .AddFormattedText("Sie finden Ihre persönliche Versichertennummer auf Ihrer Krankenkassen-Karte. ", TextFormat.NotBold)
            .AddText("Die Versichertennummer beginnt mit einem Buchstaben und hat danach neun Zahlen.");

        row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Name der Krankenkasse", TextFormat.Bold);
        row.Cells[0].AddParagraph(customer.Insurance.Name);
        row.Cells[1].MergeRight = 1;
        row.Cells[1]
            .AddParagraph()
            .AddFormattedText("Hinweis: ", TextFormat.Bold)
            .AddFormattedText("Hier tragen Sie bitte Namen und Postanschrift Ihrer Krankenkasse ein.", TextFormat.NotBold);

        row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Straße, Hausnr.", TextFormat.Bold);
        row.Cells[0].AddParagraph(customer.Insurance.Street);
        row.Cells[1].AddParagraph().AddFormattedText("PLZ", TextFormat.Bold);
        row.Cells[1].AddParagraph(customer.Insurance.ZipCode ?? "");
        row.Cells[2].AddParagraph().AddFormattedText("Ort", TextFormat.Bold);
        row.Cells[2].AddParagraph(customer.Insurance.ZipCity?.City ?? "");
    }

    private static void AddSignatureArea(Document document)
    {
        var table = document.Sections[0].AddTable();
        table.AddColumn("3.75cm");
        table.AddColumn("0.5cm");
        table.AddColumn("3.75cm");
        table.AddColumn("0.5cm");
        table.AddColumn("8.0cm");
        table.Rows.Height = "1.2cm";
        var row = table.AddRow();
        row.Cells[0].AddParagraph().AddFormattedText("Ort", TextFormat.Bold);
        row.Cells[0].Borders.Bottom.Visible = true;
        row.Cells[2].AddParagraph().AddFormattedText("Datum", TextFormat.Bold);
        row.Cells[2].Borders.Bottom.Visible = true;
        row.Cells[4].AddParagraph().AddFormattedText("Unterschrift", TextFormat.Bold);
        row.Cells[4].Borders.Bottom.Visible = true;
        document.Sections[0].AddParagraph("", "PClose");
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

        style.Font.Name = "Arial";
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