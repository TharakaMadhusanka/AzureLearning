using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CosmosDbChangeFeedProcessor;

public class ChangeFeedProcessor
{
    private readonly ILogger<ChangeFeedProcessor> _logger;

    public ChangeFeedProcessor(ILogger<ChangeFeedProcessor> logger)
    {
        _logger = logger;
    }

    [Function("ChangeFeedProcessor")]
    public void Run([CosmosDBTrigger(
        databaseName: "tharakadb",
        containerName: "employees",
        Connection = "CosmosDb",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<MyDocument> input)
    {
        if (input != null && input.Count > 0)
        {
            _logger.LogInformation("Documents modified: " + input.Count);
            _logger.LogInformation("First document Id: " + input[0].id);
        }
    }
}

public class MyDocument
{
    public string id { get; set; }

    public string Text { get; set; }

    public int Number { get; set; }

    public bool Boolean { get; set; }
}