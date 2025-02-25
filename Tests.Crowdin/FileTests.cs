using System.IO;
using System.Text;
using Apps.Crowdin.Actions;
using Apps.Crowdin.Models.Request.File;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Request.Translation;
using Blackbird.Applications.Sdk.Common.Files;
using Crowdin.Api.Translations;
using Tests.Crowdin.Base;

namespace Tests.Crowdin
{
    [TestClass]
    public class FileTests : TestBase
    {
        [TestMethod]
        public async Task AddSpreadsheetFileOneLanguage_ReturnsSuccess()
        {
            var fileRef = new FileReference
            {
                Name = "test1.xlsx"
            };

            var inputRequest = new AddNewSpreadsheetFileRequest
            {
                File = fileRef,
                Name = "TestLang7.xlsx",
                Title = "File one lang XLSX",
                FirstLineContainsHeader = true,
                ImportTranslations = true,
                ImportHiddenSheets = true,
                ContentSegmentation = false,
                SourcePhraseColumnNumber= 1,
                TranslationColumnNumber = 2,
                ContextColumnNumber = 0,
                ImportEachCellAsSeparateSourceString = true
            };

            var projectRequest = new ProjectRequest { ProjectId = "19" };
            var action = new FileActions(InvocationContext, FileManager);
            var result = await action.AddSpreadsheetFile(projectRequest, inputRequest);

            Console.WriteLine($"New file ID in Crowdin: {result.Id}");
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public async Task AddSpreadsheetFileMultilanguage_ReturnsSuccess()
        {
            var fileRef = new FileReference
            {
                Name = "EN_ML_Sample2.xlsx"
            };

            var inputRequest = new AddNewSpreadsheetFileRequest
            {
                File = fileRef,
                //Name = "TestLangMulti4.xlsx",
                //Title = "Multilingual XLSX",
                //FirstLineContainsHeader = true,
                //ImportTranslations = true,
                //ImportHiddenSheets = true,
                //ContentSegmentation = false,
                //ContextColumnNumber = 0,
                //SourcePhraseColumnNumber = 1,
                //LanguageCodes = new[] { "zh-CN", "ko" },
                //LanguageColumnNumbers = new[] {  2, 4 },
                ImportEachCellAsSeparateSourceString = true
            };

            var projectRequest = new ProjectRequest { ProjectId = "19" };
            var action = new FileActions(InvocationContext, FileManager);
            var result = await action.AddSpreadsheetFile(projectRequest, inputRequest);

            Console.WriteLine($"New file ID in Crowdin: {result.Id}");
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public async Task AddFile_ReturnsSuccess()
        {
           var action = new FileActions(InvocationContext, FileManager);
           var input1 = new ProjectRequest { ProjectId = "750225" };
           var input2 = new AddNewFileRequest {File=new FileReference { Name = "test.csv" }, Name="New test file1" };

           var response = await action.AddFile(input1, input2);
            Console.WriteLine(response.Id);
            Assert.IsNotNull(response);

        }

        [TestMethod]
        public async Task ListFile_ReturnsSuccess()
        {
            var action = new FileActions(InvocationContext, FileManager);
            var input1 = new ProjectRequest { ProjectId = "19" };
            var input2 = new ListFilesRequest { };

            var response = await action.ListFiles(input1, input2);
            foreach (var file in response.Files)
            {
                Console.WriteLine($"{file.Id} - {file.Name}");
                Assert.IsNotNull(response);
            }
        }


        [TestMethod]
        public async Task DeleteFile_ReturnsSuccess()
        {
            var action = new FileActions(InvocationContext, FileManager);
            var input1 = new ProjectRequest { ProjectId = "750225" };
            var input2 = "12";

            await action.DeleteFile(input1, input2);
            Assert.IsTrue(true);

        }


        [TestMethod]
        public async Task GetFileProgress_ReturnsSuccess()
        {
            var action = new FileActions(InvocationContext, FileManager);
            var input1 = new ProjectRequest { ProjectId = "19" };
            var input2 = new FileRequest { FileId= "581" };

            var progress=await action.GetFileProgress(input1, input2);

            foreach(var item in progress.Progress){
                Console.WriteLine($"{item.LanguageId} - {item.LanguageName} - {item.TranslationProgress} - {item.ApprovalProgress}");
                Assert.IsTrue(true);
            }

        }

        [TestMethod]
        public async Task GetLanguageProgress_ReturnsSuccess()
        {
            var action = new TranslationActions(InvocationContext, FileManager);
            var input1 = new ProjectRequest { ProjectId = "19" };
            var input2 = new LanguageRequest { LanguageId = "zh-CN" };

            var progress = await action.GetLanguageProgress(input1, input2);

            foreach (var item in progress.Data)
            {
                Console.WriteLine($"{item.FileId} - {item.TranslationProgress} - {item.ApprovalProgress}");
                Assert.IsTrue(true);
            }

        }


        [TestMethod]
        public async Task ExportProjectTranslation_ReturnsSuccess()
        {
            var action = new TranslationActions(InvocationContext, FileManager);
            var input1 = new ProjectRequest { ProjectId = "19" };
            var input2 = new FileIdsRequest { Ids = new[] { "581", "583"}, Format = "android" };
            var input3 = new LanguageRequest {LanguageId= "zh-CN" };

            var response = await action.ExportProjectTranslation(input1, input3, input2);
            Console.WriteLine(response.Data.Url);
            Assert.IsNotNull(response);
        }
    }
}
