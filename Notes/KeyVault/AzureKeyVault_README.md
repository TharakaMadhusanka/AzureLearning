- Azure Key Vault service supports 2 types of containers -

  1. Vault
  2. Managed hardware security module(HSM) pools

- Vaults support storing software and HSM-backed keys, secrets, and certificates.
- Managed HSM pools only support HSM-backed keys.

- Azure Key Vault diagnostic logs and metrics can be configured to:

        * Archive to a storage account.
        * Stream to an event hub.
        * Send the logs to Azure Monitor logs.

- Authentication [3 ways to Authenticate to Key Vault]

  1. Managed Identities for Azure resources [Recommended]
  2. service principal and certificate
  3. service principal and secret

- Azure Key Vault enforces Transport Layer Security (TLS) protocol to protect data when it’s traveling between Azure Key Vault and clients.
- Perfect Forward Secrecy (PFS) protects connections between customers’ client systems and Microsoft cloud services by unique keys.

- Authentication with Key Vault works with Microsoft Entra ID, which is responsible for authenticating the identity of any given security principal.
  A security principal is anything that can request access to Azure resources. This includes:

            - Users – Real people with accounts in Microsoft Entra ID.
            - Groups – Collections of users. Permissions given to the group apply to all its members.
            - Service Principals – Represent apps or services (not people). Think of it like a user account for an app.

- For applications, there are two main ways to obtain a service principal:

  1. Use a managed identity (recommended): Azure creates and manages the service principal for you. The app can securely access other Azure services without storing credentials. Works with services like App Service, Azure Functions, and Virtual Machines.

  2. Register the app manually: You register the app in Microsoft Entra ID. This creates a service principal and an app object that identifies the app across all tenants.

- When an access token isn't supplied, or when the service rejects a token, an HTTP 401 error is returned to the client and includes the WWW-Authenticate header | WWW-Authenticate: Bearer authorization="…", resource="…"

  - authorization: The address of the OAuth2 authorization service that might be used to obtain an access token for the request.
  - resource: The name of the resource (https://vault.azure.net) to use in the authorization request.

- Types of Managed Identities
  - system-assigned managed identity
    - When the identity is enabled, Azure creates an identity for the instance in the Microsoft Entra tenant trusted by the subscription of the instance.
    - Internally, managed identities are service principals of a special type, which are locked to only be used with Azure resources.
  - user assigned managed identity

![System-Assigned Managed vs User-Assigned](image-5.png)

- common use cases for managed identities:

  _System-assigned managed identity_

  - Workloads contained within a single Azure resource.
  - Workloads needing independent identities.
  - For example, an application that runs on a single virtual machine.

* User-assigned managed identity\*

  - Workloads that run on multiple resources and can share a single identity.
  - Workloads needing preauthorization to a secure resource, as part of a provisioning flow.
  - Workloads where resources are recycled frequently, but permissions should stay consistent.
  - For example, a workload where multiple virtual machines need to access the same resource.

- Managed Identity = Azure creates an identity for the VM, issues tokens via IMDS, and your code uses the token to access Azure services — no secrets. [Ex. VM]

System-Assigned Managed Identity (VM-bound)
📌 Remember as: Enable → Auto-Create → Auto-Delete
Steps (Condensed)

    1️⃣ Enable
        ARM receives request to enable identity on VM

    2️⃣ Create (Auto)
        ARM creates Service Principal in Microsoft Entra ID
        Identity is tied to this VM

    3️⃣ Configure
        ARM updates IMDS with:
            Client ID
            Certificate

    4️⃣ Authorize
        Assign RBAC role to VM identity
        ARM → RBAC role
        Key Vault → Access policy / RBAC

    5️⃣ Token
        VM code calls IMDS endpoint
        169.254.169.254/metadata/identity/oauth2/token

    6️⃣ Use
        Entra ID issues JWT
        Token is sent to Azure service

    🧠 Exam tip:

    Delete VM = identity deleted automatically


    User-Assigned Managed Identity (Reusable)

📌 Remember as: Create → Attach → Reuse
Steps (Condensed)

    1️⃣ Create
        ARM creates User-Assigned Managed Identity
        Service Principal created in Entra ID

    2️⃣ Attach
        ARM attaches identity to VM
        IMDS updated with:
            Client ID
            Certificate

    3️⃣ Authorize (can be before or after attach)
        Assign RBAC roles to identity
        Grant Key Vault access if needed

    4️⃣ Token
        VM code calls IMDS endpoint
        Specifies client_id if multiple identities exist

    5️⃣ Use
        Entra ID returns JWT
        Token used to access Azure service

    🧠 Exam tip:

    Identity survives even if VM is deleted

![System assigned vs User Assigned](image-6.png)

- You can configure an Azure resource with a managed identity during, or after, the creation of the resource.

- To create, or enable, an Azure virtual machine with the system-assigned managed identity your account needs the Virtual Machine Contributor role assignment. No other Microsoft Entra directory role assignments are required.
