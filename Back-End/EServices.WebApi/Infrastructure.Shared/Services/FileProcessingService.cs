using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Common;

namespace Infrastructure.Shared.Services
{
    public class FileProcessingService : IFileProcessingService
    {

        public async Task MailMergeWorkDocument(string templatePath, string DestinatePath, List<keyValue> keyValuePairs, List<TemplateRowConfig>? templateRows = null)
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
                if (templateRows != null) CreateDynamicTemplateRows(doc, templateRows);

                var allTextParams = doc.MainDocumentPart?.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>();
                var allHeaderParams = doc.MainDocumentPart?.HeaderParts.SelectMany(hp => hp.RootElement.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>());


                foreach (var keyValue in keyValuePairs)
                {
                    List<string> addedList = new List<string>();
                    foreach (Text textItem in allTextParams ?? new List<Text>())
                    {
                        if (textItem.Text != null)
                        {
                            if (textItem.Text.Contains($"<{keyValue.Key}>") && !addedList.Any(e => e == keyValue.Key))
                            {
                                textItem.Text = textItem.Text.Replace($"<{keyValue.Key}>", keyValue.Value);
                                addedList.Add(keyValue.Key);
                            }
                        }
                    }

                    // allTextParams.FirstOrDefault(e => !string.IsNullOrEmpty(e.Text) && e.Text.Contains($"<{keyValue.Key}>"))?.Text.Replace($"<{keyValue.Key}>", keyValue.Value);


                    foreach (Text textItem in allHeaderParams ?? new List<Text>())
                    {
                        if (textItem.Text != null)
                        {
                            if (textItem.Text.Contains($"<{keyValue.Key}>") && !addedList.Any(e => e == keyValue.Key))
                            {
                                textItem.Text = textItem.Text.Replace($"<{keyValue.Key}>", keyValue.Value);
                                addedList.Add(keyValue.Key);
                            }
                        }
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
                    using (WordprocessingDocument template = WordprocessingDocument.Open(documentStream, true))
                    {
                        template.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
                        MainDocumentPart mainPart = template.MainDocumentPart!;
                        mainPart.DocumentSettingsPart?.AddExternalRelationship("http://schemas.openxmlformats.org/officeDocument/2006/relationships/attachedTemplate",
                           new Uri(targetFile, UriKind.Absolute));

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
                    if (mstream != null) mstream.WriteTo(target);
                    else
                    {
                        byte[] buffer = new byte[2048];
                        int length = buffer.Length, size;
                        while ((size = source.Read(buffer, 0, length)) != 0)
                            await target.WriteAsync(buffer, 0, size);
                    }
                }
            }

            void CleanUpUnMergedTemplateFields(IEnumerable<Text> texts)
            {

                while (texts!.Any(e => e.Text.Contains("<") && e.Text.Contains(">")))
                {
                    foreach (Text textItem in texts!.Where(e => e.Text.Contains("<") && e.Text.Contains(">")))
                    {
                        foreach (var paragraph in textItem.Ancestors<Paragraph>())
                        {
                            if (paragraph != null)
                            {
                                foreach (var row in paragraph.Ancestors<TableRow>().ToList<TableRow>())
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

            void CreateDynamicTemplateRows(WordprocessingDocument document, List<TemplateRowConfig> templateRows)
            {
                var lastChild = templateRows.LastOrDefault();
                foreach (var templateRow in templateRows)
                {
                    Console.WriteLine("Foreach - " + templateRow.OrderNo);


                    // Assuming the table is in the first (0-index) body of the document
                    Table table = document.MainDocumentPart.Document.Body.Elements<Table>().First();
                    // Clone the row at the specified index
                    TableRow clonedRow = (TableRow)table.Elements<TableRow>().ElementAt(templateRow.RowIndex).CloneNode(true);

                    // Insert the cloned row into the table 
                    table.InsertAfter(clonedRow, table.Elements<TableRow>().ElementAt(templateRow.RowInsertAfter));
                    if (templateRow.OrderNo == lastChild?.OrderNo)
                    {
                        Console.WriteLine("If condition");
                        if (templateRow.NestedRowCount < 2)
                        {
                            table.RemoveChild(table.Elements<TableRow>().ElementAt(templateRow.RowInsertAfter));
                        }
                        else templateRow.NestedRowCount--;
                    }

                    for (int i = 1; i < templateRow.NestedRowCount; i++)
                    {
                        // Clone the row at the specified index
                        TableRow clonedNestedRow = (TableRow)table.Elements<TableRow>().ElementAt(templateRow.NestedRowIndex).CloneNode(true);
                        table.InsertAfter(clonedNestedRow, table.Elements<TableRow>().ElementAt(templateRow.NestedRowInsertAfter));

                    }
                }
            }
        }
    }



}