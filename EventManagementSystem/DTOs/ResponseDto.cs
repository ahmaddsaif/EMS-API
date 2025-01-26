namespace EventManagementSystem.DTOs;

public class ResponseDto<T>
{
    public required Status Status { get; set; }
    public required T Data { get; set; }
}

public class Status
{
    public required int Code { get; set; }
    public required string Message { get; set; }
}
