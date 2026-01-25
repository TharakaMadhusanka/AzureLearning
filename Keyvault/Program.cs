// See https://aka.ms/new-console-template for more information
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

Console.WriteLine("Hello, World!");


string keyVaultUrl = "https://kvtharaka.vault.azure.net/";

DefaultAzureCredentialOptions options = new()
{
    ExcludeEnvironmentCredential = true,
    ExcludeManagedIdentityCredential = true
};

// Create a Secrets Client
//var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential(options));

// Access using user-assigned managed identity

string userAssignedClientId = "efe10dbf-ab51-4f6a-a957-40a11564ba9f";
options.ManagedIdentityClientId = userAssignedClientId;
var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential(options));


// Create a Secret
async Task CreateSecretAsync()
{
    var secret = new KeyVaultSecret("hello", "world");
    await client.SetSecretAsync(secret);
}

async Task ListSecretsAsync()
{
    var secretProps = client.GetPropertiesOfSecretsAsync();

    await foreach (var secretProp in secretProps)
    {
        var sec = await client.GetSecretAsync(secretProp.Name);
        Console.WriteLine($"Secret Name: {sec.Value.Name}, Secret Value: {sec.Value.Value}");
    }
}
await CreateSecretAsync();
await ListSecretsAsync();