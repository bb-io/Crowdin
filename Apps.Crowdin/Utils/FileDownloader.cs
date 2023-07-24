namespace Apps.Crowdin.Utils;

public static class FileDownloader
{
    public static async Task<byte[]> DownloadFileBytes(string fileUrl)
    {
        using var response = await new HttpClient().GetAsync(fileUrl);

        if (!response.IsSuccessStatusCode)
            throw new($"File download failed; Code: {response.StatusCode}; Link: {fileUrl}");
        
        return await response.Content.ReadAsByteArrayAsync();
    }
}