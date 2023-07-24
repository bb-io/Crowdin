﻿using Apps.Crowdin.Api;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.TranslationMemory;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Models.Response.TranslationMemory;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Crowdin.Api.TranslationMemory;

namespace Apps.Crowdin.Actions;

[ActionList]
public class TranslationMemoryActions
{
    [Action("List translation memories", Description = "List all translation memories")]
    public async Task<ListTranslationMemoriesResponse> ListTranslationMemories(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] ListTranslationMemoryRequest input)
    {
        var intUserId = IntParser.Parse(input.UserId, nameof(input.UserId));
        var intGroupId = IntParser.Parse(input.GroupId, nameof(input.GroupId));

        var client = new CrowdinClient(creds);

        var items = await Paginator.Paginate((lim, offset)
            => client.TranslationMemory.ListTms(intUserId, intGroupId, lim, offset));

        var tms = items.Select(x => new TranslationMemoryEntity(x)).ToArray();
        return new(tms);
    }

    [Action("Get translation memory", Description = "Get specific translation memory")]
    public async Task<TranslationMemoryEntity> GetTranslationMemory(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Translation memory ID")]
        string translationMemoryId)
    {
        var intTmId = IntParser.Parse(translationMemoryId, nameof(translationMemoryId));
        var client = new CrowdinClient(creds);

        var response = await client.TranslationMemory.GetTm(intTmId!.Value);
        return new(response);
    }

    [Action("Add translation memory", Description = "Add new translation memory")]
    public async Task<TranslationMemoryEntity> AddTranslationMemory(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] AddTranslationMemoryRequest input)
    {
        var intGroupId = IntParser.Parse(input.GroupId, nameof(input.GroupId));
        var client = new CrowdinClient(creds);

        var request = new AddTmRequest
        {
            Name = input.Name,
            LanguageId = input.LanguageId,
            GroupId = intGroupId
        };
        var response = await client.TranslationMemory.AddTm(request);

        return new(response);
    }

    [Action("Delete translation memory", Description = "Delete specific translation memory")]
    public Task DeleteTranslationMemory(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Translation memory ID")] string translationMemoryId)
    {
        var intTmId = IntParser.Parse(translationMemoryId, nameof(translationMemoryId));
        var client = new CrowdinClient(creds);

        return client.TranslationMemory.DeleteTm(intTmId!.Value);
    }
    
    [Action("Export translation memory", Description = "Export specific translation memory")]
    public async Task<TmExportEntity> ExportTranslationMemory(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Translation memory ID")] string translationMemoryId,
        [ActionParameter] ExportTranslationMemoryRequest input)
    {
        var intTmId = IntParser.Parse(translationMemoryId, nameof(translationMemoryId));

        var formatEnum = TmFileFormat.Tmx;
        if (input.Format != null && !Enum.TryParse(input.Format, out formatEnum))
            throw new("Wrong format value, acceptable values are: tmx, csv, xlsx");
        
        var client = new CrowdinClient(creds);

        
        var request = new ExportTmRequest
        {
            SourceLanguageId = input.SourceLanguageId,
            TargetLanguageId = input.TargetLanguageId,
            Format = formatEnum
        };
        var response = await client.TranslationMemory.ExportTm(intTmId!.Value, request);
        
        return new(response);
    }
    
    [Action("Download translation memory", Description = "Download specific translation memory")]
    public async Task<DownloadFileResponse> DownloadTranslationMemory(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] DownloadTranslationMemoryRequest input)
    {
        var intTmId = IntParser.Parse(input.TrasnlationMemoryId, nameof(input.TrasnlationMemoryId));
        var client = new CrowdinClient(creds);

        var response = await client.TranslationMemory.DownloadTm(intTmId!.Value, input.ExportId);
        var fileContent = await FileDownloader.DownloadFileBytes(response.Url);
        
        return new(fileContent);
    }
}