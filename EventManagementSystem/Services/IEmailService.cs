using RestSharp;

namespace EventManagementSystem.Services;

public interface IEmailService
{
    //7a7c80deffdb8e5da3277fe56c73e367-ac3d5f74-54fc14a4
    public Task<RestResponse> SendEmail(string email);
}
