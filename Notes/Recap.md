- Azure Service Bus dead-letter queue is designed to hold messages that cannot be delivered to any receiver or are not processed successfully, which is suitable for the company's requirement.
- Azure Storage queues with visibility timeout only hide messages temporarily and do not move them to separate storage.
- Azure Event Grid with advanced filtering is for event routing, not for handling failed message processing. Azure Queue Storage with message expiration simply deletes expired messages rather than moving them for analysis.
- filter condition for an Azure Service Bus topic.
  - SQL | A SqlFilter holds a SQL-like conditional expression that is evaluated in the broker against the arriving message’s user-defined properties and system properties.
  - Boolean | The TrueFilter and FalseFilter either cause all arriving messages (true) or none of the arriving messages (false) to be selected for the subscription.
  - Corelation | holds a set of conditions that are matched against one or more of an arriving message's user and system properties.
- The code segment that includes the `–azure-file-volume-mount-path` parameter and the `--azure-file-volume-share-name` parameter creates a container in a container group and mounts an Azure file share as volume.
  - To create a container in a container group and mount an Azure file share as volume.
    `az container create -g MyResourceGroup --name myapp --image myimage:latest 
--command-line "cat /mnt/azfile/myfile"
--azure-file-volume-share-name myshare 
--azure-file-volume-account-name mystorageaccount 
--azure-file-volume-account-key mystoragekey 
--azure-file-volume-mount-path /mnt/azfile`
- Init containers are meant to perform initialization logic for app containers, running to completion before the application containers start.
- A repository is a collection of container images or other artifacts in a registry that have the same name but different tags.
- A namespace enables the identification of related repositories and artifact ownership by using forward slash-delimited names.
- Azure Container Apps is a serverless container service that automatically scales and recovers from failures, making it suitable for applications with fluctuating workloads and high availability requirements.
- Using a production and staging slot with `auto swap enabled` reduces the likelihood of locked files.
- Marking a setting as a deployment slot setting keeps it sticky to that deployment slot.
- The Consumption hosting plan satisfies autoscaling, has event-based scaling behavior, and provides a serverless pricing model.
- The Standard/ Premium plan supports both custom domains and Microsoft Defender for Cloud, which can automatically alert on dangling DNS domains.
- The fan-out/fan-in pattern enables multiple functions to be executed in parallel, waiting for all functions to finish.
- [Function Apps] Using the dynamicThrottlesEnabled property allows developers to let the system respond dynamically to an increased utilization, returning “429 Too Busy” errors, defined in host.json
- [Function Apps] The bindings section, part of the function.json file, is used to define the bindings and triggers for a function.
- [Function App] The `maxConcurrentRequests` property is used to determine the maximum number of function instances to run in parallel. It is defined in the host.json file.
- [Function App] The `maxOutstandingRequests` property, defined in the host.json file, defines the maximum number of requests, queued or in progress, held at any given time.
- [Cosmos DB] setting the LeaseCollectionPrefix property to “ALL” only affects lease container partitioning for scaling
- [Cosmos DB Consistency Level]
  - The _Consistent Prefix_ consistency level ensures that updates made as a batch within a transaction are returned consistently with the transaction in which they were committed.
  - Write operations within a transaction of multiple documents are always visible together.
  - The _Bounded Staleness_ consistency level is used to manage the lag of data between any two regions based on an updated version of an item or the time intervals between read and write.
  - The _Session consistency_ level is used to ensure that within a single client session, reads are guaranteed to honor the read-your-writes and write-follows-reads guarantees.
  - The _Eventual consistency_ level is used when no ordering guarantee is required.
  - The Write permission will allow users to create and update blobs.
    - The Add permission is only applicable for append blobs.
    - The Create permission only allows users to create blobs.

| Concept       | ACI Container Group                | Container Apps Environment    |
| ------------- | ---------------------------------- | ----------------------------- |
| Purpose       | Run containers together            | Host many container apps      |
| Lifecycle     | All containers start/stop together | Each app scales independently |
| Scaling       | ❌ None                            | ✅ Auto-scale per app         |
| Networking    | Shared IP/ports                    | Ingress, internal DNS         |
| Isolation     | Very limited                       | Logical environment boundary  |
| Microservices | ❌ Not suitable                    | ✅ Designed for it            |

