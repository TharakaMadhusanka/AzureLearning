- Serverless Solution
- Two types
  - trigger - ways to start execution of your code
  - bindings - ways to simplify coding for input and output data

[Important]

- Azure Functions is a serverless compute service
- Azure Logic Apps is a serverless workflow integration platform
- Both can create complex orchestrations. An orchestration is a collection of functions or steps, called actions in Logic Apps, that are executed to accomplish a complex task.
- For Azure Functions, you develop orchestrations by writing code and using the Durable Functions extension. For Logic Apps, you create orchestrations by using a GUI or editing configuration files.

![Azure Functions vs Logic Apps](image-17.png)

- Azure Functions is built on the WebJobs SDK

![Azure Functions vs Web Jobs](image-18.png)

- Azure Functions Hosting Plan

![Azure Functions Hosting Plan](image-19.png)

[Important]

- In the Consumption plan, Azure Functions billing is based on:
  - Number of executions
  - Execution duration × memory (GB-seconds)

This statement means:

- Cost is dominated by execution count, not compute usage
- Each execution is cheap in compute, but too many executions add up

- functionTimeout

  - property in host.json file
  - specifies the time-out duration for functions in a function app.
  - This property applies specifically to function executions.
  - After the trigger starts function execution, the function needs to return/respond within the time-out duration

- Things to note

  - Regardless of the function app time-out setting, _230 seconds_ is the maximum amount of time that an _HTTP triggered function_ can take to respond to a request.
    - This is because of the default idle time-out of Azure Load Balancer.

- Default timeout in mins
  ![Default timeout in mins](image-20.png)

2. There's no maximum execution time-out duration enforced. However, the grace period given to a function execution is 60 minutes during scale in for the Flex Consumption and Premium plans, and a grace period of 10 minutes is given during platform updates.
3. Requires the App Service plan be set to Always On. A grace period of 10 minutes is given during platform updates.
4. The default time-out for version 1.x of the Functions host runtime is unbounded.
5. When the minimum number of replicas is set to zero, the default time-out depends on the specific triggers used in the app.

- Function Apps Scaling

![Function Apps Scaling](image-21.png)

1. During scale-out, there's currently a limit of 500 instances per subscription per hour for Linux apps on a Consumption plan.
1. In some regions, Linux apps on a Premium plan can scale to 100 instances.
1. For specific limits for the various App Service plan options, see the App Service plan limits.
1. On Container Apps, you can set the maximum number of replicas, which is honored as long as there's enough cores quota available

- Execution Context | the unit of deployment and management for your functions.
- Trigger & Bindings

  - Every trigger or binding declares whether data flows into the function or out of the function.
  - Binding direction
    - For triggers, the direction is always in
      - Input and output bindings use in and out
      - Some bindings support a special direction inout. If you use inout, only the Advanced editor is available via the Integrate tab in the portal.

- An app running in a Consumption or Elastic Premium plan, uses the _WEBSITE_AZUREFILESCONNECTIONSTRING_ and _WEBSITE_CONTENTSHARE_ settings when connecting to Azure Files on the storage account used by your function app.
- Azure Files _doesn't_ support using managed identity when accessing the file share.

- Bindings vs Triggers

| Aspect             | Trigger                    | Binding               |
| ------------------ | -------------------------- | --------------------- |
| Purpose            | Starts the function        | Moves data in/out     |
| Count              | Exactly **1 per function** | **Multiple allowed**  |
| Direction          | Always `in`                | `in` or `out` or both |
| Required           | Yes                        | No                    |
| Controls execution | ✅                         | ❌                    |

Ex:

1. Trigger starts function, Binding fetches extra data
   `public static void Run(
    [QueueTrigger("orders")] string orderId,
    [Blob("configs/settings.json", FileAccess.Read)] string config)
{
}`

Bindings helps to connect to other azure resource without explicitly integrating. Ex: when queue trigger run retrive a given blob data also in to the function.

- In some cases, when trying to create a new hosting plan for your function app in an existing resource group you might receive one of the following errors:

      - The pricing tier isn't allowed in this resource group
      - <SKU_name> workers aren't available in resource group <resource_group_name>

- Reason for these is,
  - You create a function app in an existing resource group that has yet to contain another function app or web app. For example, Linux Consumption apps aren't supported in the same resource group as Linux Dedicated or Linux Premium plans.
  - Your new function app is created in the same region as the previous app.
  - The previous app is in some way incompatible with your new app. This incompatibility can occur between versions, operating systems, or is due to other platform-level features, such as availability zone support.

# Durable Functions

- to write stateful functions in a serverless environment.
- define stateful workflows by writing orchestrator functions
- define stateful entities by entity functions
- The primary use case for Durable Functions is simplifying complex, stateful coordination requirements in serverless applications.

Durable Function Patterns

1. Function chaining
2. Fan-out/fan-in
3. Http Trigger Asyn (default in AZ Function Apps)
4. Monitor
