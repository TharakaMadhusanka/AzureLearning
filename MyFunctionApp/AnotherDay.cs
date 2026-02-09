using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MyFunctionApp;

public class AnotherDay
{
    private readonly ILogger<AnotherDay> _logger;

    public AnotherDay(ILogger<AnotherDay> logger)
    {
        _logger = logger;
    }

    [Function("AnotherDay")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}