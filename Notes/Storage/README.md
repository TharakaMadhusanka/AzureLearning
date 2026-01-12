- Containers and blobs also support certain standard HTTP properties.
- Properties and metadata are both represented as standard HTTP headers;the difference between them is in the naming of the headers.
- Metadata headers are named with the header prefix _x-ms-meta-_ and a custom name.
- Property headers use standard HTTP header names, as specified in the Header Field Definitions section 14 of the HTTP/1.1 protocol specification.
- Names are case-insensitive.

**The standard HTTP headers supported on containers include:**

- ETag
- Last-Modified

**The standard HTTP headers supported on blobs include:**

- ETag
- Last-Modified
- Content-Length
- Content-Type
- Content-MD5
- Content-Encoding
- Content-Language
- Cache-Control
- Origin
- Range

- A lifecycle management policy is a collection of rules in a JSON document.
- Each rule definition within a policy includes a filter set and an action set.

`{
  "rules": [
    {
      "name": "rule1",
      "enabled": true,
      "type": "Lifecycle",
      "definition": {...}
    },
    {
      "name": "rule2",
      "type": "Lifecycle",
      "definition": {...}
    }
  ]
}`

# rules

- An array of rule objects
- At least one rule is required in a policy.
- can define up to 100 rules in a policy
- Rule Params

  - Name | string | required | A rule name can include up to 256 alphanumeric characters. Rule name is case-sensitive. It must be unique within a policy.
  - enabled | boolean | optional | An optional boolean to allow a rule to be temporarily disabled. Default value is true.
  - type | enum | optional | current valid type is Lifecycle required
  - definition | an object that defines the lifecycle rule | each definition is made up of a filter set and action set | requried

# .NET Libraries

- Azure.Storage.Blobs: Contains the primary classes (client objects) that you can use to operate on the service, containers, and blobs.
- Azure.Storage.Blobs.Specialized: Contains classes that you can use to perform operations specific to a blob type, such as block blobs.
- Azure.Storage.Blobs.Models: All other utility classes, structures, and enumeration types.

- Block blob vs Page blob

| Feature             | **Block Blob**                 | **Page Blob**             |
| ------------------- | ------------------------------ | ------------------------- |
| Primary use         | Files, images, videos, backups | VHDs, disks, databases    |
| Read/write pattern  | **Sequential**                 | **Random access**         |
| Max size            | ~190.7 TB                      | **8 TB**                  |
| Data organization   | Blocks (up to 50,000)          | 512-byte pages            |
| Update part of blob | ❌ (re-upload block)           | ✅ (update page range)    |
| Append support      | ✅ (via Append Blob type)      | ❌                        |
| Used by Azure VMs   | ❌                             | ✅ (OS & data disks)      |
| Performance         | Optimized for throughput       | Optimized for low latency |

- Block Blob → “Big files, streaming”
- Page Blob → “Disks, random I/O”

![Different Client Libs](image.png)

- Can set metadata either using SDK or direct url

1. Using URL

   1. Setting metadata header on Container
      PUT https://myaccount.blob.core.windows.net/mycontainer?comp=metadata&restype=container
   2. Setting metadata header on Blob
      PUT https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=metadata

   3. Retrieving properties and metadata, Container
      GET/HEAD https://myaccount.blob.core.windows.net/mycontainer?restype=metadata
   4. Retrieving properties and metadata for Blob
      GET/HEAD https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=metadata

- generate SAS Token to access blob Container in . NET

`BlobSasBuilder sasBuilder = new BlobSasBuilder()
{
BlobContainerName = containerClient.Name,
Resource = "c"
};
sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
Uri sasUri = containerClient.GenerateSasUri(sasBuilder);`

// Resource
Specifies which resources are accessible via the shared access signature.
Specify "b" if the shared resource is a blob. This grants access to the content and metadata
of the blob. Specify "c" if the shared resource is a blob container. This grants
access to the content and metadata of any blob in the container, and to the list
of blobs in the container. Beginning in version 2018-11-09, specify "bs" if the
shared resource is a blob snapshot. This grants access to the content and metadata
of the specific snapshot, but not the corresponding root blob. Beginning in version
2019-12-12, specify "bv" if the shared resource is a blob version. This grants
access to the content and metadata of the specific version, but not the corresponding
root blob.

