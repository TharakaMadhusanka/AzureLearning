**Azure Cosmos DB**

- multi-master replication protocol
  - enables
    - Unlimited elastic write and read scalability.
    - 99.999% read and write availability all around the world.
    - Guaranteed reads and writes served in less than 10 milliseconds at the 99th percentile.
- offers 99.999% read and write availability for multi-region databases.
- The Azure Cosmos DB account
  - the fundamental unit of global distribution and high availability.
  - contains a unique Domain Name System (DNS) name
  - you can add and remove Azure regions to your account at any time.
- Azure Cosmos DB container
  - the fundamental unit of scalability.
  - You can virtually have an unlimited provisioned throughput (RU/s) and storage on a container.
  - Data is stored on one or more servers called partitions.
  - Partition Key
    - When you create a container, you need to supply a partition key.
    - is a property that you select from your items to help Azure Cosmos DB distribute the data efficiently across partitions.
  - Azure Cosmos DB transparently partitions your container using the logical partition key that you specify in order to elastically scale your provisioned throughput and storage.
- maximum of 50 Azure Cosmos DB accounts under an Azure subscription.

[Important]

- The Usage of Partition Key
  - Az cosmos db use this to route data to the appropriate partition to be written, updated or deleted.
  - can use this key in WHERE clause in queries for faster data retrieval.

**the hierarchy of different entities in an Azure Cosmos DB account**
![alt text](image.png)

- The underlying storage mechanism for data in Azure Cosmos DB is called a **physical partition.**
- Physical partitions can have a throughput amount up to 10,000 Request Units per second, and they can store up to 50 GB of data.
- Azure Cosmos DB abstracts this partitioning concept with a logical partition, which can store up to 20 GB of data.
- When data or throughput grows:
  - Cosmos DB splits physical partitions automatically
  - Logical partitions never split

- All items with the same partition key:
  - Go to the same logical partition
  - Are guaranteed to be colocated

- Limits per logical partition
  - Up to 20 GB of data
  - No fixed RU/s limit (shares RU/s of physical partition)

- When you create a container, you configure throughput in one of the following modes:

1. Dedicated throughput: The throughput on a container is exclusively reserved for that container. There are two types of dedicated throughput: standard and autoscale.

2. Shared throughput: Throughput is specified at the database level and then shared with up to 25 containers within the database. Sharing of throughput excludes containers that are configured with their own dedicated throughput.

[Important]

- Azure Cosmos DB, Data Consistency Spectrum
  ![Consistency Spectrum](image-1.png)

- The consistency levels are region-agnostic and are guaranteed for all operations, regardless of:

1. The region where the reads and writes are served
2. The number of regions associated with your Azure Cosmos DB account
3. Whether your account is configured with a single or multiple write regions.

_Read consistency applies to a single read operation scoped within a partition-key range or a logical partition._

- Consistency levels define the guarantees for reads after writes have been acknowledged.
  - Data Consosistency Levels -
    - Strong
    - Bounded Staleness
    - Session
    - Consistent Prefix
    - Eventual Consistency

- These define: What version of data a read operation is allowed to see.
- Consistency guarantees are strongest and most meaningful: Within a single logical partition.
- Because:
  - Transactions only work inside one logical partition
  - Writes for a logical partition are ordered

If you:

✅ Do a point read
`ReadItemAsync(id, partitionKey)`

It targets one logical partition
Consistency works exactly as defined

⚠ But if you run a cross-partition query:
`SELECT * FROM c`

Across many partition keys:
Each partition may return slightly different freshness
Especially under Eventual or Session consistency

✅ Consistency level is configured at the Cosmos **DB account level**

NOT at:
❌ Database level
❌ Container level

| Consistency           | Read Guarantee                                      | Write Behavior                                                                  |
| --------------------- | --------------------------------------------------- | ------------------------------------------------------------------------------- |
| **Strong**            | Always returns latest committed write               | Write must complete in primary region → higher latency                          |
| **Bounded Staleness** | Reads may lag by a fixed number of versions or time | Writes still committed immediately; reads may be slightly behind                |
| **Session**           | Read-your-own-writes within the same session        | Writes are immediately visible to the same client session; others may see later |
| **Consistent Prefix** | Reads preserve order of writes                      | Writes are ordered, reads may skip recent writes                                |
| **Eventual**          | Reads eventually reflect latest writes              | Writes are immediately committed; reads may lag arbitrarily                     |

