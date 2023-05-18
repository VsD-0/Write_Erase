using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using Paragraph = iText.Layout.Element.Paragraph;
using Table = iText.Layout.Element.Table;

namespace Write_Erase.Services
{
    public static class DocumentService
    {
        public static async Task Create(decimal totalCost, decimal discount, Orderpickuppoint point, int orderNumber, int code)
        {
            PdfWriter writer = new($"WriteErase_{DateTime.Now.ToString("MMddyyyyHHmmss")}.pdf");
            PdfDocument pdf = new(writer);
            Document document = new(pdf);


            PdfFont comic = PdfFontFactory.CreateFont(@"C:\Windows\Fonts\comic.ttf", PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_NOT_EMBEDDED);

            var content = new Paragraph("Дата заказа")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(18);
            document.Add(content);

            content = new Paragraph(DateOnly.FromDateTime(DateTime.Now).ToString("d"))
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16);
            document.Add(content);

            content = new Paragraph(" ")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16);
            document.Add(content);

            Table table = new(2, true);

            var tableOrder = new Table(2, false)
                .SetWidth(UnitValue.CreatePercentValue(100))
                .SetHeight(UnitValue.CreatePercentValue(100))
                .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

            tableOrder.AddCell(new Paragraph("Артикул")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            tableOrder.AddCell(new Paragraph("Кол-во")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            foreach (var item in Global.ProductsBasket)
            {
                tableOrder.AddCell(new Paragraph(item.Product.Article)
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

                tableOrder.AddCell(new Paragraph(item.Count.ToString())
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));
            }

            table.AddCell(new Paragraph("Содержимое заказа:")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(tableOrder);

            table.AddCell(new Paragraph("Сумма заказа:")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(new Paragraph(string.Format("{0:C2}", totalCost))
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(new Paragraph("Сумма скидки:")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(new Paragraph(string.Format("{0:C2}", discount))
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(new Paragraph("Пункт выдачи:")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(new Paragraph(string.Format("{0}, г. {1}, ул. {2}, д. {3}",
                point.PickupPoint, point.City, point.Street, point.House))
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(new Paragraph(string.Format("{0}", code))
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            document.Add(table);
            
            table.Complete();

            document.Close();

            await Task.CompletedTask;
        }
    }
}
