﻿using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Request.Glossary;
using Apps.Crowdin.Models.Response.Glossaries;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Glossaries.Utils.Converters;
using Blackbird.Applications.Sdk.Utils.Utilities;
using Crowdin.Api;
using Crowdin.Api.Glossaries;
using ImportGlossaryRequest = Crowdin.Api.Glossaries.ImportGlossaryRequest;

namespace Apps.Crowdin.Actions;

[ActionList]
public class GlossariesActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : AppInvocable(invocationContext)
{
    [Action("Export glossary", Description = "Export glossary as TBX file")]
    public async Task<ExportGlossaryResponse> ExportGlossaryAsync([ActionParameter] GetGlossaryRequest request)
    {
        var client = SdkClient;

        if (!int.TryParse(request.GlossaryId, out var glossaryId))
        {
            throw new PluginMisconfigurationException("Invalid Glossary ID format. Please check your input and try again");
        }
        var format = String.IsNullOrEmpty(request.Format) ? "tbx" : request.Format; 
        var exportGlossary = await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await client.ExportGlossaryAsync(glossaryId, format));
        Task.Delay(3000).Wait();
        var downloadLink = await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await client.Glossaries.DownloadGlossary(glossaryId, exportGlossary.Identifier));

        var fileContent = await ExceptionWrapper.ExecuteWithErrorHandling(() => FileDownloader.DownloadFileBytes(downloadLink.Url));
        await FileOperationWrapper.ExecuteFileOperation(() => Task.CompletedTask, fileContent.FileStream, fileContent.Name);

        if (format != "tbx")
        {
            var Glossary = await fileManagementClient.UploadAsync(fileContent.FileStream, fileContent.ContentType,
          fileContent.Name ?? "Glossary." + format);
            return new(Glossary);
        }

        string glossaryTitle = fileContent.Name ?? "Glossary.tbx";
        var glossaryExporter = new GlossaryExporter(fileContent.FileStream);
        var glossary = await glossaryExporter.ExportGlossaryAsync(glossaryTitle);

        await using var tbxStream = glossary.ConvertToTbx();
        string contentType = MimeTypes.GetMimeType(glossaryTitle);
        var tbxFileReference = await fileManagementClient.UploadAsync(tbxStream, contentType,
            glossaryTitle);

        return new(tbxFileReference);
    }
    
    [Action("Import glossary", Description = "Import glossary from TBX file")]
    public async Task<ImportGlossaryResponse> ImportGlossaryAsync(
        [ActionParameter]  Apps.Crowdin.Models.Request.Glossary.ImportGlossaryRequest request)
    {
        var client = SdkClient;
        using var memoryStream = new MemoryStream();

        await using var file = await FileOperationWrapper.ExecuteFileDownloadOperation(() => fileManagementClient.DownloadAsync(request.File), request.File.Name);

        var fileMemoryStream = new MemoryStream();
        await file.CopyToAsync(fileMemoryStream);

        var glossaryImporter = new GlossaryImporter(fileMemoryStream);
        var xDocument = await glossaryImporter.ConvertToCrowdinFormat();

        xDocument.Save(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        string glossaryName = request.GlossaryName ?? request.File.Name.Replace(".tbx", string.Empty);
        string languageCode = request.LanguageCode ?? "en";
        var glossaryResponse = await client.Glossaries.AddGlossary(new AddGlossaryRequest
        {
            Name = glossaryName,
            LanguageId = languageCode,
            GroupId = string.IsNullOrEmpty(request.GroupId) ? null : int.Parse(request.GroupId)
        });

        var storageResponse =
            await client.Storage.AddStorage(memoryStream, request.File?.Name ?? $"{glossaryName}.tbx");
        var importGlossaryRequest = new ImportGlossaryRequest
        {
            StorageId = storageResponse.Id,
            FirstLineContainsHeader = false
        };

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await SdkClient.Glossaries.ImportGlossary(glossaryResponse.Id, importGlossaryRequest));
        
        if (response.Status != OperationStatus.Created && response.Status != OperationStatus.Finished &&
            response.Status != OperationStatus.InProgress)
        {
            throw new PluginApplicationException($"Glossary import failed, status: {response.Status}");
        }

        return new ImportGlossaryResponse
        {
            Identifier = response.Identifier,
            Status = response.Status.ToString(),
            Progress = response.Progress.ToString()
        };
    }
}