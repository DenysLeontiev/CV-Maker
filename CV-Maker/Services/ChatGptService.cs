using CV_Maker.Models;

namespace CV_Maker.Services;

public class ChatGptService
{
    private readonly string apiKey;
    private readonly string endpoint;
    private readonly HttpClient httpClient;

    public ChatGptService(IConfiguration config)
    {
        apiKey = config.GetSection("ApiKey").Value!;
        endpoint = config.GetSection("EndPoint").Value!;

        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    public async Task<string> GetGptResponseAsync(string userInput)
    {
        List<Message> messages = new List<Message>();
        var message = new Message() { Role = "user", Content = userInput };
        messages.Add(message);

        var requestData = new Request()
        {
            ModelId = "gpt-3.5-turbo",
            Messages = messages
        };

        using var response = await httpClient.PostAsJsonAsync(endpoint, requestData);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"{(int)response.StatusCode} {response.StatusCode}");
            return "Error occurred during API request.";
        }

        var responseData = await response.Content.ReadFromJsonAsync<ResponseData>();

        var choices = responseData?.Choices ?? new List<Choice>();
        if (choices.Count == 0)
        {
            return "No choices were returned by the API";
        }

        var choice = choices[0];
        var responseMessage = choice.Message;
        messages.Add(responseMessage);
        var gptResponse = responseMessage.Content.Trim();
        Console.WriteLine($"ChatGPT: {gptResponse}");

        return gptResponse;
    }
}