- | Feature         | **Event Grid**      | **Service Bus**    | **Event Hub**       |
  | --------------- | ------------------- | ------------------ | ------------------- |
  | Purpose         | Event notification  | Business messaging | Big data streaming  |
  | Pattern         | Push events         | Pull messages      | Streaming ingestion |
  | Ordering        | ❌                  | ✅ (Sessions)      | Per-partition only  |
  | Dead-letter     | ❌                  | ✅                 | ❌                  |
  | Replay events   | ❌                  | ❌                 | ✅                  |
  | Retention       | ❌                  | ❌                 | ✅ (1–90 days)      |
  | Pub/Sub         | ✅                  | ✅                 | ❌                  |
  | Scale           | Millions events/day | Thousands/sec      | **Millions/sec**    |
  | Typical payload | Small JSON          | Business objects   | Telemetry / logs    |

  - Event Grid – “Something happened”

            Used for:

            - Blob created
            - VM started
            - Resource changed
            - Webhook notifications

            Fire & forget — no replay, no guarantee of consumer availability.

- Service Bus – “Do this work”

        Used for:

        - Orders
        - Payments
        - Workflows
        - Microservice commands
        - Guaranteed processing
        - Reliable, transactional, ordered.

- Event Hub – “Here is a stream of data”

        Used for:

        - IoT telemetry
        - Logs
        - Metrics
        - Kafka pipelines
        - Real-time analytics
        - Mass ingestion, replayable streams.

  Exam pattern cheat
  Question says Pick

  - “Notify”, “react to”, “trigger” Event Grid
  - “Process order”, “transaction”, “workflow” Service Bus
  - “Millions of telemetry”, “streaming” Event Hub

- Namespace in ACR = a logical path prefix inside a registry used to group related container repositories.
- Azure API Management instance and import the existing API using the Azure portal's API import functionality.
  - This is mandatory to get the APIs into APIM.
  - Importing allows you to leverage OpenAPI (Swagger), WSDL, or other formats for automatic setup.
- Azure.Storage.Sas.**BlobSasPermissions** contains the list of permissions that can be set for a blob's access policy.

`public enum BlobSasPermissions
{
    //
    // Summary:
    //     Indicates that Read is permitted.
    Read = 1,
    //
    // Summary:
    //     Indicates that Add is permitted.
    Add = 2,
    //
    // Summary:
    //     Indicates that Create is permitted.
    Create = 4,
    //
    // Summary:
    //     Indicates that Write is permitted.
    Write = 8,
    //
    // Summary:
    //     Indicates that Delete is permitted.
    Delete = 0x10,
    //
    // Summary:
    //     Indicates that reading and writing Tags are permitted.
    Tag = 0x20,
    //
    // Summary:
    //     Indicates that deleting a Blob Version is permitted.
    DeleteBlobVersion = 0x40,
    //
    // Summary:
    //     Indicates that List is permitted.
    List = 0x80,
    //
    // Summary:
    //     Indicates that Move is permitted.
    Move = 0x100,
    //
    // Summary:
    //     Indicates that Execute is permitted.
    Execute = 0x200,
    //
    // Summary:
    //     Indicates that setting immutability policy is permitted.
    SetImmutabilityPolicy = 0x400,
    //
    // Summary:
    //     Indicates that Permanent Delete is permitted.
    PermanentDelete = 0x800,
    //
    // Summary:
    //     Indicates that all permissions are set.
    All = -1
}`
| Permission | Description |
| -------------------------------- | -------------------------------------- |
| **Read (r)** | Read blob content and properties |
| **Add (a)** | Add a block to a block blob |
| **Create (c)** | Create a new blob or snapshot |
| **Write (w)** | Write or update blob content |
| **Delete (d)** | Delete the blob |
| **DeleteVersion (x)** | Delete a specific blob version |
| **List (l)** | List blobs in the container |
| **Tag (t)** | Read/write blob tags |
| **PermanentDelete (p)** | Permanently delete a soft-deleted blob |
| **SetImmutabilityPolicy (i)** | Set an immutability policy on blob |
| **UpdateImmutabilityPolicy (u)** | Update an existing immutability policy |
| **Restore (o)** | Restore a soft-deleted blob |