---

| Consistency Level       | Guarantees                        | Latency  | Throughput | When to Use                    |
| ----------------------- | --------------------------------- | -------- | ---------- | ------------------------------ |
| **Strong**              | Linearizable reads (latest write) | Highest  | Lowest     | Financial systems              |
| **Bounded Staleness**   | Reads lag by time or versions     | High     | Medium     | Global apps with strict limits |
| **Session** _(default)_ | Read-your-own-writes              | Low      | High       | User-centric apps              |
| **Consistent Prefix**   | Order preserved, may lag          | Very low | Very high  | Logs, feeds                    |
| **Eventual**            | Eventually consistent             | Lowest   | Highest    | Analytics, metrics             |

- Detailed explanation

1️⃣ Strong Consistency

- What it guarantees
  - Reads always return latest committed write
  - Like single-master SQL

  Trade-offs

  ❌ Higher latency
  ❌ Lower availability across regions

  🔒 Guarantee

Always read the most recent committed write.
No stale reads. Global linearizability.

✅ Real-World Use Cases
💳 1. Banking Transactions
Account balance updates
Money transfers
Payment settlement

Why?
If user transfers $500:
They MUST see updated balance immediately.
No stale reads allowed.

Example

💰 Bank account balance
User transfers $100 → immediate read must show updated balance

📌 Only supported within single region or closely paired regions

⚠ Trade-Off

- Higher latency
- Reduced availability (in multi-region)
- Higher RU cost

2️⃣ Bounded Staleness

- Bounded Staleness is beneficial primarily to single-region write accounts with two or more regions.
- If the data lag in a region (determined per physical partition) exceeds the configured staleness value, writes for that partition are throttled until staleness is back within the configured upper bound.

⏳ Guarantee

Reads may lag behind writes by:
X versions OR
Y time interval

But in a controlled way.

- What it guarantees
  - Reads can lag by:
    - Time (e.g. 5 seconds)
    - Versions (e.g. 100 updates)

  Global e-commerce stock
  - Stock count can be 2–3 seconds behind

📌 Strong ordering with predictable lag

- Global strong consistency in multi-region can reduce availability.

3️⃣ Session Consistency (DEFAULT)

- What it guarantees
  - Read-your-own-writes
    - Users always see their own updates

  User updates name → refresh → sees updated name
  Other users may see it later
  📌 Best balance of:
  - Performance
  - Cost
  - UX

4️⃣ Consistent Prefix

- What it guarantees
  - Order of writes is preserved
  - May miss recent writes

  Activity feed
  Post A → Post B → Post C
  You will never see C before A or B

📌 No time/version guarantee

5️⃣ Eventual Consistency

- What it guarantees
  - Data eventually converges
  - No ordering guarantee

Metrics can arrive late or out of order
📌 Highest availability & throughput

_Session consistency is the most widely used consistency level for single-region and globally distributed applications._

| Feature              | Critical? | Consistency       |
| -------------------- | --------- | ----------------- |
| Payment capture      | Very high | Strong            |
| Inventory            | High      | Strong / Bounded  |
| Shopping cart        | Medium    | Session           |
| Order status feed    | Medium    | Consistent Prefix |
| Product rating count | Low       | Eventual          |

Ask:

"If the user reads slightly stale data, what is the worst that can happen?"

If answer is:

💰 Money loss → Strong
😡 User confusion → Session
😐 Minor inaccuracy → Eventual

- Azure Cosmos DB guarantees that 100 percent of read requests meet the consistency guarantee for the consistency level chosen.

- Writes are replicated to a minimum of three replicas in the local region, with asynchronous replication to all other regions.

- Azure Cosmos DB offers multiple database APIs, which include NoSQL, MongoDB, PostgreSQL, Cassandra, Gremlin, and Table.

**Request Units**

