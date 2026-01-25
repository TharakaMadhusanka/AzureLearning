// See https://aka.ms/new-console-template for more information
using Azure.Identity;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Hello, World!");

var credoptions = new DefaultAzureCredentialOptions()
{
    ExcludeEnvironmentCredential = true,
    ExcludeManagedIdentityCredential = true
};

var appconfigEndpoint = "https://tharakaappconfig.azconfig.io";

var builder = new ConfigurationBuilder();

builder.AddAzureAppConfiguration(options =>
{

    options.Connect(new Uri(appconfigEndpoint), new DefaultAzureCredential(credoptions));
});

// Build the final configuration object
try
{
    var config = builder.Build();

    // Retrieve a configuration value by key name
    Console.WriteLine(config["Dev:conStr"]);
}
catch (Exception ex)
{
    Console.WriteLine($"Error connecting to Azure App Configuration: {ex.Message}");
}
