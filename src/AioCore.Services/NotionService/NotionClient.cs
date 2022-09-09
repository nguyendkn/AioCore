using AioCore.Services.NotionService.Constants;
using AioCore.Services.NotionService.Responses._Globals;
using AioCore.Services.NotionService.Responses.Components.Properties;
using Newtonsoft.Json;
using RestSharp;

namespace AioCore.Services.NotionService;

public interface INotionClient
{
    Task<List<Dictionary<string, object>>> QueryAsync(string token, string database);

    Task<Page?> GetPageAsync(string? token, string page);
}

public class NotionClient : INotionClient
{
    public async Task<List<Dictionary<string, object>>> QueryAsync(string token, string database)
    {
        var client = new RestClient();
        var dictionaries = new List<Dictionary<string, object>>();
        var request = new RestRequest(new Uri($"{NotionConstants.NotionAPI}/databases/{database}/query"), Method.Post);
        request.AddHeader("Authorization", $"Bearer {token}");
        request.AddHeader("Notion-Version", "2022-02-22");
        request.AddParameter("application/json", string.Empty, ParameterType.RequestBody);
        var response = await client.ExecuteAsync(request);

        if (string.IsNullOrEmpty(response.Content)) return default!;
        var databaseResponse = JsonConvert.DeserializeObject<Database>(response.Content);
        var blocks = databaseResponse?.Results;
        if (blocks is null) return default!;
        foreach (var block in blocks)
        {
            var dictionary = new Dictionary<string, object> { { "Id", block.Id } };
            foreach (var property in block.Properties)
            {
                switch (property.Value)
                {
                    case RichTextProperty richTextProperty:
                        var textContent = richTextProperty.RichText.FirstOrDefault()?.Text.Content;
                        if (textContent != null)
                            dictionary.Add(property.Key, textContent);
                        break;
                    case TitleProperty titleProperty:
                        var content = titleProperty.Title.FirstOrDefault()?.Text.Content;
                        if (content != null)
                            dictionary.Add(property.Key, content);
                        break;
                    case RelationProperty relationProperty:
                        var value = relationProperty.Relation.FirstOrDefault()?.Id;
                        if (value != null)
                            dictionary.Add(property.Key, value);
                        break;
                }
            }

            dictionaries.Add(dictionary);
        }

        return dictionaries;
    }

    public async Task<Page?> GetPageAsync(string? token, string page)
    {
        var client = new RestClient();
        var request = new RestRequest(new Uri($"{NotionConstants.NotionAPI}/blocks/{page}/children"));
        request.AddHeader("Authorization", $"Bearer {token}");
        request.AddHeader("Notion-Version", "2022-02-22");
        request.AddParameter("application/json", string.Empty, ParameterType.RequestBody);
        var response = await client.ExecuteAsync(request);
        return !string.IsNullOrEmpty(response.Content)
            ? JsonConvert.DeserializeObject<Page>(response.Content)
            : default!;
    }
}