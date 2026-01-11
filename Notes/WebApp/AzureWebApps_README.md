Single-tenant → App is used by users from one Entra ID tenant only
Multi-tenant → App can be used by users from multiple Entra ID tenants

Multi Tenant App Service Networking Features

![Multi Tenant App Service Networking Features](image-12.png)

# Default Networking behavior

- Azure App Service scale units support many customers in each deployment.
- The Free and Shared SKU plans host customer workloads on multitenant `workers`.
- The Basic and higher plans host customer workloads that are dedicated to only one App Service plan.
- If you have a Standard App Service plan, all the apps in that plan run on the **same** worker. - If you scale out the worker, all the apps in that App Service plan are replicated on a new worker for each instance in your App Service plan.

- When you change the virtual machine family, you get a different set of outbound addresses.
  The worker virtual machines are broken down in large part by the App Service plans.

- There are many addresses that are used for outbound calls.
- The outbound addresses used by your app for making outbound calls are listed in the properties for your app.
- These addresses are shared by all the apps running on the same worker virtual machine family in the App Service deployment.

# Question

You are running you app service in Standard plan. And you have configured to call external service from the apps inside the App Service. But later you upgrade the App Service Plan to PremiumV2. Now the apps inside cannot communication with external services? Why and how to fix this?

- Azure VM outbound address = the public IP seen by external services when the VM initiates outbound traffic.

To find the list of outbound addresses, outbound IP addresses currently used by your app | CLI Command
`az webapp show \
    --resource-group <group_name> \
    --name <app_name> \ 
    --query outboundIpAddresses \
    --output tsv`

- To find all possible outbound IP addresses for your app, regardless of pricing tiers

`az webapp show \
    --resource-group <group_name> \ 
    --name <app_name> \ 
    --query possibleOutboundIpAddresses \
    --output tsv`

- AppService Plan vs Pricing Tier

App Service Plan → The infrastructure you reserve
Pricing Tier → The size & features of that infrastructure
-->
App Service Plan → The gym membership
Pricing Tier → The membership level (basic, premium, VIP)

- App Service Plan
  What it is

  # An App Service Plan defines:

        - Region
        - OS (Windows / Linux)
        - Compute resources shared by apps:
        - CPU
        - Memory
        - Disk
        Think of it as the container (server farm) that hosts your apps.

  Key points

  - Multiple apps can run on one plan
  - All apps share the same resources
  - Billing is tied to the plan, not the app

Pricing Tier

What it is

A Pricing Tier is a setting of an App Service Plan that defines:

    - VM size (CPU / RAM)
    - Scaling limits
    - Features available

    Examples:

    - Free (F1)
    - Basic (B1–B3)
    - Standard (S1–S3)
    - Premium (P1v3–P3v3)
    - Isolated (I1–I3)

[Important] - Every App Service Plan has exactly one Pricing Tier

To Note -

- Billing is per App Service Plan
- Scaling affects all apps in the plan
- To isolate performance → use separate plans
- To change size/features → change pricing tier

_To Note_
In a default Linux app service or a custom Linux container, any nested JSON key structure in the app setting name like ApplicationInsights:InstrumentationKey needs to be configured in App Service as ApplicationInsights**InstrumentationKey for the key name. In other words, replace any : with ** (double underscore). Any periods in the app setting name are replaced with a \_ (single underscore).

- At runtime, connection strings are available as environment variables, prefixed with the following connection types:

- SQLServer: SQLCONNSTR\*
- MySQL: MYSQLCONNSTR\*
- SQLAzure: SQLAZURECONNSTR\*
- Custom: CUSTOMCONNSTR\*
- PostgreSQL: POSTGRESQLCONNSTR\*
- Notification Hub: NOTIFICATIONHUBCONNSTR\*
- Service Bus: SERVICEBUSCONNSTR\*
- Event Hub: EVENTHUBCONNSTR\*
- Document DB: DOCDBCONNSTR\*
- Redis Cache: REDISCACHECONNSTR\*

  For example, a MySQL connection string named connectionstring1 can be accessed as the environment variable MYSQLCONNSTR_connectionString1.