- With Azure Cosmos DB, you pay for the throughput you provision and the storage you consume on an hourly basis. [Important]
- Throughput must be provisioned to ensure that sufficient system resources are available for your Azure Cosmos database always.
- The cost of all database operations is normalized in Azure Cosmos DB and expressed by request units (or RUs, for short).
- A request unit represents the system resources such as CPU, IOPS, and memory that are required to perform the database operations supported by Azure Cosmos DB.

![RU Calculcation](image-2.png)

- The type of Azure Cosmos DB account you're using determines the way consumed RUs get charged.
- There are two modes for account creation:
  - Provisioned throughput mode: In this mode, you provision the number of RUs for your application on a per-second basis in increments of 100 RUs per second.
  - Serverless mode: In this mode, you don't have to provision any throughput when creating resources in your Azure Cosmos DB account.

**Azure Cosmos DB Stored Procedures**

- Stored procedures are registered per collection, and can operate on any document or an attachment present in that collection.
- Stored Procedeures are written in Javascript.

`var helloWorldStoredProc = {
id: "helloWorld",
serverScript: function () {
var context = getContext();
var response = context.getResponse();

        response.setBody("Hello, World");
    }

}`

- The context object provides access to all operations that can be performed in Azure Cosmos DB, and access to the request and response objects. [Important]
- When defining a stored procedure in the Azure portal, input parameters are always sent as a string to the stored procedure.

1. Create SP

- Creating an item is an asynchronous operation and depends on the JavaScript callback functions. The callback function has two parameters:
  - one for the error object in case the operation fails
  - another for a return value

- The stored procedure also includes a parameter to set the description as a boolean value. When the parameter is set to true and the description is missing, the stored procedure throws an exception.
- When defining a stored procedure in the Azure portal, input parameters are always sent as a string to the stored procedure.

`var createDocumentStoredProc = {
    id: "createMyDocument",
    body: function createMyDocument(documentToCreate) {
        var context = getContext();
        var collection = context.getCollection();
        var accepted = collection.createDocument(collection.getSelfLink(),
              documentToCreate,
              function (err, documentCreated) {
                  if (err) throw new Error('Error' + err.message);
                  context.getResponse().setBody(documentCreated.id)
              });
        if (!accepted) return;
    }
}`

- All Azure Cosmos DB operations must complete within a limited amount of time. Stored procedures have a limited amount of time to run on the server.
- All collection functions return a Boolean value that represents whether that operation completes or not
- JavaScript functions can implement a continuation-based model to batch or resume execution.

**Javascript Continuation Model**

- The JavaScript continuation model in Azure Cosmos DB is about how server-side JavaScript (stored procedures, triggers, UDFs) handles large data sets and long-running operations within Cosmos DB’s execution limits.
- Applies to:
  - Stored procedures (most common in exams)
  - Triggers (less common)
  - UDFs (read-only, no continuation needed)

  ✔ Continuation model applies to stored procedures
  ✔ Used when operations exceed RU or time limits
  ✔ Client must re-invoke stored procedure
  ❌ Stored procedures cannot span partitions
  ❌ No background execution

🎯 When to use continuation model

    - Bulk inserts
    - Batch updates
    - Data cleanup jobs
    - Migration tasks

![alt text](image-3.png)

**Azure CosmosDB Triggers**

- 2 Types
  - Pre-Trigger
  - Post-Trigger
- [Important] The post-trigger runs as part of the same transaction for the underlying item itself. An exception during the post-trigger execution fails the whole transaction. Anything committed is rolled back and an exception returned.

**Change feed**

- Change feed in Azure Cosmos DB is a persistent record of changes to a container in the order they occur.
- Change feed support in Azure Cosmos DB works by listening to an Azure Cosmos DB container for any changes.
- It then outputs the _sorted_ list of documents that were changed in the order in which they were modified.
- The persisted changes can be processed asynchronously and incrementally, and the output can be distributed across one or more consumers for parallel processing.
- You can't filter the change feed for a specific type of operation.
- Currently change feed doesn't log delete operations.

- Two ways you can read from the change feed, push model or pull model.

**Reading with Push Model**

- two ways you can read from the change feed with a push model:
  - Azure Functions Azure Cosmos DB triggers
  - the change feed processor library.

