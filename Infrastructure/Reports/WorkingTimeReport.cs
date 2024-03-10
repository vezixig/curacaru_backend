namespace Curacaru.Backend.Infrastructure.Reports;

using Core.Entities;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

internal static class WorkingTimeReport
{
    public static Document Create(Core.Entities.WorkingTimeReport report, List<Appointment> appointments)
    {
        Document document = new();
        document.Info.Title = "Arbeitszeiterfassung";

        DefineStyles(document);

        CreatePage(document);
        var section = document.Sections[0];
        section.AddParagraph("Arbeitszeiterfassung", "H1");

        var monthName = new DateTime(DateTime.Now.Year, report.Month, 1).ToString("MMMM");

        section.AddParagraph($"Arbeitnehmer: {report.Employee.FullName}", "H2");
        section.AddParagraph($"Zeitraum: {monthName} {report.Year}", "H2");

        var tableDates = document.Sections[0].AddTable();
        tableDates.Borders.Visible = true;
        tableDates.Rows.Height = "0.3cm";
        tableDates.AddColumn("3.5cm");
        tableDates.AddColumn("3.5cm");
        tableDates.AddColumn("3.5cm");
        tableDates.AddColumn("6.0cm");

        var headerRow = tableDates.AddRow();
        headerRow.Cells[0].AddParagraph("Datum").Style = "ThCenter";
        headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;

        headerRow.Cells[1].AddParagraph("Begin Std./Min.").Style = "ThCenter";
        headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Center;

        headerRow.Cells[2].AddParagraph("Ende Std./Min.").Style = "ThCenter";
        headerRow.Cells[2].VerticalAlignment = VerticalAlignment.Center;

        headerRow.Cells[3].AddParagraph("Arbeitszeit gesamt Std./Min.").Style = "ThCenter";
        headerRow.Cells[3].VerticalAlignment = VerticalAlignment.Center;

        var totalHours = new TimeSpan(0);
        foreach (var appointment in appointments)
        {
            var appointmentRow = tableDates.AddRow();
            appointmentRow.Cells[0].AddParagraph(appointment.Date.ToString("dd.MM.yyyy"));
            appointmentRow.Cells[1].AddParagraph(appointment.TimeStart.ToString("HH:mm"));
            appointmentRow.Cells[2].AddParagraph(appointment.TimeEnd.ToString("HH:mm"));
            appointmentRow.Cells[3].AddParagraph((appointment.TimeEnd - appointment.TimeStart).ToString("HH:mm"));
            totalHours += appointment.TimeEnd - appointment.TimeStart;
        }

        var totalsRow = tableDates.AddRow();
        totalsRow.Cells[0].MergeRight = 2;
        totalsRow.Cells[0].Borders.Width = 0;
        totalsRow.Cells[0].AddParagraph("Summe:").Style = "ThRight";
        totalsRow.Cells[1].AddParagraph("12:34");

        return document;
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
        style.ParagraphFormat.SpaceBefore = "0.1cm";
        style.ParagraphFormat.SpaceAfter = "0.1cm";

        style = document.Styles.AddStyle("H1", "Normal");
        style.Font.Size = 15;
        style.Font.Bold = true;
        style.ParagraphFormat.SpaceBefore = "0.5cm";
        style.ParagraphFormat.SpaceAfter = "0.5cm";

        style = document.Styles.AddStyle("H2", "Normal");
        style.Font.Size = 13;
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