- Set AppSetting for Container Environment
  CLI Command
  `az webapp config appsettings set --resource-group <group-name> --name <app-name> --settings key1=value1 key2=value2`

When your app runs, the App Service app settings are injected into the process as environment variables automatically. You can verify container environment variables with the URL `https://<app-name>.scm.azurewebsites.net/Env`.

Note - Most modern browsers support HTTP/2 protocol over TLS only, while nonencrypted traffic continues to use HTTP/1.1. To ensure that client browsers connect to your app with HTTP/2, secure your custom DNS name.

**Path Mapping**

- In Azure App Service, Path Mapping tells Azure which physical directory on the App Service file system should serve a specific URL path.
- You can configure virtual applications and directories by specifying each virtual directory and its corresponding physical path relative to the website root (D:\home).
- To mark a virtual directory as a web application, clear the Directory check box.

Steps to Add Custom Storage for a Containerized App

1. Select --> New Azure Storage Mount
2. Name: The display name
3. Configuration options - Basic or Advanced. Select Basic if the storage account isn't using [service endpoints, private endpoint or azure key vault], else Advanced
4. storage accounts - the storage account with the container you want
5. Storage type: Azure Blobs or Azure Files.
   1. Windows container apps only support Azure Files.
   2. Azure Blobs only supports read-only access.
6. Storage container: For basic configuration, the container you want.
7. Share name: For advanced configuration, the file share name.
8. Access key: For advanced configuration, the access key.
9. Mount path: The absolute path in your container to mount the custom storage.
10. Deployment slot setting: When checked, the storage mount settings also apply to deployment slots.

**AppService Logging**

![Logs, supported platforms, location](image-13.png)

Main Log Types

- Application Logs
- Web Server Logs
- Detailed Error Logs
- Failed Request Logs
- Deployment Logs

For Windows Platform, Application Logs can be stored in either blob or azure file system. The Filesystem option is for temporary debugging purposes, and turns itself off in 12 hours.

For Linux/Container Web App, to store Application Logs

- Only option is File System
- Set Quota (MB) & Retention Period (days)

- ASP.NET applications can use the `System.Diagnostics.Trace` class to log information to the application diagnostics log.
- By default, ASP.NET Core uses the `Microsoft.Extensions.Logging.AzureAppServices` logging provider.

**Stream Logs**

- Before you stream logs in real time, enable the log type that you want. Any information written to files ending in .txt, .log, or .htm that are stored in the `/LogFiles` directory `(d:/home/logfiles)` is streamed by App Service.

For logs stored in the App Service file system, the easiest way is to download the ZIP file in the browser at:

- Linux/container apps: `https://<app-name>.scm.azurewebsites.net/api/logs/docker/zip`
- Windows apps: `https://<app-name>.scm.azurewebsites.net/api/dump`

[Important]
For Linux/container apps, the ZIP file contains console output logs for both the docker host and the docker container.
For a scaled-out app, the ZIP file contains one set of logs for each instance. In the App Service file system, these log files are the contents of the /home/LogFiles directory.

- A certificate uploaded into an app is stored in a deployment unit that is bound to the app service plan's resource group and region combination (internally called a **webspace**).
- The certificate is accessible to other apps in the same resource group and region combination.

- options you have for adding certificates in App Service

![Options to add certificate](image-14.png)

[Important] - Public certificates aren't used to secure custom domains

- To use private certificate in app service, it must meet the following requirements

  - Exported as a password-protected PFX file, encrypted using triple DES.
  - Contains private key at least 2,048 bits long.
  - Contains all intermediate certificates and the root certificate in the certificate chain.

- To secure a custom domain in a TLS binding, the certificate has other requirements:
  - Contains an Extended Key Usage for server authentication (OID = 1.3.6.1.5.5.7.3.1)
  - Signed by a trusted certificate authority

