// See https://aka.ms/new-console-template for more information
using Azure.Storage.Queues.Models;

Console.WriteLine("Hello, World!");

// Create a queue client
var queueClient = new Azure.Storage.Queues.QueueClient(
    new Uri("https://tharakast.queue.core.windows.net/tharakaqueue"),
    new Azure.Identity.DefaultAzureCredential());

// Create the queue if it doesn't already exist
await queueClient.CreateIfNotExistsAsync();

if (queueClient.Exists())
{
    queueClient.SendMessage("Hello, Azure Queue Storage!");
}

// Peek Message -- Just looks at the message without removing it from the queue
await queueClient.PeekMessageAsync().ContinueWith(peekedMessageTask =>
{
    var peekedMessage = peekedMessageTask.Result;
    Console.WriteLine($"Peeked message: {peekedMessage.Value.MessageText}");
});

// Receive Message -- Retrieves and removes the message from the queue
await queueClient.ReceiveMessageAsync().ContinueWith(receivedMessageTask =>
{
    var receivedMessage = receivedMessageTask.Result;
    Console.WriteLine($"Received message: {receivedMessage.Value.MessageText}");
});

// change the content of message
await queueClient.ReceiveMessageAsync().ContinueWith(async receivedMessageTask =>
{
    var receivedMessage = receivedMessageTask.Result;
    string updatedMessageText = "Updated message content";
    await queueClient.UpdateMessageAsync(
        receivedMessage.Value.MessageId,
        receivedMessage.Value.PopReceipt,
        updatedMessageText,
        TimeSpan.FromMinutes(5));
    Console.WriteLine($"Updated message to: {updatedMessageText}");
});

// Get Queue Properties
if (queueClient.Exists())
{
    QueueProperties properties = queueClient.GetProperties();

    // Retrieve the cached approximate message count.
    int cachedMessagesCount = properties.ApproximateMessagesCount;

    // Display number of messages.
    Console.WriteLine($"Number of messages in queue: {cachedMessagesCount}");
}

if (queueClient.Exists())
{
    // Get the next message
    QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();

    // Process (i.e. print) the message in less than 30 seconds
    Console.WriteLine($"Dequeued message: '{retrievedMessage[0].Body}'");

    // Delete the message
    queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
}