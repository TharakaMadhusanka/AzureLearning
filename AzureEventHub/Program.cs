// See https://aka.ms/new-console-template for more information
using Azure.Identity;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Producer;
using System.Text;

Console.WriteLine("Hello, World!");

var namespaceURL = "myegspace.servicebus.windows.net";
var eventHubName = "myeventhub";
int numOfEvents = 3;

DefaultAzureCredentialOptions options = new()
{
    ExcludeEnvironmentCredential = true,
    ExcludeManagedIdentityCredential = true
};

await using var producer = new EventHubProducerClient(namespaceURL, eventHubName, new DefaultAzureCredential(options));




// Create a producer client to send events to the event hub
EventHubProducerClient producerClient = new EventHubProducerClient(
    namespaceURL,
    eventHubName,
    new DefaultAzureCredential(options));

// Create a batch of events 
using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();


// Adding a random number to the event body and sending the events. 
var random = new Random();
for (int i = 1; i <= numOfEvents; i++)
{
    int randomNumber = random.Next(1, 101); // 1 to 100 inclusive
    string eventBody = $"Event {randomNumber}";
    if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(eventBody))))
    {
        // if it is too large for the batch
        throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
    }
}

try
{
    // Use the producer client to send the batch of events to the event hub
    await producerClient.SendAsync(eventBatch);

    Console.WriteLine($"A batch of {numOfEvents} events has been published.");
    Console.WriteLine("Press Enter to retrieve and print the events...");
    Console.ReadLine();
}
finally
{
    await producerClient.DisposeAsync();
}

// Consume using EventHubConsumerClient || This is not recommended, rather recommend to use EventProcessorClient for production applications
string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;



await using (var consumer = new EventHubConsumerClient(consumerGroup, namespaceURL, eventHubName, new DefaultAzureCredential(options)))
{
    long totalEvents = 0;

    string[] partitionIds = await producer.GetPartitionIdsAsync();

    foreach (string partitionId in partitionIds)
    {
        PartitionProperties properties = await consumer.GetPartitionPropertiesAsync(partitionId);
        if (!properties.IsEmpty && properties.LastEnqueuedSequenceNumber >= properties.BeginningSequenceNumber)
        {
            totalEvents += (properties.LastEnqueuedSequenceNumber - properties.BeginningSequenceNumber + 1);
        }

    }

    using var cancellationSource = new CancellationTokenSource();
    cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));


    int retrievedCount = 0;
    //await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(startReadingAtEarliestEvent: true))
    //{
    //    if (partitionEvent.Data != null)
    //    {
    //        string body = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());
    //        Console.WriteLine($"Retrieved event: {body}");
    //        retrievedCount++;
    //        if (retrievedCount >= totalEvents)
    //        {
    //            Console.WriteLine("Done retrieving events. Press Enter to exit...");
    //            Console.ReadLine();
    //        }
    //    }
    //}

    var partitionReader = consumer.ReadEventsFromPartitionAsync(partitionId: "0", EventPosition.Earliest);
    if (partitionReader is not null)
    {
        var x = await partitionReader.ToListAsync();

        foreach (var partitionEvent in x)
        {
            if (partitionEvent.Data != null)
            {
                string body = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());
                Console.WriteLine($"Retrieved event from partition 0: {body}");
            }
        }
    }
}

// Read events from Event Hub partitions


//await using (var consumer = new EventHubConsumerClient(consumerGroup, namespaceURL, eventHubName))
//{
//    EventPosition startingPosition = EventPosition.Earliest;
//    string partitionId = (await consumer.GetPartitionIdsAsync()).First();

//    using var cancellationSource = new CancellationTokenSource();
//    cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));

//    await foreach (PartitionEvent receivedEvent in consumer.ReadEventsFromPartitionAsync(partitionId, startingPosition, cancellationSource.Token))
//    {
//        // At this point, the loop will wait for events to be available in the partition. When an event
//        // is available, the loop will iterate with the event that was received. Because we did not
//        // specify a maximum wait time, the loop will wait forever unless cancellation is requested using
//        // the cancellation token.
//    }
//}