[Important]

- To create custom TLS/SSL bindings or enable client certificates for your App Service app, your App Service plan must be in the Basic, Standard, Premium, or Isolated tier.
- The free App Service managed certificate is a turn-key solution for securing your custom DNS name in App Service. It's a TLS/SSL server certificate fully managed by App Service and renewed continuously and automatically in six-month increments, 45 days before expiration.

- The free certificate comes with the following limitations:

  - Doesn't support wildcard certificates.
  - Doesn't support usage as a client certificate by using certificate thumbprint, which is planned for deprecation and removal.
  - Doesn't support private DNS.
  - Isn't exportable.
  - Isn't supported in an App Service Environment (ASE).
  - Only supports alphanumeric characters, dashes (-), and periods (.).
  - Only custom domains of length up to 64 characters are supported.

[Important]

- App Service Certificates aren't supported in Azure National Clouds at this time.

- Azure Web Apps Automatic Scaling, 2 Options
  1. Auto scale - Scaling done by defined rules
  2. Automatic Scale - Scaling done by defined parameters

![auto scale vs automatic scale](image-15.png)

- Autoscaling is doing scale in/out instead scale up/down.

  - Autoscaling responds to changes in the environment by adding or removing web servers and balancing the load between them.
  - Autoscaling doesn't have any effect on the CPU power, memory, or storage capacity of the web servers powering the app, it only changes the number of web servers.
  - 2 Options for Autoscaling
    - based on metric
    - scale to specific instance count according to a schedule
  - [ If you need to scale out incrementally, you can combine metric and schedule-based autoscaling in the same autoscale condition. ]
  - An App Service Plan also has a default condition that is used if none of the other conditions are applicable. This condition is always active and doesn't have a schedule.

- Metrics for autoscale rules for a web app

  - CPU Percentage - cpu usage accross all instances.
  - Memory Percentage - This metric captures the memory occupancy of the application across all instances.
  - Disk queue length - This metric is a measure of the number of outstanding I/O requests across all instances.
  - Http Queue Length - This metric shows how many client requests are waiting for processing by the web app. If this no is large will throw 408 http error
  - Data In - the number of bytes received across all instances.
  - Data Out - the number of bytes sent by all instances.

- An autoscale action has a cool down period, specified in minutes. During this interval, the scale rule can't be triggered again. The minimum cool down period is five minutes.
- A single autoscale condition can contain several autoscale rules
- For Autoscaling can have multiple set of rules for scale in/out. [!Important]

  - Scale out will be done if either one of the scale out conditions true
  - Scale in will be done when all the scale in conditions are true

- [Not all pricing tiers support autoscaling. The development pricing tiers are either limited to a single instance (the F1 and D1 tiers), or they only provide manual scaling (the B1 tier). If you selected one of these tiers, you must first scale up to the S1 or any of the P level production tiers.]

- Automatic Scaling [scaling by defined parameters]
  - When to use Automatic Scaling
    - You don't want to set up autoscale rules based on resource metrics.
    - You want your web apps within the same App Service Plan to scale differently and independently of each other.
    - Your web app is connected to a database or legacy system, which may not scale as fast as the web app. Scaling automatically allows you to set the maximum number of instances your App Service Plan can scale to. This setting helps the web app to not overwhelm the backend.

**Slots**

- Slots are available Pricing Tiers Standard, Premium, Isolated.
- The slot's URL has the format `http://sitename-slotname.azurewebsites.net`.

![Settings that are swapped and not swapped](image-16.png)

- Note | To make settings swappable, add the app setting _WEBSITE_OVERRIDE_PRESERVE_DEFAULT_STICKY_SLOT_SETTINGS_ in every slot of the app and set its value to 0 or false. These settings are either all swappable or not at all. You can't make just some settings swappable and not the others. Managed identities are never swapped and aren't affected by this override app setting.