- The change feed processor is part of the Azure Cosmos DB .NET V3 & Java V4 SDKs.
  - ## There are four main components of implementing the change feed processor:
    - Monitored Container [Source Container]
      - The monitored container has the data from which the change feed is generated.
      - Any inserts and updates to the monitored container are reflected in the change feed of the container.
    - Lease Container
      - The lease container acts as a state storage and coordinates processing the change feed across multiple workers.
      - The lease container can be stored in the same account as the monitored container or in a separate account.
    - The compute instance
      - A compute instance hosts the change feed processor to listen for changes.
      - Depending on the platform, it might be represented by a VM, a kubernetes pod, an Azure App Service instance, an actual physical machine. It has a unique identifier referenced as the instance name throughout this article.
  - The delegate:
    - The delegate is the code that defines what you, the developer, want to do with each batch of changes that the change feed processor reads.
    1. Source container – where data changes occur
    1. Lease container – stores checkpoints & partition ownership
    1. Compute host – Function / Worker / Web App
    1. Processor logic – your code that handles changes

✔ Change Feed is pull-based
✔ Requires lease container
✔ Supports exactly-once processing per partition
❌ Does NOT trigger on deletes (soft deletes only)
❌ Does NOT cross containers

## Implementing Change Feed Processor

- When implementing the change feed processor the point of entry is always the monitored container, from a Container instance you call GetChangeFeedProcessorBuilder:

`/// <summary>
/// Start the Change Feed Processor to listen for changes and process them with the HandleChangesAsync implementation.
/// </summary>
private static async Task<ChangeFeedProcessor> StartChangeFeedProcessorAsync(
CosmosClient cosmosClient,
IConfiguration configuration)
{
string databaseName = configuration["SourceDatabaseName"];
string sourceContainerName = configuration["SourceContainerName"];
string leaseContainerName = configuration["LeasesContainerName"];

    Container leaseContainer = cosmosClient.GetContainer(databaseName, leaseContainerName);
    ChangeFeedProcessor changeFeedProcessor = cosmosClient.GetContainer(databaseName, sourceContainerName)
        .GetChangeFeedProcessorBuilder<ToDoItem>(processorName: "changeFeedSample", onChangesDelegate: HandleChangesAsync)
            .WithInstanceName("consoleHost")
            .WithLeaseContainer(leaseContainer)
            .Build();

    Console.WriteLine("Starting Change Feed Processor...");
    await changeFeedProcessor.StartAsync();
    Console.WriteLine("Change Feed Processor started.");
    return changeFeedProcessor;

}`

- Where the first parameter is a distinct name that describes the goal of this processor and the second parameter is the delegate implementation that handles changes. Following is an example of a delegate:

`/// <summary>
/// The delegate receives batches of changes as they are generated in the change feed and can process them.
/// </summary>
static async Task HandleChangesAsync(
ChangeFeedProcessorContext context,
IReadOnlyCollection<ToDoItem> changes,
CancellationToken cancellationToken)
{
Console.WriteLine($"Started handling changes for lease {context.LeaseToken}...");
    Console.WriteLine($"Change Feed request consumed {context.Headers.RequestCharge} RU.");
// SessionToken if needed to enforce Session consistency on another client instance
Console.WriteLine($"SessionToken ${context.Headers.Session}");

    // We may want to track any operation's Diagnostics that took longer than some threshold
    if (context.Diagnostics.GetClientElapsedTime() > TimeSpan.FromSeconds(1))
    {
        Console.WriteLine($"Change Feed request took longer than expected. Diagnostics:" + context.Diagnostics.ToString());
    }

    foreach (ToDoItem item in changes)
    {
        Console.WriteLine($"Detected operation for item with id {item.id}, created at {item.creationTime}.");
        // Simulate some asynchronous operation
        await Task.Delay(10);
    }

    Console.WriteLine("Finished handling changes.");

}`

- Afterwards, you define the compute instance name or unique identifier with `WithInstanceName`, this should be unique and different in each compute instance you're deploying, and finally, which is the container to maintain the lease state with `WithLeaseContainer`

- The normal life cycle of a host instance is:
  1. Read the change feed.
  2. If there are no changes, sleep for a predefined amount of time (customizable with WithPollInterval in the Builder) and go to #1.
  3. If there are changes, send them to the delegate.
  4. When the delegate finishes processing the changes successfully, update the lease store with the latest processed point in time and go to #1.

