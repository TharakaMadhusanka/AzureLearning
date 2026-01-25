// See https://aka.ms/new-console-template for more information
using Microsoft.Identity.Client;

Console.WriteLine("Hello, World!");


var clientId = "321c36db-3d2c-48de-acb3-2576fc5e0ab6";
var tenantId = "51672326-aa4f-474d-90b1-3beb33ec85ac";

string[] _scopes = { "User.Read" };

var app = PublicClientApplicationBuilder
    .Create(clientId)
    .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
    .WithDefaultRedirectUri()
    .Build();


// Attempt to acquire an access token silently or interactively
AuthenticationResult result;
try
{
    // Try to acquire token silently from cache for the first available account
    var accounts = await app.GetAccountsAsync();
    result = await app.AcquireTokenSilent(_scopes, accounts.FirstOrDefault())
                .ExecuteAsync();
}
catch (MsalUiRequiredException)
{
    // If silent token acquisition fails, prompt the user interactively
    result = await app.AcquireTokenInteractive(_scopes)
                .ExecuteAsync();
}

// Output the acquired access token to the console
Console.WriteLine($"Access Token:\n{result.AccessToken}");