- To configure an app setting or connection string to stick to a specific slot (not swapped), go to the Configuration page for that slot. Add or edit a setting, and then select Deployment slot setting. Selecting this check box tells App Service that the setting isn't swappable.

- Auto swap isn't currently supported in web apps on Linux and Web App for Containers.
- pre warmup or custom initialization,

`<system.webServer>
    <applicationInitialization>
        <add initializationPage="/" hostName="[app hostname]" />
        <add initializationPage="/Home/About" hostName="[app hostname]" />
    </applicationInitialization>
</system.webServer>`

- App Settings can be used to warm-up

  1. WEBSITE_SWAP_WARMUP_PING_PATH: The path to ping to warm up your site. Add this app setting by specifying a custom path that begins with a slash as the value. An example is /statuscheck. The default value is /.
  2. WEBSITE_SWAP_WARMUP_PING_STATUSES: Valid HTTP response codes for the warm-up operation. Add this app setting with a comma-separated list of HTTP codes. An example is 200,202 . If the returned status code isn't in the list, the warmup and swap operations are stopped. By default, all response codes are valid.
  3. WEBSITE_WARMUP_PATH: A relative path on the site that should be pinged whenever the site restarts (not only during slot swaps). Example values include /statuscheck or the root path, /.

- By setting route traffic % can divert route traffice among the slots.
- After a client is automatically routed to a specific slot, it's "pinned" to that slot for the life of that client session. On the client browser, you can see which slot your session is pinned to by looking at the x-ms-routing-name cookie in your HTTP headers. A request routed to the "staging" slot has the cookie `x-ms-routing-name=staging`. A request routed to the production slot has the cookie `x-ms-routing-name=self`.
- Rather doing automatic traffic manage, can do manual routing among slots using the query params.
  - ex. `.azurewebsites.net/?x-ms-routing-name=staging`

# App Service on Linux | Limitation

1. Azure portal shows only the features support for Linux
2. Not supported for Shared pricing tier
3. When deployed to built-in images, your code and content are allocated as a storage volume for web content, backed by Azure Storage.
   1. The disk latency of this volume is higher and more variable than the latency of the container filesystem.
   2. Apps that require heavy read-only access to content files might benefit from the custom container option, which places files in the container filesystem instead of on the content volume. [Important]
   - This can be a question, as in what is the App Service type should be to deploy heavy readonly application?

- Azure App Service zone redundancy is supported only on Premium v2 and Premium v3 plans.
- Slots are available only in Standard or Premium Plans

- CICD Supports for

  - Azure DevOps
  - Github
  - Bitbucket

- Supported Auth Providers
  - Microsoft Entra
  - google
  - X
  - Facebook
  - Any OpenId Connect Provider
  - Github
  - Apple
- The authentication and authorization middleware component is a feature of the platform that runs on the same VM as your application.
- In Linux and containers the authentication and authorization module runs in a separate container, isolated from your application code.

- Web Apps can be Authenticated/ Athourized using the allowed.

  - Without Provider SDK (Server Side Flow)
    - This is useful when it cann access browser to provide provider login page into user, from the browser.
  - With Provider SDK (Client Side Flow)
    - The application signs users in to the provider manually and then submits the authentication token to App Service for validation.

- Authentication Flow
  ![Authentication Flow with Providers](WebApp_Auth_Flow.png)

# Authorization Behavior

- in AZ portal, you can configure App Service with many behaviors when an incoming request isn't authenticated.

  - Allow unauthenticated requests
    - This option defers authorization of unauthenticated traffic to your application code.
    - For authenticated requests, App Service also passes along authentication information in the HTTP headers.
    - This option provides more flexibility in handling anonymous requests. It lets you present multiple sign-in providers to your users.
  - Require Authentication
    - This option rejects any unauthenticated traffic to your application.

- The token store is only available when using the built-in authentication feature, and tokens can be accessed via environment variables or HTTP headers.

- App settings are always encrypted when stored (encrypted-at-rest).
- App settings names can only contain letters, numbers (0-9), periods ("."), and underscores ("\_")
- Special characters in the value of an App Setting must be escaped as needed by the target OS.