-- Use of .NET SDK v3 ==> Microsoft.Azure.Cosmos

1. Create Client

`CosmosClient client = new CosmosClient(endpoint, key);`

2. Create Database

- The CosmosClient.CreateDatabaseAsync method throws an exception if a database with the same name already exists.

`// New instance of Database class referencing the server-side database
Database database1 = await client.CreateDatabaseAsync(
    id: "adventureworks-1"
);`

// Check DB Exist and unless create
`// New instance of Database class referencing the server-side database
Database database2 = await client.CreateDatabaseIfNotExistsAsync(
    id: "adventureworks-2"
);`

3. Read Database by Id

`// Reads a Database resource with the ID property of the Database resource you wish to read.
Database database = this.cosmosClient.GetDatabase(database_id);
DatabaseResponse response = await database.ReadAsync();`

4. Delete Database

`await database.DeleteAsync();`

5. Create a Container

`// Set throughput to the minimum value of 400 RU/s
ContainerResponse simpleContainer = await database.CreateContainerIfNotExistsAsync(
    id: containerId,
    partitionKeyPath: partitionKey,
    throughput: 400);`

6. Get Container by Id

`Container container = database.GetContainer(containerId);
ContainerProperties containerProperties = await container.ReadContainerAsync();`

7. Delete a Container

`await database.GetContainer(containerId).DeleteContainerAsync();`

8. Create Item

`ItemResponse<SalesOrder> response = await container.CreateItemAsync(salesOrder, new PartitionKey(salesOrder.AccountNumber));`

9. Read an Item

`string id = "[id]";
string accountNumber = "[partition-key]";
ItemResponse<SalesOrder> response = await container.ReadItemAsync(id, new PartitionKey(accountNumber));`

10. query an Item

`QueryDefinition query = new QueryDefinition(
"select \* from sales s where s.AccountNumber = @AccountInput ")
.WithParameter("@AccountInput", "Account1");

FeedIterator<SalesOrder> resultSet = container.GetItemQueryIterator<SalesOrder>(
query,
requestOptions: new QueryRequestOptions()
{
PartitionKey = new PartitionKey("Account1"),
MaxItemCount = 1
});`

### Cosmos DB Scale-Out on the load, instead Scale Up

- Data is stored on one or more servers called partitions.

# Partition Key

- A partition key is a property used to group related data together.
- It determines how data is distributed and stored.
  Used in services like:
  - Azure Cosmos DB
  - Azure Table Storage
  - Service Bus (Sessions conceptually similar)

- Why Partition Key is Important
  ✅ Enables horizontal scaling
  ✅ Improves query performance
  ✅ Reduces cross-partition queries
  ✅ Controls throughput distribution
  ✅ Affects cost (RU consumption in Cosmos DB)

- In Azure Cosmos DB
  - Each container must have a partition key defined at creation time
  - Data with the same partition key value:
    - Stored in the same logical partition
    - Efficiently queried together
    - Each logical partition:
      - Max 20 GB storage
      - Max 10,000 RU/s throughput

🔹 Good Partition Key Characteristics
Choose a key that:
✔ Has high cardinality (many unique values)
✔ Evenly distributes data
✔ Matches common query patterns
✔ Avoids “hot partitions”

🔹 Bad Partition Key Example
❌ Country
If most users are from one country → uneven distribution → hot partition

🔹 Good Partition Key Example
✅ UserId
Usually high cardinality → evenly distributed

🔹 Good Partition Key Example

✅ UserId
Usually high cardinality → evenly distributed

🔹 Cross-Partition Query

- Happens when query doesn’t include partition key
- More expensive (higher RU cost)
- Slower than single-partition query

# Logical Vs Physical Partition

1️⃣ Logical Partition
📌 Defined by Partition Key value

All items with the same partition key value
→ belong to the same logical partition

Example:

Partition Key = /userId

| userId | OrderId |
| ------ | ------- |
| 101    | A1      |
| 101    | A2      |
| 202    | B1      |

userId = 101 → one logical partition

userId = 202 → another logical partition

🔹 Characteristics

