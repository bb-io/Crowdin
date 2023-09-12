using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.StringTranslations;
using Crowdin.Api.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Crowdin.Models.Entities
{
    public class FileTranslationEntity
    {
        [Display("File ID")]
        public string FileId { get; set; }

        [Display("Language ID")]
        public string LanguageId { get; set; }

        [Display("Project ID")]
        public string ProjectId { get; set; }

        [Display("Storage ID")]
        public string StorageId { get; set; }

        public FileTranslationEntity(UploadTranslationsResponse fileTranslation)
        {
            FileId = fileTranslation.FileId.ToString();
            LanguageId = fileTranslation.LanguageId.ToString();
            ProjectId = fileTranslation.ProjectId.ToString();
            StorageId = fileTranslation.StorageId.ToString();
        }
    }
}
