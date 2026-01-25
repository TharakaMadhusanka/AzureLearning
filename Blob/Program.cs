// See https://aka.ms/new-console-template for more information
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

Console.WriteLine("Hello, World!");


// Access Blob
static BlobClient AccessBlob()
{
    var options = new BlobClientOptions();
    options.Retry.MaxRetries = 5;
    // Code to access Azure Blob Storage
    // Option 1 - Using DefaultAzureCredential
    BlobClient blobClient = new(
            new Uri("https://sttharaka.blob.core.windows.net/mycontainer/New Text Document.txt"),
            new DefaultAzureCredential(),
            options
        );

    // Option 2 - Using Connection String
    //BlobClient blobClient = new BlobClient(connectionstring,
    return blobClient;
}

var blobClient = AccessBlob();
Console.WriteLine($"Blob Client Uri: {blobClient.Uri}");


var props = blobClient.GetProperties();
Console.WriteLine($"Blob Size: {props.Value.ContentLength} bytes");

static BlobContainerClient AccessContainer()
{
    BlobContainerClient containerClient = new(
            new Uri("https://sttharaka.blob.core.windows.net/mycontainer"),
            new DefaultAzureCredential()
        );
    return containerClient;
}

var containerClient = AccessContainer();
var blob = containerClient.GetBlobClient("New Text Document.txt");



blob.SetMetadata(new Dictionary<string, string>
{
    { "owner", "sttharaka" },
    { "project", "BlobDemo" }
});


props = blob.GetProperties();
Console.WriteLine($"Blob Size: {props.Value.ContentLength} bytes");

// Create Blob Policy
BlobSignedIdentifier identifier = new BlobSignedIdentifier
{
    Id = "stored access policy identifier",
    AccessPolicy = new BlobAccessPolicy
    {
        ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
        Permissions = "rw"
    }
};

containerClient.SetAccessPolicy(permissions: new BlobSignedIdentifier[] { identifier });