- Max 20 GB storage
- Max 10,000 RU/s
- All transactions must stay inside one logical partition
- ACID transactions supported within one logical partition only

2️⃣ Physical Partition
📌 Internal storage unit managed by Cosmos DB

Cosmos DB automatically creates physical partitions
Each physical partition:

- Max 50 GB storage
- Max 10,000 RU/s
- You do NOT control this directly.

🔷 How They Relate

Many logical partitions live inside one physical partition
If data grows → Cosmos DB automatically splits physical partitions

✅ Logical partition is defined by: Partition key value

✅ Physical partition is: Internal infrastructure unit

✅ Transactions: Only within a single logical partition

✅ Scaling: Achieved by spreading logical partitions across physical partitions

🚨 Hot Partition (Very Common Exam Scenario)

If many requests use the same partition key value:

- One logical partition gets overloaded
- That logical partition maps to one physical partition

Performance throttling happens
👉 Even if container has 100k RU/s
👉 That logical partition still capped at 10k RU/s

| Feature          | Logical Partition             | Physical Partition   |
| ---------------- | ----------------------------- | -------------------- |
| Defined by       | Partition key value           | Cosmos DB internally |
| Max Size         | 20 GB                         | 50 GB                |
| Max RU/s         | 10,000                        | 10,000               |
| User Controlled? | Yes (via partition key)       | No                   |
| Transactions     | Within same logical partition | N/A                  |

# Azure Cosmos DB: Dedicated Throughput vs Shared Throughput

Throughput in Cosmos DB is measured in:
| RU/s (Request Units per second)

You can provision RU/s at two levels:

🔹 Container level → Dedicated throughput
🔹 Database level → Shared throughput

1️⃣ Dedicated Throughput (Container-Level RU/s)
📌 RU/s allocated to ONE container only

Database
├── Container A (400 RU/s)
├── Container B (1000 RU/s)

\*\* Each container has its own isolated throughput.

🔹 Characteristics

- Performance isolated
- No competition from other containers
- Better for high-traffic workloads
- More expensive if many small containers

🔥 When to Use

- High-volume production workloads
- Uneven traffic patterns
- Critical microservices
- Need predictable performance

⚠ Example

If Container A has 400 RU/s:
It can use full 400 RU/s
Container B cannot use its RU
No sharing

2️⃣ Shared Throughput (Database-Level RU/s)
📌 RU/s shared among multiple containers

Database (1000 RU/s shared)
├── Container A
├── Container B
├── Container C

🔹 Characteristics

Cost efficient

Containers compete for RU
Good for low-traffic containers
Minimum 400 RU/s per database

🔥 When to Use

Dev/test environments
Many small containers
Low or unpredictable traffic
Cost optimization scenarios

⚠ Example

Database has 1000 RU/s shared.

If:

Container A uses 800 RU/s
Container B tries to use 400 RU/s
👉 It will get throttled (429 error)
Because total > 1000 RU/s.

| Feature        | Dedicated Throughput   | Shared Throughput         |
| -------------- | ---------------------- | ------------------------- |
| Provisioned At | Container level        | Database level            |
| Isolation      | Yes                    | No                        |
| Cost           | Higher                 | Lower                     |
| Best For       | High workload          | Multiple small containers |
| Min RU         | 400 RU/s per container | 400 RU/s per database     |
| Throttling     | Per container          | Across all containers     |

🔷 Autoscale Behavior

Both models support autoscale.

Example:
400–4000 RU/s autoscale
Cosmos scales automatically based on usage

🔥 Very Important Limitation

You CANNOT:

Move from shared → dedicated directly
Move from dedicated → shared directly

You must:
Migrate data to new container/database

# Cosmos Db Apis

| API                | Data Model        | Query Language                    | Best Use Case                     | When To Choose                     |
| ------------------ | ----------------- | --------------------------------- | --------------------------------- | ---------------------------------- |
| **Core (SQL) API** | JSON documents    | SQL-like query (SELECT \* FROM c) | New cloud-native apps             | Default choice for modern apps     |
| **MongoDB API**    | BSON documents    | MongoDB query syntax              | Lift-and-shift Mongo apps         | Migrating existing MongoDB apps    |
| **Cassandra API**  | Wide-column       | CQL (Cassandra Query Language)    | Large-scale write-heavy workloads | Migrating Cassandra workloads      |
| **Table API**      | Key-Value (NoSQL) | OData                             | Simple key-value apps             | Migrating from Azure Table Storage |
| **Gremlin API**    | Graph             | Gremlin traversal language        | Relationship-heavy systems        | Social networks, fraud detection   |