# Retention policy

1️⃣ Time-based Retention Policy

What it is:

- A time-based retention policy ensures that a blob cannot be deleted or modified for a specific period of time.

Key points:

- Applied per container or per blob (depending on configuration)
- Once a blob is written, it cannot be modified or deleted until the retention period expires
- Commonly used for compliance with regulations (financial, legal, healthcare)

## Example:

-Retention period = 30 days
-Blob created on Jan 1 → cannot delete/overwrite until Jan 31

Benefits:

- Protects data from accidental deletion
- Meets regulatory requirements like SEC 17a-4(f) or FINRA

Config:

- Configured in Azure portal, PowerShell, or ARM templates

2️⃣ Container-level WORM Policy (Write Once, Read Many)

What it is:

- WORM = Write Once, Read Many
- Once set at the container level, all blobs in the container are immutable until policy removal or expiration

Key points:

- Can enforce legal hold or time-based retention
- Prevents overwrite and deletion of any blob within the container
- Often combined with time-based retention

Example:

- Legal requirement: All audit logs must be retained for 7 years
- Apply container-level WORM policy → all blobs written are protected for 7 years

Benefits:

- Ensures full compliance across a container
- Simplifies management of retention policies

Config:

- Can enable WORM on a container with immutable storage policies

| Feature       | Time-based Retention               | Container-level WORM                               |
| ------------- | ---------------------------------- | -------------------------------------------------- |
| Scope         | Blob or container                  | Entire container                                   |
| Duration      | Defined period                     | Until policy removal or expiration                 |
| Purpose       | Protect individual blobs           | Enforce compliance for all blobs                   |
| Compliance    | Regulatory retention               | Regulatory or legal hold                           |
| Modifications | Cannot delete/modify during period | Cannot delete/modify any blob until policy expires |

## Pricing Tiers

1. Standard - general purpose V2
2. Premium -
   1. High performance
   2. using SSD
   3. Available 3 account types
      1. Page Blob
      2. Block blob
      3. file shares
   4. No noisy-neighbor problems
   5. Ultra-low latency
   6. Predictable throughput

### Standard General Purpose V2

- Supported Blob Storage including Data Lake, Queue Storage, Table Storage & Azure Files
- Support Redundancy, LRS, ZRS, RA-GRS, GRS, GZRS, RA-GZRS
- Usage:
  - Standard storage account type for blobs, file shares, queues, and tables.
  - Recommended for most scenarios using Azure Storage.
  - _If you want support for network file system (NFS) in Azure Files, use the premium file shares account type._

### Premium Storage

1. Premium block blobs

   - Supports Blob storage, Data Lake
   - LRS & ZRS
   - Usage:
     - Premium storage account type for block blobs and _append blobs._
     - Recommended for scenarios with high transaction rates or that use smaller objects or require consistently low storage latency.

2. Premium Page Blob

   1. Page Blobs only
   2. LRS & ZRS
   3. Premium storage account type for page blobs only.
   4. Usage: Virtual Hard Disks (VHD), rand read/ write data
   5. Premium Page Blobs are basically “cloud SSD hard drives” used to store VM disks and ultra-fast databases.

| Use Case                 | Why Premium Page Blob          |
| ------------------------ | ------------------------------ |
| Azure VM OS Disk         | Fast boot & I/O                |
| Azure VM Data Disk       | Databases, high IOPS workloads |
| SQL Server on Azure VM   | Stable 5,000+ IOPS             |
| SAP HANA on Azure VM     | Guaranteed throughput          |
| Large caching engines    | Very fast random updates       |
| Legacy VHD-based systems | Needs block-level disk access  |

