using System.Text.Json;
using AioCore.Notion.Constants;
using AioCore.Notion.Responses._Globals;
using Newtonsoft.Json;
using RestSharp;

namespace AioCore.Notion;

public interface INotionClient
{
    Task<Database> QueryAsync<T>(string database);

    Task<Page> GetPageAsync(string page);
}

public class NotionClient : INotionClient
{
    private readonly NotionOptions _options;

    public NotionClient(NotionOptions options)
    {
        _options = options;
    }

    public async Task<Database> QueryAsync<T>(string database)
    {
        var client = new RestClient();
        var request = new RestRequest(new Uri($"{NotionConstants.NotionAPI}/databases/{database}/query"), Method.Post);
        request.AddHeader("Authorization", $"Bearer {_options.Token}");
        request.AddHeader("Notion-Version", _options.Version ?? NotionConstants.NotionVersion);
        request.AddParameter("application/json", string.Empty, ParameterType.RequestBody);
        var response = await client.ExecuteAsync<Database>(request);
        return response.Data;
    }

    public async Task<Page> GetPageAsync(string page)
    {
        var client = new RestClient();
        var request = new RestRequest(new Uri($"{NotionConstants.NotionAPI}/blocks/{page}/children"));
        request.AddHeader("Authorization", $"Bearer {_options.Token}");
        request.AddHeader("Notion-Version", _options.Version ?? NotionConstants.NotionVersion);
        request.AddParameter("application/json", string.Empty, ParameterType.RequestBody);
        var response = await client.ExecuteAsync(request);
        return !string.IsNullOrEmpty(response.Content) ? JsonConvert.DeserializeObject<Page>(response.Content) : default!;
    }
}