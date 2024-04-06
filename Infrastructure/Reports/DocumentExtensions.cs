namespace Curacaru.Backend.Infrastructure.Reports;

using MigraDoc.DocumentObjectModel;

public static class DocumentExtensions
{
    public static Unit GetPageWidth(this Document document)
        => document.DefaultPageSetup.PageWidth - document.DefaultPageSetup.LeftMargin - document.DefaultPageSetup.RightMargin;
}