3. Premium File Share
   1. Azure Files
   2. LRS & ZRS
   3. Usage:
      1. Premium storage account type for file shares only.
      2. Recommended for enterprise or high-performance scale applications.

| Blob Type     | Used For                                         |
| ------------- | ------------------------------------------------ |
| Block Blob    | Normal files, images, videos, backups            |
| Append Blob   | Logs                                             |
| **Page Blob** | Virtual hard disks (VHD), random read/write data |

- Performance Comparison

| Metric     | Hot Block Blob | Premium Page Blob |
| ---------- | -------------- | ----------------- |
| Latency    | 10–30 ms       | **<1 ms**         |
| IOPS       | ~500           | **5,000+**        |
| Throughput | ~60 MB/s       | **200+ MB/s**     |

## Encryption

- By default data store in a storage are encrypted by Microsoft Managed Keys.
- To manage excryption with own keys [Service Side Encryption],
  - customer-managed key - to use for encrypting and decrypting data in Blob Storage and in Azure Files.
    - Customer-managed keys must be stored in Azure Key Vault or Azure Key Vault Managed Hardware Security Model (HSM).
    - customer-provided key:
      - on Blob Storage operations.
      - A client can include an encryption key on a read/write request for granular control over how blob data is encrypted and decrypted.
  - Client-side encryption
    - The Blob Storage and Queue Storage client libraries uses AES in order to encrypt user data.
    - There are two versions of client-side encryption available in the client libraries:
      - Version 2 uses _Galois/Counter Mode (GCM) mode with AES_.
        - The Blob Storage and Queue Storage SDKs support client-side encryption with v2.
      - Version 1 uses Cipher Block Chaining (CBC) mode with AES.
        - _The Blob Storage, Queue Storage, and Table Storage_ SDKs support client-side encryption with v1.

## Lifecycle Policy

- Is a collection of rules in a JSON document.
- Each rule includes filter set and action set.
- The action set applies to the tier or delete actions to the filtered set of objects.
- Can define up to 100 rules in a policy.
- Applicable Filters:
  - blobTypes
    - Required
    - ex> block blob, page blob etc.
  - prefixMatch
    - Optional
    - An array of string for prefixes to be match.
    - Each rule can define up to 10 prefixes.
    - A prefix string must start with a container name
  - blobIndexMatch
    - An array of dictionary values consisting of blob index tag key and value conditions to be matched.
  - Actions
    1. tierToCool
       - Supported for block blob.
       - Supported for Snapshot
       - Supported for Previous Versions
    2. tierToCold
       - Supported for block blob
       - Supported for Snapshot
       - Supported for Previous Versions
    3. enableAutoTierToHotFromCool
       - Supported for block blob
       - Snapshot not supported
       - Previous Versions not supported
    4. tierToArchive
       - Supported for block blob
       - Supported Snapshot
       - Supported Previous Versions
    5. delete
       - supported for block blob & append blob
       - Supported for Snapshot
       - Supported for Previous Version
  - Action Run Conditions
    1. daysAfterModificationGreaterThan
       - integer value indicating the age in days
       - condition for base blob actions
    2. daysAfterCreationGreaterThan
       - integer value indicating the age in days
       - condition for blob snapshot actions
    3. daysAfterLastAccessTimeGreaterThan
       - integer value indicating the age in days
       - The condition for a current version of a blob when access tracking is enabled
    4. daysAfterLastTierChangeGreaterThan
       - Integer value indicating the age in days after last blob tier change time
       - The minimum duration in days that a rehydrated blob is kept in hot, cool, or cold tiers before being returned to the archive tier. This condition applies only to `tierToArchive` actions.

`Note: If you define more than one action on the same blob, lifecycle management applies the least expensive action to the blob. For example, action delete is cheaper than action tierToArchive. Action tierToArchive is cheaper than action tierToCool.`

### Base blobs use the last modified time to track age. Blob snapshots use the snapshot creation time to track age.

#### A lifecycle management policy must be read or written in full. Partial updates aren't supported.

Qn: What will happen if delete blob before minimum retain period from a tier?
