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

        public async Task MailMergeWorkDocument(string templatePath, string DestinatePath, List<keyValue> keyValuePairs)
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
                var allParas = doc.MainDocumentPart?.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>();

                foreach (var keyValue in keyValuePairs)
                {
                    List<string> addedList = new List<string>();
                    foreach (Text textItem in allParas ?? new List<Text>())
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
               CleanUpUnMergedTemplateFields(allParas);

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
        }
    }



}