using StrongMindExercise.Application.Errors;
using System.Text;
using System.Text.Json;

namespace StrongMindExercise.WebUI.HelperMethods;

public static class HttpHelperMethods
{
    public static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public static async Task<T> GetAsync<T>(string url)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _options);
    }

    public static async Task<Error> PostAsync<T>(string url, T data)
    {
        using var client = new HttpClient();
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        
        return await DeserializeError(response);
    }

    public static async Task<Error> PutAsync<T>(string url, T data)
    {
        using var client = new HttpClient();
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PutAsync(url, content);

        return await DeserializeError(response);
    }

    public static async Task<Error> DeleteAsync(string url)
    {
        using var client = new HttpClient();
        var response = await client.DeleteAsync(url);

        return await DeserializeError(response);
    }

    private static async Task<Error> DeserializeError(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return Error.None;
        }

        return JsonSerializer.Deserialize<Error>(await response.Content.ReadAsStringAsync(), _options);
    }
}
