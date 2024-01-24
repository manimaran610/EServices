using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Application.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Common;
using OpenXmlPowerTools;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using Word = Microsoft.Office.Interop.Word;

namespace Infrastructure.Shared.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        public Task<string> ConvertDocToHtml(string path, byte[] byteArray)
        {
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(memoryStream, true))
                {
                    // Iterate through header parts and copy content to the main document
                    foreach (var headerPart in doc.MainDocumentPart.HeaderParts)
                    {
                        CopyContentFromHeaderToBody(
                            doc.MainDocumentPart.Document,
                            headerPart.Header
                        );
                    }

                    var settings = new HtmlConverterSettings() { PageTitle = "My Page Title" };

                    // Convert main document to HTML
                    var mainHtml = OpenXmlPowerTools.HtmlConverter.ConvertToHtml(doc, settings);

                    // Write the HTML to the specified path
                    File.WriteAllText(path, mainHtml.ToString());
                }

                return Task.FromResult("Success");
            }
        }

        private void CopyContentFromHeaderToBody(Document mainDocument, Header header)
        {
            // Iterate through paragraphs in the header and copy to the body
            foreach (var headerParagraph in header.Descendants<Paragraph>())
            {
                // Clone the paragraph to avoid modifying the original
                var clonedParagraph = new Paragraph(headerParagraph.OuterXml);

                // Append the cloned paragraph to the main document body
                mainDocument.Body.AppendChild(clonedParagraph.CloneNode(true));
            }
        }

        public async Task MailMergeWorkDocument(
            string templatePath,
            string DestinatePath,
            List<keyValue> keyValuePairs,
            List<TemplateRowConfig> templateRows = null,
            int documentTableIndex = 0
        )
        {
            try
            {
                await ConvertDotxToDocs(templatePath, DestinatePath);
            }
            catch
            {
                File.Copy(templatePath, DestinatePath, true);
            }

            using (WordprocessingDocument doc = WordprocessingDocument.Open(DestinatePath, true))
            {
                if (templateRows != null)
                    CreateDynamicTemplateRows(doc, templateRows, documentTableIndex);

                var allTextParams =
                    doc.MainDocumentPart
                        ?.Document
                        .Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
                    ?? new List<Text>();
                var allHeaderParams =
                    doc.MainDocumentPart
                        ?.HeaderParts
                        .SelectMany(
                            hp =>
                                hp.Header.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
                        ) ?? new List<Text>();

                foreach (var keyValue in keyValuePairs)
                {
                    List<string> addedList = new List<string>();

                    //Mapping header contents
                      foreach (
                        Text textItem in allHeaderParams
                            .Where(e => e.Text != null && e.Text.Contains($"<{keyValue.Key}>"))
                            .Take(2)
                    )
                    {
                        if (textItem.Text != null)
                        {
                            if (
                                textItem.Text.Contains($"<{keyValue.Key}>")
                                && !addedList.Any(e => e == keyValue.Key)
                            )
                            {
                                textItem.Text = textItem
                                    .Text
                                    .Replace($"<{keyValue.Key}>", keyValue.Value);
                                addedList.Add(keyValue.Key);
                            }
                        }
                    }
                    //Mapping Body contents
                    foreach (
                        Text textItem in allTextParams
                            .Where(e => e.Text != null && e.Text.Contains($"<{keyValue.Key}>"))
                            .Take(2)
                    )
                    {
                        if (textItem.Text != null)
                        {
                            if (
                                textItem.Text.Contains($"<{keyValue.Key}>")
                                && !addedList.Any(
                                    e => e == keyValue.Key && !keyValue.Key.Contains("ImgQR")
                                )
                            )
                            {
                                textItem.Text = textItem
                                    .Text
                                    .Replace($"<{keyValue.Key}>", keyValue.Value);
                                addedList.Add(keyValue.Key);
                            }
                        }
                    }

                  

                    //Map QR Image into word document
                    if (keyValue.Key.Contains("ImgQR"))
                    {
                        // byte[] imageBytes = await GenerateQRImage(
                        //     $"http://chart.apis.google.com/chart?cht=qr&chs=600x500&chl={keyValue.Value}"
                        // );

                        byte[] imageBytes = await GenerateQRImage(
                            $"https://api.qrserver.com/v1/create-qr-code/?size=150x150&data={keyValue.Value}"
                        );

                        MapImageIntoDocument(doc.MainDocumentPart, imageBytes, "<QRCode>");
                        addedList.Add(keyValue.Key);
                    }
                }

                // Remove unfilled rows from the document
                CleanUpUnMergedTemplateFields(allTextParams);

                // Save the document
                doc.MainDocumentPart.Document.Save();
            }

            // Merge the data into the Word template
            async Task ConvertDotxToDocs(string sourceFile, string targetFile)
            {
                MemoryStream documentStream;
                using (Stream tplStream = File.OpenRead(sourceFile))
                {
                    documentStream = new MemoryStream((int)tplStream.Length);
                    await CopyStream(tplStream, documentStream);
                    documentStream.Position = 0L;
                }

                if (sourceFile.Split('.')[1] == "dotx")
                {
                    using (
                        WordprocessingDocument template = WordprocessingDocument.Open(
                            documentStream,
                            true
                        )
                    )
                    {
                        template.ChangeDocumentType(
                            DocumentFormat.OpenXml.WordprocessingDocumentType.Document
                        );
                        MainDocumentPart mainPart = template.MainDocumentPart!;
                        mainPart
                            .DocumentSettingsPart
                            ?.AddExternalRelationship(
                                "http://schemas.openxmlformats.org/officeDocument/2006/relationships/attachedTemplate",
                                new Uri(targetFile, UriKind.Absolute)
                            );

                        mainPart.Document.Save();
                    }
                }
                await File.WriteAllBytesAsync(targetFile, documentStream.ToArray());
            }

            async Task CopyStream(Stream source, Stream target)
            {
                if (source != null)
                {
                    MemoryStream mstream = source as MemoryStream ?? default!;
                    if (mstream != null)
                        mstream.WriteTo(target);
                    else
                    {
                        byte[] buffer = new byte[2048];
                        int length = buffer.Length,
                            size;
                        while ((size = source.Read(buffer, 0, length)) != 0)
                            await target.WriteAsync(buffer, 0, size);
                    }
                }
            }

            void CleanUpUnMergedTemplateFields(IEnumerable<Text> texts)
            {
                while (texts!.Any(e => e.Text.Contains("<") && e.Text.Contains(">")))
                {
                    foreach (
                        Text textItem in texts!.Where(
                            e => e.Text.Contains("<") && e.Text.Contains(">")
                        )
                    )
                    {
                        foreach (var paragraph in textItem.Ancestors<Paragraph>())
                        {
                            if (paragraph != null)
                            {
                                foreach (
                                    var row in paragraph.Ancestors<TableRow>().ToList<TableRow>()
                                )
                                {
                                    var parent = paragraph.Ancestors<Table>().FirstOrDefault();
                                    if (parent != null)
                                    {
                                        parent.RemoveChild(row);
                                    }
                                    //paragraph.Remove();
                                }
                            }
                        }
                    }
                }
            }

            void CreateDynamicTemplateRows(
                WordprocessingDocument document,
                List<TemplateRowConfig> templateRows,
                int tableIndex = 0
            )
            {
                var lastChild = templateRows.LastOrDefault();
                foreach (var templateRow in templateRows)
                {
                    // Assuming the table is in the first (0-index) body of the document
                    var tables = document.MainDocumentPart.Document.Body.Elements<Table>();
                    Table table = document
                        .MainDocumentPart
                        .Document
                        .Body
                        .Elements<Table>()
                        .Skip(tableIndex)
                        .First();
                    // Clone the row at the specified index
                    TableRow clonedRow = (TableRow)
                        table.Elements<TableRow>().ElementAt(templateRow.RowIndex).CloneNode(true);

                    // Insert the cloned row into the table
                    table.InsertAfter(
                        clonedRow,
                        table.Elements<TableRow>().ElementAt(templateRow.RowInsertAfter)
                    );
                    if (templateRow.OrderNo == lastChild?.OrderNo)
                    {
                        if (templateRow.NestedRowCount < 2)
                        {
                            table.RemoveChild(
                                table.Elements<TableRow>().ElementAt(templateRow.RowInsertAfter)
                            );
                        }
                        else
                            templateRow.NestedRowCount--;
                    }

                    for (int i = 1; i < templateRow.NestedRowCount; i++)
                    {
                        // Clone the row at the specified index
                        TableRow clonedNestedRow = (TableRow)
                            table
                                .Elements<TableRow>()
                                .ElementAt(templateRow.NestedRowIndex)
                                .CloneNode(true);
                        table.InsertAfter(
                            clonedNestedRow,
                            table.Elements<TableRow>().ElementAt(templateRow.NestedRowInsertAfter)
                        );
                    }
                }
            }
        }

        private async Task<byte[]> GenerateQRImage(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                    return await Task.FromResult(client.DownloadData(url));
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error occurred while downloading QR Code");
                return new byte[0];
            }
        }

        private ImagePart FeedImagePart(MainDocumentPart mainPart, byte[] imageBytes)
        {
            // Create a new ImagePart for the QR code image
            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);
            using (MemoryStream stream = new MemoryStream(imageBytes))
            {
                imagePart.FeedData(stream);
            }

            return imagePart;
        }

        private void MapImageIntoDocument(
            MainDocumentPart mainPart,
            byte[] imageBytes,
            string placeholder
        )
        {
            ImagePart imagePart = FeedImagePart(mainPart, imageBytes);
            var textElementsList =
                mainPart
                    ?.Document
                    .Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
                    .ToList()
                    .Where(e => e.Text.Contains(placeholder)) ?? new List<Text>();
            foreach (var textElement in textElementsList)
            {
                if (textElement != null)
                {
                    string text = textElement.Text;

                    if (text.Contains(placeholder))
                    {
                        textElement.Text = "";
                        text = ""; // Clear the existing text

                        // Create an inline image
                        var inline = new Drawing(
                            new DW.Inline(
                                new DW.Extent() { Cx = 400000, Cy = 400000 }, // Adjust the size as needed
                                new DW.EffectExtent()
                                {
                                    LeftEdge = 0L,
                                    TopEdge = 0L,
                                    RightEdge = 0L,
                                    BottomEdge = 0L
                                },
                                new DW.DocProperties()
                                {
                                    Id = (uint)new Random().Next(0, 4),
                                    Name = $"QRCode{new Random().Next(0, 1000)}",
                                    Description = "QR Code"
                                },
                                new DW.NonVisualGraphicFrameDrawingProperties(
                                    new A.GraphicFrameLocks() { NoChangeAspect = true }
                                ),
                                new A.Graphic(
                                    new A.GraphicData(
                                        new PIC.Picture(
                                            new PIC.NonVisualPictureProperties(
                                                new PIC.NonVisualDrawingProperties()
                                                {
                                                    Id = (uint)new Random().Next(0, 4),
                                                    Name = $"QRCode{new Random().Next(0, 1000)}",
                                                },
                                                new PIC.NonVisualPictureDrawingProperties()
                                            ),
                                            new PIC.BlipFill(
                                                new A.Blip(
                                                    new A.BlipExtensionList(
                                                        new A.BlipExtension()
                                                        {
                                                            Uri =
                                                                "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                        }
                                                    )
                                                )
                                                {
                                                    Embed = mainPart.GetIdOfPart(imagePart),
                                                    CompressionState = A.BlipCompressionValues.None
                                                },
                                                new A.Stretch(new A.FillRectangle())
                                            ),
                                            new PIC.ShapeProperties(
                                                new A.Transform2D(
                                                    new A.Offset() { X = 0, Y = 0 },
                                                    new A.Extents() { Cx = 400000, Cy = 400000 }
                                                ), // Adjust the size as needed
                                                new A.PresetGeometry(new A.AdjustValueList())
                                                {
                                                    Preset = A.ShapeTypeValues.Rectangle
                                                }
                                            )
                                        )
                                    )
                                    {
                                        Uri =
                                            "http://schemas.openxmlformats.org/drawingml/2006/picture"
                                    }
                                )
                            )
                        );

                        // Append the inline image to the paragraph
                        textElement?.Parent?.InsertAfter(inline, textElement);
                    }
                }
            }
        }
    }
}