- Function App

| Setting                     | File        | Scope / Notes                                                                                                                      |
| --------------------------- | ----------- | ---------------------------------------------------------------------------------------------------------------------------------- |
| **maxConcurrentRequests**   | `host.json` | Applies **host-wide** to all HTTP-triggered functions. Controls how many requests a single host instance can process concurrently. |
| **maxOutstandingRequests**  | `host.json` | Applies **host-wide**. Sets the maximum number of requests waiting in the queue before being rejected or throttled.                |
| **dynamicThrottlesEnabled** | `host.json` | Applies **host-wide**. Enables automatic load-based throttling for incoming requests.                                              |
| **hsts**                    | `host.json` | Applies **host-wide** to HTTP-triggered functions. Configures HTTP Strict Transport Security headers.                              |

- two possible ways to rehydrate offline blob
  - Copy the blob to a new blob in the Hot or Cool tier with the Copy Blob operation.
  - Change the blob’s tier using the Set Blob Tier operation.
- Configuring a legal hold policy on the container allows the documents to be protected from modifications or deletions until the hold is explicitly cleared. [Question]
- Best pracise to choose Production Image Tag
  - Production: Use the full semantic version tag (2.9.0)
  - Testing / evaluation: Use latest or v3-preview
  - Never use v3 in production, because minor/patch updates happen automatically
- In AZ Web App, to enable and configure Azure Web Service Local Cache with 1.5 GB,
  - By using _WEBSITE_LOCAL_CACHE_OPTION = Always_, local cache will be enabled. _WEBSITE_LOCAL_CACHE_SIZEINMB_ will properly configure Local Cache with 1.5 GB of size
- In AZ Web App, to load a TLS/SSL certificate in application code.
  - The _WEBSITE_LOAD_CERTIFICATES_ app setting makes the specified certificates accessible to Windows or Linux custom containers as files.
- WEBSITE_ROOT_CERTS_PATH is read-only and does not allow comma-separated thumbprint values to be mentioned to the certificates and then be loaded in the code.
- The App Service, App Service Environment, and Functions Premium hosting plans support autoscaling but does not provide the serverless pricing model. Its scaling behavior is not event based but performance based.
- [Function APP] The fan-out/fan-in pattern enables multiple functions to be executed in parallel, waiting for all functions to finish. Often, some aggregation work is done on the results that are returned from the functions.
  - The _function chaining pattern_ is a sequence of functions that execute in a specific order. In this pattern, the output of one function is applied to the input of another function.
- [Function App] he _functionAppScaleLimit_ property lets you define the number of instances of the Azure Functions app that will be created.
  - A value of 0 or null for the _functionAppScaleLimit_ property means that an unrestricted number of instances of the Azure Functions app will be created.
- [Cosmos Db] the _feedPollDelay_ parameter controls how frequently the Azure Functions change feed processor polls for new changes in the Cosmos DB container.
  - By default, there is a delay (typically 5 seconds) between polls to balance responsiveness and cost.
- [Cosmos DB] Cosmos DB supports SQL queries with parameters expressed by the @ notation. When writing SQL queries based on parameters, we need to mention the name of the container in the Azure Cosmos DB account. We do not use [accountname].[containername] or just [accountname] in the SQL query.
- Application Insights resource provides access to end-to-end transaction details including request traces, dependencies, exceptions, and telemetry collected from App.
  - This allows you to drill into detailed transaction diagnostics with minimal configuration once Application Insights is enabled.