- Important App Service Configurations

  - ARR - In a multi-instance deployment, ensure that the client is routed to the same instance for the life of the session. You can set this option to Off for stateless applications.
  - HTTP version - Set to 2.0 to enable support for HTTPS/2 protocol.
  - Debugging - Enable remote debugging for ASP.NET, ASP.NET Core, or Node.js apps. This option turns off automatically after 48 hours.

- Path mapping allows you to map a specific URL path to a virtual directory or application in your Azure Web App.

# Why it’s useful

- Host multiple apps under a single Web App
- Separate static content from main app
- Organize apps in subfolders or virtual directories
- Useful for legacy apps or multi-app solutions

ex:
| Path | Physical path | Type |
| ----- | ----------------- | ----------------- |
| /api | site\wwwroot\api | Application |
| /blog | site\wwwroot\blog | Virtual directory |

Virtual Application:

- Treated as a separate “app” inside the Web App
- Has its own web.config, can run separately

Virtual Directory:

- Just a mapped folder
- Shares the parent app’s configuration

Physical path is always relative to wwwroot (for Windows) or /home/site/wwwroot (for Linux).

# Windows App (Uncontainerized)

- For Windows apps, you can customize the _IIS handler mappings_ and _virtual applications_ and _directories_.

### Handler Mappings

- Handler mappings let you add custom script processors to handle requests for specific file extensions.

  - Extension: The file extension you want to handle, such as \*.php or handler.fcgi.
  - Script processor: The absolute path of the script processor.
    - Requests to files that match the file extension are processed by the script processor.
    - Use the path D:\home\site\wwwroot to refer to your app's root directory.
  - Arguments: Optional command-line arguments for the script processor.

- ASP.NET Core includes the Azure App Service logging provider.
  `builder.Logging.AddAzureWebAppDiagnostics();`

- Any information written to files ending in .txt, .log, or .htm that are stored in the `/home/`LogFiles directory (Windows: D:\home\LogFiles) is streamed by App Service.

- To stream web app logs in Azure CLI, use the below commands

  - `az webapp log tail --name appname --resource-group myResourceGroup`
  - [For Specific Logs] `az webapp log tail --name appname --resource-group myResourceGroup --provider http`

- For Linux/container apps, the ZIP file contains console output logs for both the docker host and the docker container.

# Certificates

- Certificates are used to secur the app service custom domain.
- Before we add certificates we must have purchased/ added custom domain.
- Certificates are available only for the Dedicated+ pricing tiers, as in, this is not available for the tiers, Free/ Shared.
- There are 4 ways we can add certificiates

1. create a free app service managed certificate
   1. this is free
   2. managed by the app service.
   3. Prerequisites to create free managed certificate
      1. Free certs are issues by the DigiCert.
      2. For some domains, you must explicitly allow DigiCert as a certificate issuer by creating a CAA domain record with the value: `0 issue digicert.com`
      3. Make sure to avoid hard dependencies and "pinning" practice certificates to the managed certificate or any part of the certificate hierarchy.
2. purchase an app service certificate
   1. Managed by Azure
   2. combines the automate cert management and flexbility of renewal and export options.
3. import a certificate from key vault
   1. useful if use Azure KeyVault to manage certs
4. upload a private certificate
   1. upload private certificate, purchased from a 3rd party provider
   2. you have to manage renewal, export, update etc.
5. upload a public certificate
   1. Public certs are not used to secure custom domains.
   2. can load them into code if need them to access remote resources.

### Important difference

- Free Certificate

| Feature                   | Free Managed Cert                   |
| ------------------------- | ----------------------------------- |
| Cost                      | **Free**                            |
| Type                      | Domain-validated (DV)               |
| Validity                  | 1 year (auto-renewed)               |
| Renewal                   | **Fully automatic**                 |
| Scope                     | **Only works on Azure App Service** |
| Exportable                | ❌ No                               |
| Supports wildcard         | ❌ No                               |
| Supports apex/root domain | ❌ No (`contoso.com`)               |
| Supports subdomain        | ✔️ Yes (`www.contoso.com`)          |
| Private key access        | ❌ No                               |
| Use outside Azure         | ❌ No                               |

