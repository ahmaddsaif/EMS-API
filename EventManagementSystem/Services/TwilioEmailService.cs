
using RestSharp;
using RestSharp.Authenticators;

namespace EventManagementSystem.Services;

public class TwilioEmailService : IEmailService
{
    public async Task<RestResponse> SendEmail(string email)
    {
        var options = new RestClientOptions("https://api.mailgun.net/v3")
        {
            Authenticator = new HttpBasicAuthenticator("api", Environment.GetEnvironmentVariable("API_KEY") ?? "7a7c80deffdb8e5da3277fe56c73e367-ac3d5f74-54fc14a4")
        };

        var client = new RestClient(options);
        var request = new RestRequest("/sandboxd5f078af189649498f156e7bf025188f.mailgun.org/messages", Method.Post);
        request.AlwaysMultipartFormData = true;
        request.AddParameter("from", "Mailgun Sandbox <postmaster@sandboxd5f078af189649498f156e7bf025188f.mailgun.org>");
        request.AddParameter("to", email);
        request.AddParameter("subject", "Hello Saif Ahmad");
        request.AddParameter("text", "Congratulations Saif Ahmad, you just sent an email with Mailgun! You are truly awesome!");
        return await client.ExecuteAsync(request);
    }
}
