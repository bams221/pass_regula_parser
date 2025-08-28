using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PassRegulaParser.Models;

namespace PassRegulaParser.Api;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "http://localhost:5000/add_passport";

    public ApiClient()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<bool> SendPassportDataAsync(PassportData passportData)
    {
        try
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(passportData, options);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(ApiUrl, content);

            Console.WriteLine($"Data sent. Response status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode}, Details: {errorContent}");
                return false;
            }
            return true;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP request error: {ex.Message}");
            throw;
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"Request timeout: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw;
        }
    }
}