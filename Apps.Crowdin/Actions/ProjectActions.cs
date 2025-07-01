using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Api.RestSharp.Basic;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Models.Response.Project;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Utils.Utilities;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Crowdin.Api.ProjectsGroups;
using Crowdin.Api.Reports;
using Crowdin.Api.Translations;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace Apps.Crowdin.Actions;

[ActionList]
public class ProjectActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : AppInvocable(invocationContext)
{
    [Action("Search projects", Description = "List all projects")]
    public async Task<ListProjectsResponse> ListProjects([ActionParameter] ListProjectsRequest input)
    {
        var userId = IntParser.Parse(input.UserId, nameof(input.UserId));
        var groupId = IntParser.Parse(input.GroupID, nameof(input.GroupID));

        var items = await Paginator.Paginate((lim, offset)
            => ExceptionWrapper.ExecuteWithErrorHandling(() =>
                SdkClient.ProjectsGroups.ListProjects<ProjectBase>(userId, groupId, input.HasManagerAccess ?? false,
                    null, lim, offset)));

        var projects = items.Select(x => new ProjectEntity(x)).ToArray();
        return new(projects);
    }

    [Action("Get project", Description = "Get specific project")]
    public async Task<ProjectEntity> GetProject([ActionParameter] ProjectRequest project)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.ProjectsGroups.GetProject<ProjectBase>(intProjectId!.Value));
        return new(response);
    }

    [Action("Add project", Description = "Add new project")]
    public async Task<ProjectEntity> AddProject([ActionParameter] AddNewProjectRequest input)
    {
        var request = new StringsBasedProjectForm
        {
            Name = input.Name,
            SourceLanguageId = input.SourceLanguageId,
            Identifier = input.Identifier,
            Visibility = EnumParser.Parse<ProjectVisibility>(input.Visibility, nameof(input.Visibility)),
            TargetLanguageIds = input.TargetLanguageIds?.ToList(),
            Cname = input.Cname,
            Description = input.Description,
            IsMtAllowed = input.IsMtAllowed,
            AutoSubstitution = input.AutoSubstitution,
            AutoTranslateDialects = input.AutoTranslateDialects,
            PublicDownloads = input.PublicDownloads,
            HiddenStringsProofreadersAccess = input.HiddenStringsProofreadersAccess,
            UseGlobalTm = input.UseGlobalTm,
            SkipUntranslatedStrings = input.SkipUntranslatedStrings,
            SkipUntranslatedFiles = input.SkipUntranslatedFiles,
            ExportApprovedOnly = input.ExportApprovedOnly,
            InContext = input.InContext,
            InContextProcessHiddenStrings = input.InContextProcessHiddenStrings,
            InContextPseudoLanguageId = input.InContextPseudoLanguageId,
            QaCheckIsActive = input.QaCheckIsActive,
        };

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.ProjectsGroups.AddProject<ProjectBase>(request));
        return new(response);
    }

    [Action("Delete project", Description = "Delete specific project")]
    public async Task DeleteProject([ActionParameter] ProjectRequest project)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.ProjectsGroups.DeleteProject(intProjectId!.Value));
    }

    [Action("Build project", Description = "Build project translation")]
    public async Task<ProjectBuildEntity> BuildProject([ActionParameter] ProjectRequest project,
        [ActionParameter] BuildProjectRequest input)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.Translations.BuildProjectTranslation(intProjectId!.Value,
                new EnterpriseTranslationCreateProjectBuildForm
                {
                    BranchId = IntParser.Parse(input.BranchId, nameof(input.BranchId)),
                    TargetLanguageIds = input.TargetLanguageIds?.ToList(),
                    SkipUntranslatedStrings = input.SkipUntranslatedStrings,
                    SkipUntranslatedFiles = input.SkipUntranslatedFiles,
                    ExportWithMinApprovalsCount = input.ExportWithMinApprovalsCount
                }));

        return new(response);
    }

    [Action("Download translations as ZIP", Description = "Download project translations as ZIP")]
    public async Task<DownloadFileResponse> DownloadTranslationsAsZip(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] BuildRequest build)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId))!.Value;
        var intBuildId = IntParser.Parse(build.BuildId, nameof(build.BuildId))!.Value;

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await SdkClient.Translations.DownloadProjectTranslations(intProjectId, intBuildId));

        if (response.Link is null)
        {
            throw new PluginMisconfigurationException("Project build is in progress, you can't download the data now");
        }

        var file = await ExceptionWrapper.ExecuteWithErrorHandling(() => FileDownloader.DownloadFileBytes(response.Link.Url));
        await FileOperationWrapper.ExecuteFileOperation(() => Task.CompletedTask, file.FileStream, file.Name);

        file.Name = $"{project.ProjectId}.zip";

        var memoryStream = new MemoryStream();
        await file.FileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var fileReference = await fileManagementClient.UploadAsync(memoryStream, file.ContentType, file.Name);
        return new(fileReference);
    }

    [Action("Download translations", Description = "Download project translations")]
    public async Task<DownloadFilesResponse> DownloadTranslations(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] BuildRequest build)
    {
        if (!int.TryParse(build.BuildId, out int intBuildID)) 
        {
            throw new PluginMisconfigurationException($"Invalid Build ID: {build.BuildId} must be a numeric value. Please check the input Build ID");
        }

        var filesArchive = await DownloadTranslationsAsZip(project, build);

        var zipFile = await FileOperationWrapper.ExecuteFileDownloadOperation(() => fileManagementClient.DownloadAsync(filesArchive.File), filesArchive.File.Name);
        var files = await zipFile.GetFilesFromZip();

        var result = files.Where(x => x.FileStream.Length > 0).ToArray();

        // Cleaning files path from the root folder of the archive
        result.ToList().ForEach(x =>
            x.Path = string.Join('/', x.Path.Split("/").Skip(1)));

        var tasks = result.Select(x =>
        {
            var contentType = MimeTypes.GetMimeType(x.UploadName);
            return fileManagementClient.UploadAsync(x.FileStream, contentType, x.UploadName);
        }).ToList();

        var fileReferences = await Task.WhenAll(tasks);
        return new(fileReferences.ToList());
    }

    [Action("Download file translations from build", Description = "Download one file from project translations")]
    public async Task<DownloadFileResponse> DownloadFileTranslations(
    [ActionParameter] ProjectRequest project,
    [ActionParameter] BuildRequest build,
    [ActionParameter] [Display("File name")]string FileName)
    {
        if (!int.TryParse(build.BuildId, out int intBuildID))
        {
            throw new PluginMisconfigurationException($"Invalid Build ID: {build.BuildId} must be a numeric value. Please check the input Build ID");
        }

        var filesArchive = await DownloadTranslationsAsZip(project, build);

        var zipFile = await FileOperationWrapper.ExecuteFileDownloadOperation(() => fileManagementClient.DownloadAsync(filesArchive.File), filesArchive.File.Name);
        var files = await zipFile.GetFilesFromZip();

        var result = files.FirstOrDefault(x => x.FileStream.Length > 0 && x.UploadName == FileName);

        if (result == null)
        { return null; }

        result.Path = string.Join('/', result.Path.Split("/").Skip(1));

        var contentType = MimeTypes.GetMimeType(result.UploadName);
        var response = await fileManagementClient.UploadAsync(result.FileStream, contentType, result.UploadName);
        
        return new(response);
    }


    [Action("Generate translation costs post-editing", Description = "Generates report")]
    public async Task<Models.Response.Project.TranslationCostReportResponse> GenerateTranslateCostReport([ActionParameter] ProjectRequest project,
       [ActionParameter] GenerateTranslationCostReportOptions options)
    {
        var reportRequest = new CrowdinRestRequest($"/projects/{project.ProjectId}/reports", Method.Post, InvocationContext.AuthenticationCredentialsProviders);
        var action = new CrowdinRestClient();
        reportRequest.AddJsonBody(new
        {
            name = "translation-costs-pe",
            schema = new
            {
                unit = "words",
                currency = options.Currency ?? "USD",
                format = "json",
                baseRates = new
                {
                    fullTranslation = options.BaseFullTranslations ?? 0.10f,
                    proofread = options.BaseProofRead ?? 0.05f
                },
                individualRates = (options.LanguageIds?.Any() == true || options.UserIds?.Any() == true)
                    ? new[]
                    {
                    new {
                        languageIds     = options.LanguageIds,
                        userIds         = options.UserIds,
                        fullTranslation = options.IndividualFullTranslations ?? 0.10f,
                        proofread       = options.IndividualProofRead        ?? 0.05f
                    }
                    }
                    : null,
                netRateSchemes = new
                {
                    tmMatch = new[]
                    {
                    new { matchType = "perfect", price = options.TmMatchType == "perfect" ? (options.TmPrice ?? 0.02f) : 0.0f },
                    new { matchType = "100",     price = options.TmMatchType == "100"     ? (options.TmPrice ?? 0.02f) : 0.0f },
                    new { matchType = "99-82",   price = options.TmMatchType == "99-82"   ? (options.TmPrice ?? 0.02f) : 0.0f },
                    new { matchType = "81-60",   price = options.TmMatchType == "81-60"   ? (options.TmPrice ?? 0.02f) : 0.0f }
                },
                    mtMatch = new[]
                    {
                    new { matchType = "100",   price = options.MtMatchType == "100"   ? (options.MtPrice ?? 0.01f) : 0.0f },
                    new { matchType = "99-82", price = options.MtMatchType == "99-82" ? (options.MtPrice ?? 0.01f) : 0.0f },
                    new { matchType = "81-60", price = options.MtMatchType == "81-60" ? (options.MtPrice ?? 0.01f) : 0.0f }
                },
                    suggestionMatch = new[]
                    {
                    new { matchType = "100",   price = options.SuggestMatchType == "100"   ? (options.SuggestPrice ?? 0.03f) : 0.0f },
                    new { matchType = "99-82", price = options.SuggestMatchType == "99-82" ? (options.SuggestPrice ?? 0.03f) : 0.0f }
                }
                },
                dateFrom = options.FromDate?.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss+00:00"),
                dateTo = options.ToDate?.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss+00:00"),
                languageIds = options.LanguageIds,
                userIds = options.UserIds
            }
        });

        var generateReport = await ExceptionWrapper.ExecuteWithErrorHandling(
            () => action.ExecuteWithErrorHandling<GenerateReportResponse>(reportRequest)
        );

        Task.Delay(5000).Wait();


        var downloadRequest = new CrowdinRestRequest(
            $"/projects/{project.ProjectId}/reports/{generateReport.Data.Identifier}/download",
            Method.Get,
            InvocationContext.AuthenticationCredentialsProviders
        );
        var downloadResponse = await ExceptionWrapper.ExecuteWithErrorHandling(
            () => action.ExecuteWithErrorHandling<GetReportResponse>(downloadRequest)
        );

        if (downloadResponse.Data.Url is null)
            throw new PluginApplicationException("Report is still being generated, please try again later.");

        using var file = await ExceptionWrapper.ExecuteWithErrorHandling(
            () => FileDownloader.DownloadFileBytes(downloadResponse.Data.Url)
        );
        using var memoryStream = new MemoryStream();
        await file.FileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var jsonDocument = await JsonDocument.ParseAsync(memoryStream);
        var root = jsonDocument.RootElement;

        var totalWordsDecimal = root.GetProperty("preTranslated").GetProperty("total").GetDecimal();
        var weightedWordsDecimal = root.GetProperty("weightedUnits").GetProperty("total").GetDecimal();

        var translationCostsArr = ParseBreakdownObject(root.GetProperty("translationCosts"));
        var savingsArr = ParseBreakdownObject(root.GetProperty("savings"));
        var weightedArr = ParseBreakdownObject(root.GetProperty("weightedUnits"));
        var preTransArr = ParseBreakdownObject(root.GetProperty("preTranslated"));

        var response = new Models.Response.Project.TranslationCostReportResponse
        {
            TotalWords = (int)totalWordsDecimal,
            WeightedWords = weightedWordsDecimal,
            TaskName = root.GetProperty("name").GetString() ?? string.Empty,
            TranslationCost = root.GetProperty("totalCosts").GetDecimal(),
            ProofreadingCost = root.GetProperty("approvalCosts").GetProperty("total").GetDecimal(),
            EstimatedTMSavingsTotal = root.GetProperty("savings").GetProperty("total").GetDecimal(),
            Currency = root.GetProperty("currency").GetString(),

            TranslationCosts = translationCostsArr,
            Savings = savingsArr,
            WeightedUnits = weightedArr,
            PreTranslated = preTransArr
        };

        return response;
    }

    [Action("Generate estimation post-editing cost", Description = "Generates report")]
    public async Task<EstimateCostReportResponse> GenerateCostReport([ActionParameter] ProjectRequest project,
      [ActionParameter] GenerateEstimateCostReportOptions options)
    {
        var reportRequest = new CrowdinRestRequest($"/projects/{project.ProjectId}/reports", Method.Post, InvocationContext.AuthenticationCredentialsProviders);
        var action = new CrowdinRestClient();
        reportRequest.AddJsonBody(new
        {
            name = "costs-estimation-pe",
            schema = new
            {
                unit = "words",
                currency = "USD",
                format = "json",
                baseRates = new
                {
                    fullTranslation = options.BaseFullTranslations ?? 0.10f,
                    proofread = options.BaseProofRead ?? 0.05f
                },
                individualRates = options.LanguageIds?.Any() == true
                ? new[]
                {
                    new
                    {
                        languageIds = options.LanguageIds,
                        fullTranslation = options.IndividualFullTranslations ?? 0.10f,
                        proofread = options.IndividualProofRead ?? 0.05f
                    }
                }
                : null,
                netRateSchemes = new
                {
                    tmMatch = new[]
                {
                    new { matchType = "perfect", price = options.TmMatchType == "perfect" ? (options.TmPrice ?? 0.02f) : 0.0f },
                    new { matchType = "100", price = options.TmMatchType == "100" ? (options.TmPrice ?? 0.02f) : 0.0f },
                    new { matchType = "99-82", price = options.TmMatchType == "99-82" ? (options.TmPrice ?? 0.02f) : 0.0f },
                    new { matchType = "81-60", price = options.TmMatchType == "81-60" ? (options.TmPrice ?? 0.02f) : 0.0f }
                }
                },
                dateFrom = options.FromDate?.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss+00:00"),
                dateTo = options.ToDate?.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss+00:00"),
                languageIds = options.LanguageIds
            }
        });

        var generateReport = await ExceptionWrapper.ExecuteWithErrorHandling(() => action.ExecuteWithErrorHandling<GenerateReportResponse>(reportRequest));
        var downloadRequest = new CrowdinRestRequest($"/projects/{project.ProjectId}/reports/{generateReport.Data.Identifier}/download", Method.Get, InvocationContext.AuthenticationCredentialsProviders);
        var downloadResponse = await ExceptionWrapper.ExecuteWithErrorHandling(() => action.ExecuteWithErrorHandling<GetReportResponse>(downloadRequest));

        if (downloadResponse.Data.Url is null)
        {
            throw new PluginApplicationException("Report is still being generated, please try again later.");
        }

        using var file = await ExceptionWrapper.ExecuteWithErrorHandling(() => FileDownloader.DownloadFileBytes(downloadResponse.Data.Url));

        using var memoryStream = new MemoryStream();
        await file.FileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var jsonDocument = await JsonDocument.ParseAsync(memoryStream);
        var root = jsonDocument.RootElement;

        int totalWords = 0;
        if (root.TryGetProperty("data", out var dataArray) && dataArray.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in dataArray.EnumerateArray())
            {
                if (item.TryGetProperty("statistics", out var stats) &&
                    stats.TryGetProperty("untranslated", out var untranslated))
                {
                    totalWords += untranslated.GetInt32();
                }
            }
        }

        var response = new EstimateCostReportResponse
        {
            TotalWords = totalWords,
            WeightedWords = root.TryGetProperty("weightedUnits", out var weightedUnits) &&
                        weightedUnits.TryGetProperty("total", out var weightedTotal) ?
                        weightedTotal.GetDecimal() : null,
            TaskName = root.TryGetProperty("name", out var name) ? name.GetString() : string.Empty,
            TranslationCost = root.TryGetProperty("translationCosts", out var transCosts) &&
                         transCosts.TryGetProperty("total", out var transTotal) ?
                         transTotal.GetDecimal() : null,
            ProofreadingCost = root.TryGetProperty("approvalCosts", out var approvalCosts) &&
                          approvalCosts.TryGetProperty("total", out var approvalTotal) ?
                          approvalTotal.GetDecimal() : null,
            EstimatedTMSavingsTotal = root.TryGetProperty("savings", out var savings) &&
                                 savings.TryGetProperty("total", out var savingsTotal) ?
                                 savingsTotal.GetDecimal() : null
        };

        return response;
    }



    [Action("Generate estimation post-editing cost by task", Description = "Generates cost estimation report for a specific task")]
    public async Task<EstimateCostReportResponse> GenerateCostReportByTask(
            [ActionParameter] ProjectRequest project,
            [ActionParameter] GenerateEstimateCostReportByTaskOptions options)
    {
        if (!int.TryParse(options.TaskId, out var parsedTaskId))
        {
            throw new PluginMisconfigurationException("Task ID must be a valid integer.");
        }

        var reportRequest = new CrowdinRestRequest($"/projects/{project.ProjectId}/reports", Method.Post, InvocationContext.AuthenticationCredentialsProviders);
        reportRequest.AddJsonBody(new
        {
            name = "costs-estimation-pe",
            schema = new
            {
                unit = "words",
                currency = "USD",
                format = "json",
                baseRates = new
                {
                    fullTranslation = options.BaseFullTranslations ?? 0.10f,
                    proofread = options.BaseProofRead ?? 0.05f
                },
                individualRates = options.LanguageIds?.Any() == true
                    ? new[]
                    {
                            new
                            {
                                languageIds = options.LanguageIds,
                                fullTranslation = options.IndividualFullTranslations ?? 0.10f,
                                proofread = options.IndividualProofRead ?? 0.05f
                            }
                    }
                    : null,
                netRateSchemes = new
                {
                    tmMatch = new[]
                    {
                            new { matchType = "perfect", price = options.TmMatchType == "perfect" ? (options.TmPrice ?? 0.02f) : 0.0f },
                            new { matchType = "100", price = options.TmMatchType == "100" ? (options.TmPrice ?? 0.02f) : 0.0f },
                            new { matchType = "99-82", price = options.TmMatchType == "99-82" ? (options.TmPrice ?? 0.02f) : 0.0f },
                            new { matchType = "81-60", price = options.TmMatchType == "81-60" ? (options.TmPrice ?? 0.02f) : 0.0f }
                        }
                },
                dateFrom = options.FromDate?.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss+00:00"),
                dateTo = options.ToDate?.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss+00:00"),
                taskId = parsedTaskId
            }
        });

        var client = new CrowdinRestClient();
        var generateResponse = await ExceptionWrapper.ExecuteWithErrorHandling(() =>
            client.ExecuteWithErrorHandling<GenerateReportResponse>(reportRequest)
        );

        Task.Delay(5000).Wait();

        var downloadRequest = new CrowdinRestRequest($"/projects/{project.ProjectId}/reports/{generateResponse.Data.Identifier}/download", Method.Get, InvocationContext.AuthenticationCredentialsProviders);
        var downloadResp = await ExceptionWrapper.ExecuteWithErrorHandling(() =>
            client.ExecuteWithErrorHandling<GetReportResponse>(downloadRequest)
        );

        if (downloadResp.Data.Url == null)
            throw new PluginApplicationException("Report is still being generated, please try again later.");

       
        using var file = await ExceptionWrapper.ExecuteWithErrorHandling(() => FileDownloader.DownloadFileBytes(downloadResp.Data.Url));
        using var ms = new MemoryStream();
        await file.FileStream.CopyToAsync(ms);
        ms.Position = 0;
        var doc = await JsonDocument.ParseAsync(ms);
        var root = doc.RootElement;

        int totalWords = root.TryGetProperty("data", out var dataArray) && dataArray.ValueKind == JsonValueKind.Array
            ? dataArray.EnumerateArray().Sum(item => item.GetProperty("statistics").GetProperty("untranslated").GetInt32())
            : 0;

        return new EstimateCostReportResponse
        {
            TotalWords = totalWords,
            WeightedWords = root.GetProperty("weightedUnits").GetProperty("total").GetDecimal(),
            TaskName = root.GetProperty("name").GetString() ?? string.Empty,
            TranslationCost = root.GetProperty("translationCosts").GetProperty("total").GetDecimal(),
            ProofreadingCost = root.GetProperty("approvalCosts").GetProperty("total").GetDecimal(),
            EstimatedTMSavingsTotal = root.GetProperty("savings").GetProperty("total").GetDecimal()
        };
    }

    [Action("Generate translation costs post-editing by task", Description = "Generates translation+proofread cost report for a specific task")]
    public async Task<TranslationCostReportResponse> GenerateTranslateCostReportByTask(
    [ActionParameter] ProjectRequest project,
    [ActionParameter] GenerateTranslationCostReportByTaskOptions options)
    {
        if (!int.TryParse(options.TaskId, out var parsedTaskId))
            throw new PluginMisconfigurationException("Task ID must be a valid integer.");

        var reportRequest = new CrowdinRestRequest($"/projects/{project.ProjectId}/reports", Method.Post, InvocationContext.AuthenticationCredentialsProviders);
        reportRequest.AddJsonBody(new
        {
            name = "translation-costs-pe",
            schema = new
            {
                unit = "words",
                currency =  options.Currency ?? "USD",
                format = "json",
                baseRates = new
                {
                    fullTranslation = options.BaseFullTranslations ?? 0.10f,
                    proofread = options.BaseProofRead ?? 0.05f
                },
                individualRates = (options.LanguageIds?.Any() == true || options.UserIds?.Any() == true)
                    ? new[]
                    {
                            new
                            {
                                languageIds = options.LanguageIds,
                                userIds = options.UserIds,
                                fullTranslation = options.IndividualFullTranslations ?? 0.10f,
                                proofread = options.IndividualProofRead ?? 0.05f
                            }
                    }
                    : null,
                netRateSchemes = new
                {
                    tmMatch = new[]
                    {
                            new { matchType = "perfect", price = options.TmMatchType == "perfect" ? (options.TmPrice ?? 0.02f) : 0.0f },
                            new { matchType = "100",     price = options.TmMatchType == "100"     ? (options.TmPrice ?? 0.02f) : 0.0f },
                            new { matchType = "99-82",   price = options.TmMatchType == "99-82"   ? (options.TmPrice ?? 0.02f) : 0.0f },
                            new { matchType = "81-60",   price = options.TmMatchType == "81-60"   ? (options.TmPrice ?? 0.02f) : 0.0f }
                        },
                    mtMatch = new[]
                    {
                            new { matchType = "100",   price = options.MtMatchType == "100"   ? (options.MtPrice ?? 0.01f) : 0.0f },
                            new { matchType = "99-82", price = options.MtMatchType == "99-82" ? (options.MtPrice ?? 0.01f) : 0.0f },
                            new { matchType = "81-60", price = options.MtMatchType == "81-60" ? (options.MtPrice ?? 0.01f) : 0.0f }
                        },
                    suggestionMatch = new[]
                    {
                            new { matchType = "100",   price = options.SuggestMatchType == "100"   ? (options.SuggestPrice ?? 0.03f) : 0.0f },
                            new { matchType = "99-82", price = options.SuggestMatchType == "99-82" ? (options.SuggestPrice ?? 0.03f) : 0.0f }
                        }
                },
                dateFrom = options.FromDate?.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss+00:00"),
                dateTo = options.ToDate?.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss+00:00"),
                taskId = parsedTaskId
            }
        });

        var client = new CrowdinRestClient();
        var genResp = await ExceptionWrapper.ExecuteWithErrorHandling(() =>
            client.ExecuteWithErrorHandling<GenerateReportResponse>(reportRequest)
        );

        await Task.Delay(5000);

        var dlRequest = new CrowdinRestRequest($"/projects/{project.ProjectId}/reports/{genResp.Data.Identifier}/download", Method.Get, InvocationContext.AuthenticationCredentialsProviders);
        var dlResp = await ExceptionWrapper.ExecuteWithErrorHandling(() =>
            client.ExecuteWithErrorHandling<GetReportResponse>(dlRequest)
        );

        if (dlResp.Data.Url == null)
            throw new PluginApplicationException("Report is still being generated, please try again later.");

        using var file = await ExceptionWrapper.ExecuteWithErrorHandling(() => FileDownloader.DownloadFileBytes(dlResp.Data.Url));
        using var memoryStream = new MemoryStream();
        await file.FileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        var jsonDocument = await JsonDocument.ParseAsync(memoryStream);
        var root = jsonDocument.RootElement;

        var translationCostsArr = ParseBreakdownObject(root.GetProperty("translationCosts"));
        var savingsArr = ParseBreakdownObject(root.GetProperty("savings"));
        var weightedArr = ParseBreakdownObject(root.GetProperty("weightedUnits"));
        var preTransArr = ParseBreakdownObject(root.GetProperty("preTranslated"));

        var totalWordsDecimal = root.GetProperty("preTranslated").GetProperty("total").GetDecimal();
        var weightedWordsDecimal = root.GetProperty("weightedUnits").GetProperty("total").GetDecimal();

        var response = new TranslationCostReportResponse
        {
            TotalWords = (int)totalWordsDecimal,
            WeightedWords = weightedWordsDecimal,
            TaskName = root.GetProperty("name").GetString() ?? string.Empty,
            TranslationCost = root.GetProperty("totalCosts").GetDecimal(),
            ProofreadingCost = root.GetProperty("approvalCosts").GetProperty("total").GetDecimal(),
            EstimatedTMSavingsTotal = root.GetProperty("savings").GetProperty("total").GetDecimal(),
            Currency = root.GetProperty("currency").GetString(),

            TranslationCosts = translationCostsArr,
            Savings = savingsArr,
            WeightedUnits = weightedArr,
            PreTranslated = preTransArr
        };

        return response;
    }

    private ColumnValue[] ParseBreakdownObject(JsonElement element)
    {
        var list = new List<ColumnValue>();

        foreach (var prop in element.EnumerateObject())
        {
            if (prop.Value.ValueKind == JsonValueKind.Object)
            {
                foreach (var inner in prop.Value.EnumerateObject())
                {
                    list.Add(new ColumnValue
                    {
                        Name = $"{prop.Name}.{inner.Name}",
                        Value = inner.Value.GetDecimal()
                    });
                }
            }
            else if (prop.Value.ValueKind == JsonValueKind.Number)
            {
                list.Add(new ColumnValue
                {
                    Name = prop.Name,
                    Value = prop.Value.GetDecimal()
                });
            }
        }

        return list.ToArray();
    }

}