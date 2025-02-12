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
                Name = "CN_EN Sample 2.csv"
            };

            var action = new FileActions(InvocationContext, FileManager);
            var input1 = new AddNewSpreadsheetFileRequest
            {
                File = csvFileReference,
                Name = "TestAA11AAfffx",
                Context = "Test",
                Title = "Test Title11",
                ContentSegmentation = true,
                FirstLineContainsHeader = true,
                ImportTranslations = true,
                ImportEachCellAsSeparateSourceString = true,
                ImportHiddenSheets = true
            };

            var input2 = new ProjectRequest { ProjectId = "750225" };

            var result = await action.AddSpreadsheetFile(input2, input1);

            Console.WriteLine(result.Id);
            Assert.IsTrue(true);
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
        public async Task ListFile_ReturnsSuccess()
        {
            var action = new FileActions(InvocationContext, FileManager);
            var input1 = new ProjectRequest { ProjectId = "750225" };
            var input2 = new ListFilesRequest { };

            var response = await action.ListFiles(input1, input2);
            foreach (var file in response.Files)
            {
                Console.WriteLine(file.Id);
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
    }
}