👉 Use when:

- Simple public websites
- You don’t need wildcards or root domains
- You want zero maintenance

- App Service Certificate (Paid)

| Feature                   | Paid App Service Cert                                             |
| ------------------------- | ----------------------------------------------------------------- |
| Cost                      | **Paid (~$70+/yr)**                                               |
| Type                      | Domain-validated (DV)                                             |
| Validity                  | 1 year                                                            |
| Renewal                   | Manual or scripted                                                |
| Exportable                | ✔️ Yes (PFX)                                                      |
| Supports wildcard         | ✔️ Yes                                                            |
| Supports apex/root domain | ✔️ Yes                                                            |
| Private key access        | ✔️ Yes                                                            |
| Can use on                | Azure App Service, VM, Application Gateway, CDN, Front Door, etc. |

👉 Use when:

- You need wildcard or root domain support
- Same cert must be reused across services
- You need private key access

## Web App Scaling

- There are 2 main ways to Scale Web App
  - Manually
  - Automatically Scale
    - Autoscale
    - Automatic Scale

1. Autoscale

   1. This will do only `Scale in/out`. Not `Scale up/down`
   2. That means this will add instances to balance the load.
   3. This is Rule-based, not the HTtp Traffice Based.
   4. can be done schedule based
   5. Available for Standard+ pricing tiers.
   6. No need Prewarmed instances, No need per-app maximum, No need Always ready-instances.
   7. Autoscale will not fix memory or CPU starvation.
   8. Autoscaling improves availability and fault tolerance.
   9. Autoscaling isn't the best approach to handling long-term growth.
   10. Autoscaling has an overhead associated with monitoring resources and determining whether to trigger a scaling event.
   11. Spin up new instances will be triggered, by the resource matrics.
   12. The rules defined here applied to all the Apps within the App Service Plan.
   13. Autoscale rules are applied at the App Service Plan level, not at the individual Web App level.
   14. Require Standard+ Pricing Tier/ Plan.
   15. Options
       1. Schdule
       2. Rule based/ Metics based
   16. Steps follow to analyzes metrics
       1. collect the value for the rule from all the instances - Time grain
       2. Agreegate those values - Time aggregation
       3. Calculate the (2) for the given Duration [user specified time to look for]
   17. An autoscale action has a `cool down period`, specified in minutes. During this interval, the scale rule can't be triggered again. Min cool down period is 5mins.

| If your problem is        | Correct fix                  |
| ------------------------- | ---------------------------- |
| Too many users            | Autoscale OUT                |
| App running out of memory | Change App Service Plan tier |
| CPU bottleneck            | Change App Service Plan tier |
| Storage full              | Change App Service Plan tier |

1. Automatic Scaling
   1. This will do Scale in/ out based on the traffic.
   2. And can configure this in the Web App Level within the App Service Plan.
   3. Available for the plans Premium V2 or V3

![Autoscale Rule](AutoScaleRule.png)

- The Storage queue Approximate Message Count and Service Bus Message Count metrics are special in that these metrics are point in time only and have no history. For these metrics, the `duration` field will be ignored by the Autoscale engine.
- The metric sampling duration. For example, TimeGrain = "PT1M" means that the metrics will be aggregated every 1 minute using the aggregation method specified in 'Time grain statistic' control. The grain is automatically chosen from the smallest supported grain of the selected metric. This is so the Autoscale engine can accurately respond to the metric changes.
- This is the aggregation method used to aggregate the sampled metrics. For example, TimeAggregation = "Average" will aggregate the sampled metrics by taking the average.
- Estimation during a scale-in is intended to avoid "flapping" situations, where scale-in and scale out actions continually go back and forth. Keep this behavior in mind when you choose the same thresholds for scale-out and in.

