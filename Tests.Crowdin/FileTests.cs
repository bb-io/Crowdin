using Apps.Crowdin.Actions;
using Apps.Crowdin.Models.Request.File;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Request.Translation;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Newtonsoft.Json;
using Tests.Crowdin.Base;

namespace Tests.Crowdin;

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

    //AddOrUpdateFile

    [TestMethod]
    public async Task AddOrUpdateFile_ReturnsSuccess()
    {
        var action = new FileActions(InvocationContext, FileManager);
        var input1 = new ProjectRequest { ProjectId = "3" };
        var input2 = new AddOrUpdateFileRequest { File = new FileReference { Name = "Testing - PDF not searchable.pdf" } };

        var response = await action.AddOrUpdateFile(input1, input2);
        Console.WriteLine(response.Id);
        Assert.IsNotNull(response);
    }


    [TestMethod]
    public async Task ListFile_ReturnsSuccess()
    {
        var action = new FileActions(InvocationContext, FileManager);
        var input1 = new ProjectRequest { ProjectId = "783572" };
        var input2 = new ListFilesRequest { Recursive=true, CreatedBefore = DateTime.UtcNow.AddDays(-100) };

        var response = await action.ListFiles(input1, input2);
        Console.WriteLine(response.Files.Count());
        foreach (var file in response.Files)
        {
            Console.WriteLine($"{file.Id} - {file.Name}");
            Assert.IsNotNull(response);
        }
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(response, Formatting.Indented);
        Console.WriteLine(json);
        
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
    public async Task GetFileEnterpriseProgress_ReturnsSuccess()
    {
        var action = new FileActions(InvocationContext, FileManager);
        var input1 = new ProjectRequest { ProjectId = "614573" };
        var input2 = new FileRequest { FileId= "95064" };

        var progress=await action.GetFileProgress(input1, input2);

        foreach(var item in progress.Progress){
            Console.WriteLine($"{item.LanguageId} - {item.LanguageName} - {item.TranslationProgress} - {item.ApprovalProgress}");
            Assert.IsTrue(true);
        }

    }

    [TestMethod]
    public async Task GetFile_ReturnsSuccess()
    {
        var action = new FileActions(InvocationContext, FileManager);
        var input1 = new ProjectRequest { ProjectId = "134" };
        var input2 = new FileRequest { FileId = "10616" };

        //var input1 = new ProjectRequest { ProjectId = "596457" };
        //var input2 = new FileRequest { FileId = "149138" };

        var file = await action.GetFile(input1, input2);

        var json = JsonConvert.SerializeObject(file, Formatting.Indented);
        Console.WriteLine(json);

        Assert.IsNotNull(file);
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
        var input2 = new FileIdsRequest { Ids = new[] { "581"}, Format = "xliff" };
        var input3 = new LanguageRequest {LanguageId= "zh-CN" };

        var response = await action.ExportProjectTranslation(input1, input3, input2);
        Console.WriteLine(response.Data.Url);
        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task AddFileTranslation_ReturnsSuccess()
    {
        var action = new TranslationActions(InvocationContext, FileManager);
       
        var response = await action.AddFileTranslation(new AddNewFileTranslationRequest {LanguageId="fr", 
            ProjectId= "665202",
            SourceFileId= "3499",
            File = new FileReference { Name = "test1.xlsx" },
        });
        Console.WriteLine(response);
        Assert.IsNotNull(response);

    }

    [TestMethod]
    public async Task GetFileProgress_ExistingFile_ReturnsSuccess()
    {
        // Arrange
        var actions = new FileActions(InvocationContext, FileManager);
        var project = new ProjectRequest { ProjectId = "1" };
        var fileRequest = new FileRequest { FileId = "1" };

        // Act
        var result = await actions.GetFileProgress(project, fileRequest);

        // Assert
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetFileProgress_NonExistingFile_ReturnsSuccess()
    {
        // Arrange
        var actions = new FileActions(InvocationContext, FileManager);
        var project = new ProjectRequest { ProjectId = "1" };
        var fileRequest = new FileRequest { FileId = "111111111" };

        // Act
        var ex = await Assert.ThrowsExceptionAsync<PluginApplicationException>(
            async () => await actions.GetFileProgress(project, fileRequest)
        );

        // Assert
        StringAssert.Contains(ex.Message, "Code: 404; Message: File Not Found");
    }
}
