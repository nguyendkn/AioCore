using AioCore.Notion.Constants;
using AioCore.Notion.Responses._Globals;
using AioCore.Notion.Responses.Components.Properties;
using Newtonsoft.Json;
using RestSharp;

namespace AioCore.Notion;

public interface INotionClient
{
    Task<List<Dictionary<string, object>>> QueryAsync(string token, string database);

    Task<Page> GetPageAsync(string token, string page);
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
                        dictionary.Add(property.Key, richTextProperty.RichText.FirstOrDefault()?.Text.Content);
                        break;
                    case TitleProperty titleProperty:
                        dictionary.Add(property.Key, titleProperty.Title.FirstOrDefault()?.Text.Content);
                        break;
                    case RelationProperty relationProperty:
                        dictionary.Add(property.Key, relationProperty.Relation.FirstOrDefault()?.Id);
                        break;
                }
            }

            dictionaries.Add(dictionary);
        }

        return dictionaries;
    }

    public async Task<Page> GetPageAsync(string token, string page)
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