### Slots

- To enable SLOT swap, `Auto Swap Enabled` should be checked.

![Auto Swap Enabled](AutoSwapEnabled.png)

- Slot Swapping Steps
  1. Apply the following settings from target slot to all instances of the source slot
     1. Slot-specific app settings and connection strings, if applicable
     2. Continous deployment settings, if enabled
     3. app service authentication settings, if enabled

#### `Swap with preview` breaks down a normal swap into two phases.

- In phase one, any slot-specific application settings and connections strings on the destination will be temporarily copied to the source slot.
- This allows you to test the slot with its final configuration values.
- From here, you may choose to either cancel phase one to revert to your normal configuration, or proceed to phase two, which would remove the temporary config changes and complete swapping the source to destination slot.

  2. Wait for every instance in the source slot to complete its restart. If any instance fails to restart, the swap operation reverts all changes to the source slot and stops the operation.
  3. If local cache is enabled, trigger local cache initialization by making an HTTP request to the application root ("/") on each instance of the source slot. Wait until each instance returns any HTTP response. Local cache initialization causes another restart on each instance.
  4. If auto swap is enabled with `custom warm-up`, trigger Application Initiation by making an HTTP request to the application root ("/") on each instance of the source slot.

#### Custom Warm-up

- The `applicationInitialization` configuration element in web.config lets you specify custom initialization actions.

`<system.webServer>
    <applicationInitialization>
        <add initializationPage="/" hostName="[app hostname]" />
        <add initializationPage="/Home/About" hostName="[app hostname]" />
    </applicationInitialization>
</system.webServer>`

or can do using appSettings [combined tgt]

- `WEBSITE_SWAP_WARMUP_PING_PATH`: The path to ping to warm up your site. Add this app setting by specifying a custom path that begins with a slash as the value. An example is /statuscheck. The default value is /.
- `WEBSITE_SWAP_WARMUP_PING_STATUSES`: valid HTTP response codes for the warm-up operation. Add this app setting with a comma-separated list of HTTP codes. An example is 200,202 . If the returned status code isn't in the list, the warmup and swap operations are stopped. By default, all response codes are valid.
- `WEBSITE_WARMUP_PATH`: A relative path on the site that should be pinged whenever the site restarts (not only during slot swaps). Example values include /statuscheck or the root path, /

1. If all instances on the source slot are warmed up successfully, swap the two slots by switching the routing rules for the two slots. After this step, the target slot (for example, the production slot) has the app previously warmed up in the source slot.
2. Now that the source slot has the pre-swap app previously in the target slot, perform the same operation by applying all settings and restarting the instances.

-- To swap a staging slot with the production slot, make sure that the production slot is always the target slot. This way, the swap operation doesn't affect your production app.

- This is because Azure keeps the Target Slot stable until the Source Slot is stabalized with the swap operations.
- What will happen if we change the direction, as in the target as Staging Slot and the Source as Production slot, then the Production Slot will be unstabalized which may cause to the live users

  - At any point of the swap operation, all work of initializing the swapped apps happens on the source slot.

- To make settings swappable (as there are some settings which are not swappable by default), add the app setting `WEBSITE_OVERRIDE_PRESERVE_DEFAULT_STICKY_SLOT_SETTINGS` in every slot of the app and set its value to 0 or false.
- Managed identities are never swapped and aren't affected by this override app setting.

- To configure an app setting or connection string to stick to a specific slot (not swapped), go to the Configuration page for that slot. Add or edit a setting, and then select `Deployment slot setting`. Selecting this check box tells App Service that the setting isn't swappable.

  - There must be Deployment Setting (in any of the swapping slots) to enable `Swap with preview`

- Auto swap isn't currently supported in web apps on Linux and Web App for Containers.

To Note - You cannot delete slot when the traffic is configured to slot app. So to delete a Slot, Traffic should be 0 to the particular slot.