1️⃣ Core (SQL) API (Most Recommended)
📌 Native Cosmos API

- JSON document database
- Rich querying
- Stored procedures
- Transactions (within partition)

✅ Use Cases

- E-commerce apps
- Microservices
- IoT device state
- User profile systems
- Modern web/mobile apps

💡 Why Choose It?

- Full Cosmos DB feature set
- Best SDK support (.NET, Java, Node)
- Recommended for new development

2️⃣ MongoDB API
📌 MongoDB-compatible wire protocol

You can use:

MongoDB drivers

Existing Mongo tools

✅ Use Cases

- Migrating MongoDB to cloud
- Teams already skilled in Mongo
- Apps tightly coupled to Mongo driver

⚠ Important

- Not 100% Mongo feature parity
- Cosmos backend still different

3️⃣ Cassandra API
📌 Wide-column data model

Designed for:

High write throughput
Massive distributed workloads

✅ Use Cases

Telemetry ingestion
IoT streaming data
Logging systems

Time-series workloads

💡 Choose When

Migrating Apache Cassandra apps
Need CQL compatibility

4️⃣ Table API
📌 Key-value store (like Azure Table Storage)

Simple schema:
PartitionKey
RowKey

✅ Use Cases

Simple metadata storage
Lightweight lookup data
Migrating Azure Table Storage apps

⚠ Limited querying capabilities

5️⃣ Gremlin API (Graph API)
📌 Graph database model

Stores:

Vertices (nodes)
Edges (relationships)

Uses:
Gremlin traversal language

✅ Use Cases

Fraud detection
Social network relationships
Recommendation engines
Network topology
Supply chain mapping

- API for Table only supports OLTP scenarios.

- Azure CosmosDb Provides 3 different types of Backup policies
  - Periodic | Backup is taken at periodic interval based on your configuration
  - Continous (7days) | Provides backup window of 7 days / 168 hours and you can restore to any point of time within the window. This mode is available for free.
    - You will not be able to switch to Periodic mode once you adopt Continuous mode.
  - Continous (30days)

- Provision a dedicated gateway cluster for your Azure Cosmos DB account. A dedicated gateway is compute that is a front-end to data in your Azure Cosmos DB account. Your dedicated gateway automatically includes the integrated cache, which can improve read performance.

## Things to note in Stored Procedures

❌ Stored procedure across partitions → Not allowed
❌ Calling without partition key → Not allowed
❌ Using C# inside Cosmos → Not allowed
❌ Expecting cross-container transaction → Not supported
Stored procedures are written in JavaScript only.
❌ No cross-partition document creation
❌ No querying other partitions

Sample

`function createMyDocument(partitionKey) {
var context = getContext();
var collection = context.getCollection();
var response = context.getResponse();

    var documentToCreate = {
        id: partitionKey,
        name: "Sample Product",
        price: 100,
    };

    var accepted = collection.createDocument(
        collection.getSelfLink(),
        documentToCreate,
        function (err, createdDoc) {
            if (err) throw new Error("Error: " + err.message);
            response.setBody(createdDoc.id);
        }
    );

    if (!accepted) {
        throw new Error("Request not accepted by server.");
    }

}
`

#### Important

Using /id as partition key is almost always wrong for real workloads.
Why?

- Every document becomes its own logical partition
- No grouping
- No transactional batching
- Poor query performance
- No aggregation inside partition
- It defeats the purpose of partitioning.

# UDF

- UDFs are mainly designed for queries — not for stored procedures.

| Question                                     | Answer                |
| -------------------------------------------- | --------------------- |
| Does Change Feed support scaling?            | Yes                   |
| How?                                         | Using lease container |
| Can multiple processors read same partition? | No                    |
| Where is checkpoint stored?                  | Lease container       |
