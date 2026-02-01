// See https://aka.ms/new-console-template for more information
using Azure.Messaging.EventGrid;

Console.WriteLine("Hello, World!");

var accessKey = "";
var topicUrl = "https://mytopic-evgtopic-4028.southeastasia-1.eventgrid.azure.net/api/events";


async Task ProcessAsync()
{
    try
    {
        var eventPublisher = new EventGridPublisherClient(new Uri(topicUrl), new Azure.AzureKeyCredential(accessKey));

        var eventGridEvent = new EventGridEvent(
                subject: "ExampleSubject",
                eventType: "ExampleEventType",
                dataVersion: "1.0",
                data: new { Message = "Hello, Event Grid!" }
            );
        await eventPublisher.SendEventAsync(eventGridEvent);
    }
    catch (Exception e) { }
}

// Start the process to send an Event Grid Event
await ProcessAsync();


