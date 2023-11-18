using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common;

namespace Application.Interfaces
{
    public interface IFileProcessingService
    {
        public Task MailMergeWorkDocument(string templatePath, string DestinatePath, List<keyValue> keyValuePairs);

    }
}