// See https://aka.ms/new-console-template for more information
using Microsoft.Azure.Cosmos;

Console.WriteLine("Hello, World!");

var DOCUMENT_ENDPOINT = "https://tharakacosmosdbacct.documents.azure.com:443/";
var ACCOUNT_KEY = "";

var databaseName = "tharakadb";
var container = "employees";

var client = new CosmosClient(accountEndpoint: DOCUMENT_ENDPOINT, authKeyOrResourceToken: ACCOUNT_KEY);

// Create Database if not exist
var dbr = await client.CreateDatabaseIfNotExistsAsync(databaseName);


var db = dbr.Database;

Console.WriteLine($"Db Id {db.Id}");
Console.ReadKey();

var containerInstance = await db.CreateContainerIfNotExistsAsync(new() { Id = container, PartitionKeyPath = "/id" });

ItemRequestOptions options = new ItemRequestOptions
{
    PreTriggers = new List<string> { "AppendFrom" }
};

Product newItem = new Product
{
    id = Guid.NewGuid().ToString(), // Generate a unique ID for the product
    name = "Sample Item",
    description = "This is a sample item in my Azure Cosmos DB exercise."
};

ItemResponse<Product> itemResponse = await containerInstance.Container.CreateItemAsync(newItem, requestOptions: options);

Console.WriteLine($"Created item with ID: {itemResponse.Resource.id}");
Console.WriteLine($"Request charge: {itemResponse.RequestCharge} RUs");

// Execute Stored Procedure
var key = Guid.NewGuid().ToString();
await dbr.Database.GetContainer(container).Scripts.ExecuteStoredProcedureAsync<string>(
    "CreateMyDocument",
    new PartitionKey(key),
    [key],
    new()
);

// To Note
// To execute Triggers on Action, it must be attached

public class Product
{
    public string? id { get; set; }
    public string? name { get; set; }
    public string? description { get; set; }
}



