using System.IO;
using System.Text;
using Apps.Crowdin.Actions;
using Apps.Crowdin.Models.Request.File;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common.Files;
using Tests.Crowdin.Base;

namespace Tests.Crowdin
{
    [TestClass]
    public class FileTests : TestBase
    {
        [TestMethod]
        public async Task AddSpreadsheetFile_ReturnsSuccess()
        {
            var csvContent = "header1,header2\nvalue1,value2";
            var csvBytes = Encoding.UTF8.GetBytes(csvContent);
            var csvStream = new MemoryStream(csvBytes);

            var csvFileReference = new FileReference
            {
                Name = "test.csv"
            };

            var action = new FileActions(InvocationContext, FileManager);
            var input1 = new AddNewSpreadsheetFileRequest
            {
                File = csvFileReference,
                Name = "Test Name",
                Context = "Test",
                Title = "Test Title",
                ContentSegmentation = true,
                FirstLineContainsHeader = true,
                ImportTranslations = true,
                SrxStorageId = 1,
                ContextColumnNumber = 1,
                IdentifierColumnNumber = 1,
                LabelsColumnNumber = 1,
                MaxLengthColumnNumber = 1,
            }; 
            
            var input2 = new ProjectRequest { ProjectId = "750225" };

            var result = await action.AddSpreadsheetFile(input2, input1);

            Console.WriteLine(result.Id);
            Assert.IsNotNull(result);

        }
    }
}