- | Telemetry Type             | What it tells you                 | Real-world meaning                        |
  | -------------------------- | --------------------------------- | ----------------------------------------- |
  | **Requests**               | Incoming HTTP calls               | API endpoint performance & failures       |
  | **Dependencies**           | Outgoing calls (DB, APIs, queues) | What your app calls and how long it takes |
  | **Performance (Metrics)**  | CPU, memory, response time        | How healthy your app is                   |
  | **Exceptions**             | Unhandled & handled errors        | What is breaking                          |
  | **Traces (Logs)**          | Custom application logs           | What your code is saying                  |
  | **Events (Custom Events)** | Business-level actions            | What users are doing                      |
  | **Availability**           | Uptime probes                     | Is your app alive                         |
  | **Page Views (Web apps)**  | Frontend usage                    | Which pages users visit                   |
  | **Users / Sessions**       | Who & how long                    | Usage behavior                            |

1️⃣ Request Telemetry

    Tracks every incoming HTTP request:
        - URL
        - Response code
        - Duration
        - Success/failure
        - Used to detect slow endpoints and failing APIs.

2️⃣ Dependency Telemetry

    Tracks everything your app calls:
      - SQL
      - Cosmos DB
      - REST APIs
      - Service Bus, Storage, Redis, etc.
      - Shows which dependency is slowing or failing your app.

3️⃣ Performance / Metrics

    System health:

        - CPU %
        - Memory
        - Request duration
        - Throughput
        - Used for autoscaling and alerting.

4️⃣ Exception Telemetry

Tracks: -

- Stack trace
- Type of exception
- Where it happened -
- How often -
- Helps root-cause crashes.

5️⃣ Trace Telemetry

    Application logs:
        - ILogger.LogInformation()
        - LogWarning()
        - LogError()
        - Detailed execution flow.

6️⃣ Custom Events

    Business actions:

    - UserRegistered
    - PaymentCompleted
    - ReportGenerated
    - Not errors — business behavior tracking.

7️⃣ Availability

Synthetic monitoring:

- Ping tests
- Multi-region uptime checks
- Alerts when your site is down.

| You want to know…      | Telemetry    |
| ---------------------- | ------------ |
| Is my API slow?        | Requests     |
| Is my DB slow?         | Dependencies |
| Why did it crash?      | Exceptions   |
| What happened in code? | Traces       |
| What are users doing?  | Events       |
| Is my site alive?      | Availability |

- download blob content to a byte array by using an operation that automatically recovers from transient failures.

`byte[] data;
BlobClientOptions options = new BlobClientOptions();
options.Retry.MaxRetries = 10;
options.Retry.Delay = TimeSpan.FromSeconds(20);
BlobClient client = new BlobClient(new Uri("https://mystorageaccount.blob.core.windows.net/containers/blob.txt"), options);
Response<BlobDownloadResult> response = client.DownloadContent();
data = response.Value.Content.ToArray();`

- If you define more than one action on the same blob, lifecycle management applies the least expensive action to the blob. For example, action delete is cheaper than action tierToArchive. Action tierToArchive is cheaper than action tierToCool. [Important]

- Microsoft Entra ID credentials are required to generate the SAS token. The account used must have the _Microsoft.Storage/storageAccounts/blobServices/generateUserDelegationKey_ permission, which is present in the following built-in roles:** Contributor, Storage Account Contributor, Storage Blob Data Contributor, Storage Blob Data Owner, Storage Blob Data Reader, and Storage Blob Delegator.** The account key can be used to generate the SAS token, but it can be more easily compromised.

- Application Insights allows you to create web tests that simulate user interactions with your application and then set up alerts based on the results of these tests.
- Azure Monitor Resource Health alerts are used for infrastructure monitoring, not application performance.
- Azure Service Health provides information about Azure service issues and planned maintenance, not application performance.
- Azure Advisor provides best practice recommendations, not application performance alerts.

- **Azure Bicep** is a Domain Specific Language (DSL) for declaratively defining, deploying, and managing Azure resources as Infrastructure as Code (IaC), offering a simpler, more readable syntax than traditional ARM (Azure Resource Manager) JSON templates, which it transpiles into behind the scenes, simplifying complex deployments with features like modules and type safety for better developer experience.
