using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Paragraph = iText.Layout.Element.Paragraph;
using Table = iText.Layout.Element.Table;

namespace Write_Erase.Services
{
    public static class DocumentService
    {
        public static async Task Create(decimal totalCost, Orderpickuppoint point, Order order, ObservableCollection<Basket> ProductsBasket)
        {
            string fileName = $"Чек за {DateTime.Now.ToString("MMddyyHHmmss")}.pdf";

            // Создаем файловый поток для записи документа в файл
            FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);

            // Создаем объект PdfWriter
            PdfWriter writer = new PdfWriter(fileStream);

            // Создаем объект PdfDocument
            PdfDocument pdf = new PdfDocument(writer);

            // Создаем объект Document
            Document document = new Document(pdf);

            // Создаем объекты шрифтов
            PdfFont headlineFont = PdfFontFactory.CreateFont(@"C:\Windows\Fonts\comic.ttf", PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_NOT_EMBEDDED);
            PdfFont cellFont = PdfFontFactory.CreateFont(@"C:\Windows\Fonts\arial.ttf", PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_NOT_EMBEDDED);

            // Добавляем заголовок документа
            Paragraph headline = new Paragraph("Заказ на Write-Erase");
            headline.SetFont(headlineFont).SetFontSize(24);

            document.Add(headline);

