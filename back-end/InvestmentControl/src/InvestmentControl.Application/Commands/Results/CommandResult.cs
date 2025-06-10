namespace InvestmentControl.Application.Commands.Results;

public class CommandResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object? Data { get; set; }

    public CommandResult(bool success, string message, object? data = null)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public static CommandResult Ok(string message, object? data = null) => new(true, message, data);
    public static CommandResult Fail(string message) => new(false, message);
}
