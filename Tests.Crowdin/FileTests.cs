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
            var csvFileReference = new FileReference
            {
                Name = "test.csv"
            };

            var action = new FileActions(InvocationContext, FileManager);
            var input1 = new AddNewSpreadsheetFileRequest
            {
                File = csvFileReference,
                Name = "Test Name12",
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


        [TestMethod]
        public async Task AddFile_ReturnsSuccess()
        {
           var action = new FileActions(InvocationContext, FileManager);
           var input1 = new ProjectRequest { ProjectId = "750225" };
           var input2 = new AddNewFileRequest {File=new FileReference { Name = "test.csv" }, Name="~New test file" };

           var response = await action.AddFile(input1, input2);
            Console.WriteLine(response.Id);
            Assert.IsNotNull(response);

        }


        [TestMethod]
        public async Task DeleteFile_ReturnsSuccess()
        {
            var action = new FileActions(InvocationContext, FileManager);
            var input1 = new ProjectRequest { ProjectId = "750225" };
            var input2 = "234";

            var response = action.DeleteFile(input1, input2);
            Assert.IsNotNull(response);

        }
    }
}