            // Создаем таблицу для вывода данных заказа
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 30, 70 }));
            table.SetWidth(UnitValue.CreatePointValue(500));

            // Добавляем ячейку со значением "Общая стоимость"
            Cell totalCostCellTitle = new Cell().Add(new Paragraph("Общая стоимость:"));
            totalCostCellTitle.SetFont(cellFont).SetFontSize(16);
            table.AddCell(totalCostCellTitle);

            // Добавляем ячейку с общей стоимостью заказа
            Cell totalCostCell = new Cell().Add(new Paragraph($"{totalCost:C2}"));
            totalCostCell.SetFont(cellFont).SetFontSize(16);
            table.AddCell(totalCostCell);

            // Создаем ячейку с заголовком "Пункт выдачи"
            Cell pickupPointTitleCell = new Cell().Add(new Paragraph("Пункт выдачи:"));
            pickupPointTitleCell.SetFont(cellFont).SetFontSize(16);
            table.AddCell(pickupPointTitleCell);

            // Создаем строку с адресом пункта выдачи заказа
            string pickupAddress = $"{point.PickupPoint} {point.City}, {point.Street}, {point.House}";

            // Добавляем ячейку с адресом пункта выдачи заказа
            Cell pickupPointCell = new Cell().Add(new Paragraph(pickupAddress));
            pickupPointCell.SetFont(cellFont).SetFontSize(16);
            table.AddCell(pickupPointCell);

            // Добавляем ячейку со значением "Дата доставки"
            Cell deliveryDateTitleCell = new Cell().Add(new Paragraph("Дата доставки:"));
            deliveryDateTitleCell.SetFont(cellFont).SetFontSize(16);
            table.AddCell(deliveryDateTitleCell);

            // Добавляем ячейку с датой доставки заказа
            Cell deliveryDateCell = new Cell().Add(new Paragraph(order.OrderDeliveryDate.ToShortDateString()));
            deliveryDateCell.SetFont(cellFont).SetFontSize(16);
            table.AddCell(deliveryDateCell);

            // Добавляем ячейку со значением "Дата оформления"
            Cell orderDateTitleCell = new Cell().Add(new Paragraph("Дата оформления:")); 
            orderDateTitleCell.SetFont(cellFont).SetFontSize(16); 
            table.AddCell(orderDateTitleCell);

            // Добавляем ячейку с датой оформления заказа
            Cell orderDateCell = new Cell().Add(new Paragraph(order.DateOfOrder.ToShortDateString()));
            orderDateCell.SetFont(cellFont).SetFontSize(16);
            table.AddCell(orderDateCell);

            // Добавляем ячейку со значением "Код заказа"
            Cell receiptCodeTitleCell = new Cell().Add(new Paragraph("Код заказа:"));
            receiptCodeTitleCell.SetFont(cellFont).SetFontSize(16);
            table.AddCell(receiptCodeTitleCell);

            // Добавляем ячейку с кодом заказа
            Cell receiptCodeCell = new Cell().Add(new Paragraph(order.ReceiptCode.ToString()));
            receiptCodeCell.SetFont(cellFont).SetFontSize(16);
            table.AddCell(receiptCodeCell);

            // Добавляем ячейку со значением "Полное имя пользователя"
            Cell fullNameTitleCell = new Cell().Add(new Paragraph("Полное имя пользователя:"));
            fullNameTitleCell.SetFont(cellFont).SetFontSize(16);
            table.AddCell(fullNameTitleCell);

            // Добавляем ячейку с полным именем пользователя
            Cell fullNameCell = new Cell().Add(new Paragraph(order.FullNameUser));
            fullNameCell.SetFont(cellFont).SetFontSize(16);
            table.AddCell(fullNameCell);

            // Добавляем таблицу на страницу документа
            document.Add(table);

            // Создаем таблицу для вывода корзины
            Table basketTable = new Table(UnitValue.CreatePercentArray(new float[] { 30, 20, 20, 10, 20 }));
            basketTable.SetWidth(UnitValue.CreatePointValue(500));

            // Добавляем заголовок таблицы
            Cell productTitleCell = new Cell().Add(new Paragraph("Товар"));
            productTitleCell.SetFont(cellFont).SetFontSize(12);
            basketTable.AddCell(productTitleCell);

            Cell priceTitleCell = new Cell().Add(new Paragraph("Цена"));
            priceTitleCell.SetFont(cellFont).SetFontSize(12);
            basketTable.AddCell(priceTitleCell);

            Cell discountTitleCell = new Cell().Add(new Paragraph("Скидка"));
            discountTitleCell.SetFont(cellFont).SetFontSize(12);
            basketTable.AddCell(discountTitleCell);

            Cell countTitleCell = new Cell().Add(new Paragraph("Количество"));
            countTitleCell.SetFont(cellFont).SetFontSize(12);
            basketTable.AddCell(countTitleCell);

            Cell sumTitleCell = new Cell().Add(new Paragraph("Сумма"));
            sumTitleCell.SetFont(cellFont).SetFontSize(12);
            basketTable.AddCell(sumTitleCell);

            // Добавляем строки с продуктами в корзине
            foreach (var product in ProductsBasket)
            {
                Cell productCell = new Cell().Add(new Paragraph(product.Product.Title));
                productCell.SetFont(cellFont).SetFontSize(12);
                basketTable.AddCell(productCell);

                Cell priceCell = new Cell().Add(new Paragraph($"{product.Product.Price:C2} за {product.Product.Unit}"));
                priceCell.SetFont(cellFont).SetFontSize(12);
                basketTable.AddCell(priceCell);

                Cell discountCell = new Cell().Add(new Paragraph($"{product.Product.Discount} %"));
                discountCell.SetFont(cellFont).SetFontSize(12);
                basketTable.AddCell(discountCell);

                Cell countCell = new Cell().Add(new Paragraph(product.Count.ToString()));
                countCell.SetFont(cellFont).SetFontSize(12);
                basketTable.AddCell(countCell);

                Cell sumCell = new Cell().Add(new Paragraph($"{(product.Product.DisplayedPrice * product.Count):C2}"));
                sumCell.SetFont(cellFont).SetFontSize(12);
                basketTable.AddCell(sumCell);
            }

            // Добавляем таблицу с корзиной в документ
            document.Add(basketTable);

            // Закрываем объект Document
            document.Close();

            await Task.CompletedTask;
        }
    }
}
