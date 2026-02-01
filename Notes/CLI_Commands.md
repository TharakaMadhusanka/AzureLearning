1. To retrieve the programmings languages/ application stacks for a given App Service [Windows, Linux]

`az webapp list-runtimes --os-type <os>` ex: az webapp list-runtimes --os-type linux

2. To Set WebApp Environment Varible

`az webapp config appsettings set --resource-group <group-name> --name <app-name> --settings key1=value1 key2=value2`

3. To stream logs live in Cloud Shell

`az webapp log tail --name appname --resource-group myResourceGroup`

4. To create a container in a container group and mount an Azure file share as volume.

- The code segment that includes the `–azure-file-volume-mount-path` parameter and the `--azure-file-volume-share-name` parameter creates a container in a container group and mounts an Azure file share as volume.

`az container create -g MyResourceGroup --name myapp --image myimage:latest 
--command-line "cat /mnt/azfile/myfile"
--azure-file-volume-share-name myshare 
--azure-file-volume-account-name mystorageaccount 
--azure-file-volume-account-key mystoragekey 
--azure-file-volume-mount-path /mnt/azfile`

1. Delete an image with the tag from ACR

`az acr repository delete  --name devregistry   --image dev/nginx:latest   --yes`

6. Specify the `--restart-policy` parameter when you call `az container create`

`az container create --resource-group myResourceGroup --name mycontainer --image mycontainerimage --restart-policy OnFailure`

7. deploy the web app by using the Azure CLI.

`az webapp deploy`

8. Configure a web app cors, to allow access from anther website

`az webapp cors add -g MyResourceGroup -n MyWebApp --allowed-origins https://myapps.com`

9.  Set environment variable for web app

`az webapp config appsettings set --resource-group <group-name> --name <app-name> --settings key1=value1 key2=value2`

10. To stream logs live in Cloud Shell [Web App Streams Logs]

`az webapp log tail --name appname --resource-group myResourceGroup`

11. Build docker image in Azure

`az acr build`

12. Build task [Docker image]

`az acr task create`

13. To create container image with the environment variables

`az container create --resource-group myResourceGroup --name mycontainer2 --image mcr.microsoft.com azuredocs/aci-wordcount:latest --restart-policy OnFailure --environment-variables 'NumWords'='5' 'MinLength'='8'`

14. To deploy container with yml file
    `az container create --resource-group myResourceGroup  --file fileName.yaml`

15. Create Container App

`az containerapp create`

16. Update Container App

`az containerapp update`

`az containerapp update --name <APPLICATION_NAME> --resource-group <RESOURCE_GROUP_NAME> --image <IMAGE_NAME>`

17. list all container app revision

`az containerapp revision list`

18. to generate a new version of a key stored in Azure Key Vault.

`az keyvault key rotate --vault-name mykeyvault --name mykey`

The `Rotate` operation will generate a new version of the key based on the key policy. The `Rotation Policy` operation updates the rotation policy of a key vault key. The `Purge Deleted` Key operation is applicable for soft-delete enabled vaults or HSMs. The Set Attributes operation changes specified attributes of a stored key.

19. To add immutable policy to blob

`az storage container immutability-policy create \
    --account-name mystorage \
    --container-name mycontainer \
    --period 3650 \
    --allow-protected-append-writes true
`

20. To retrieve the list of outbound addresses web app, currently using

`az webapp show --resource-group <group_name> --name <app_name>  --query outboundIpAddresses --output tsv`

21. To find all the possible outbound ip addresses regardless of pricing tiers

`az webapp show --resource-group <group_name>  --name <app_name>  --query possibleOutboundIpAddresses --output tsv`

22. To assign user-managed identity to VM

diff - --assign-identity <created identity name>

`az vm create --resource-group myResourceGroup \ --name myVM --image win2016datacenter \ --generate-ssh-keys \ --assign-identity <created identity name> \ --role contributor \ --scope mySubscription \ --admin-username azureuser \ --admin-password myPassword12`

23. To assign system-managed identity to vm

diff - --assign-identity [With no Identity Name]

`az vm create --resource-group myResourceGroup \  --name myVM --image win2016datacenter \  --generate-ssh-keys \  --assign-identity \  --role contributor \ --scope mySubscription \ --admin-username azureuser \  --admin-password myPassword12`

24. deploy the container app using the Dockerfile.

`az webapp up \
  --name mydockerwebapp \
  --resource-group MyRG \
  --plan MyLinuxPlan \
  --location eastasia \
  --sku B1 \
  --source <dockerFilePath>`

25. to add partitions to the event hub.

`az eventhubs eventhub update --resource-group MyResourceGroupName --namespace-name MyNamespaceName --name MyEventHubName --partition-count 12`

26. Local Az CLI install

`winget install microsoft.azd`

27. Verify/ Check azd version (AZ CLI Local)

`azd version`

28. Bash Command

`SET APP_NAME=$(azd env get-value AZURE_FUNCTION_NAME)
func azure functionapp list-functions $APP_NAME --show-keys`

The `azd env get-value` command gets your function app name from the local environment.
When you use the `--show-keys` option with `func azure functionapp list-functions`, the returned Invoke URL: value for each endpoint includes a function-level access key.

29. Deploy Az Function in Az Powershell [Exam Qn]

`$resourceGroupName = "exampleRG"
$location = Read-Host -Prompt "Enter a supported Azure region"
$templateUri = "https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/quickstarts/microsoft.web/function-app-flex-managed-identities/azuredeploy.json"

New-AzResourceGroup -Name $resourceGroupName -Location "$location"
New-AzResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateUri $templateUri -functionAppRuntime "dotnet-isolated" -functionAppRuntimeVersion "8.0"

Read-Host -Prompt "Press [ENTER] to continue ..."`

in Az CLI

`read -p "Enter a supported Azure region: " location &&
resourceGroupName=exampleRG &&
templateUri="https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/quickstarts/microsoft.web/function-app-flex-managed-identities/azuredeploy.json" &&
az group create --name $resourceGroupName --location "$location" &&
az deployment group create --resource-group $resourceGroupName --template-uri  $templateUri --parameters functionAppRuntime=dotnet-isolated functionAppRuntimeVersion=8.0 &&
echo "Press [ENTER] to continue ..." &&
read`

30. View the storage queues in account [AZ CLI]

`set AZURE_STORAGE_CONNECTION_STRING="<MY_CONNECTION_STRING>"`
`az storage queue list --output tsv`

31. get message from queue

az storage message get --queue-name outqueue -o tsv --query [].{Message:content} > %TEMP%out.b64 && certutil -decode -f %TEMP%out.b64 %TEMP%out.txt > NUL && type %TEMP%out.txt && del %TEMP%out.b64 %TEMP%out.txt /q`

Note - After you execute `az storage message get`, the message is removed from the queue. If there was only one message in outqueue, you won't retrieve a message when you run this command a second time and instead get an error.

32. Create Resource Group

`az group create --location <location> --name <name>`

33. Create Storage Account

`az storage account create --name <name> --location <location> --sku <sku> `

34. Assign Role to Storage [follow the same for any resource]

`az role assignment create --assignee $userPrincipal \
    --role "Storage Blob Data Owner" \
    --scope $resourceID`

35. Get User Principal

`az rest --method GET --url https://graph.microsoft.com/v1.0/me \
    --headers 'Content-Type=application/json' \
    --query userPrincipalName --output tsv`

36. Get ResourceId

`az storage account show --name stkalum --resource-group myresourcegroup2 --query id`

###### az storage account show --name stkalum --resource-group myresourcegroup2 - this is to get complete information list for the storage account

37. Get Permission list in App

`az ad app permission list --id <appId>`

38. Assign Virtual Machine Contributor role to enable system-assigned managed identity during creation of virtual machine

`az vm create --resource-group myResourceGroup \ 
    --name myVM --image win2016datacenter \ 
    --generate-ssh-keys \ 
    --assign-identity \ 
    --role contributor \
    --scope mySubscription \
    --admin-username azureuser \ 
    --admin-password myPassword12`

39. To assign system-assigned managed identity to existing VM

`az vm identity assign -g myResourceGroup -n myVm`

40. Create a user-assigned managed identity

`az identity create -g myResourceGroup -n myUserAssignedIdentity`

41. Assign a user-assigned managed identity during the creation of an Azure virtual machine

`az vm create \
--resource-group <RESOURCE GROUP> \
--name <VM NAME> \
--image Ubuntu2204 \
--admin-username <USER NAME> \
--admin-password <PASSWORD> \
--assign-identity <USER ASSIGNED IDENTITY NAME> \
--role <ROLE> \
--scope <SUBSCRIPTION>`

42. Assign a user-assigned managed identity to an existing Azure virtual machine

`az vm identity assign -g <RESOURCE GROUP> -n <VM NAME> --identities <USER ASSIGNED IDENTITY>`

43. Register EventGrid global namespace

`az provider register --namespace Microsoft.EventGrid`

44. To view the registration status

`az provider register --namespace Microsoft.EventGrid --query "registrationState"` // registrationState is case-sensitive

45. To create eventgrid topic

`az eventgrid topic create --name <name> --location <location> --resource-group <resourceGroup>`

46. To create subscription to Topic

`az eventgrid event-subscription create  --source-resource-id $topicId --name TopicSubscription --endpoint $endpoint`

47. Get Topic access keys list

`az eventgrid topic key list --name <topicName> --resource-group <resourceGroup>`

48. Get Topic Details
    `az eventgrid topic show --name <topicName> --resource-group <resourceGroup>`

49. Create Azure Event Hub

`az eventhubs namespace create --name <name> --resource-group <rgName> --location <location>`

50. Create EventHub

`az eventhubs eventhub create --name <name> --resource-group <rgName> --namepsace-name <namespace>`

51. Create Service Bus

`az servicebus namespace create --name mynamespace --resource-group az204tharaka`

52. Create Azure Service Bus Queue

`az servicebus queue create --resource-group <RG>  --namespace-name <namespace> --name <name>`

53. Create Azure Container Registry

`az acr create --name <name> --resource-group <rgName> --sku <pricingPlan>`

54. To create DockerFile from image

`echo FROM <imageregistryurl> > DockerFile`

55. Build Docker Image

`az acr build --image <imageName> --registry <registryName> --file <DockerFile> .`

56. List down repositories

`az acr repository list --name <registryName> --output table`

57. List tags

`az acr repository show-tags --name <registryName> --repository <repoName> --output table`

58. To run Container

`az acr  run --registry <registryName> --cmd '$Registry/sample/helloworld:v1' /dev/null`

### Container Instance

59. Create Instance

`az container create --name <name> --image <imageUrl> --resource-group <resourceGroup> --ports <ports> --dns-name-label <dnsNameLabel> --location <location> --os-type <osType> --cpu <cpu> --memory <memory>`

// Here required to have Admin Creds in Container Registry to pull image From

60. To verify the container Status

`az container show --resource-group myResourceGroup --name mycontainer --query "{FQDN:ipAddress.fqdn,ProvisioningState:provisioningState}" --out table `

61. To Mount Azure Fileshare to Container Instance

`az container create \
 --resource-group $ACI*PERS_RESOURCE_GROUP \
 --name hellofiles \
 --image mcr.microsoft.com/azuredocs/aci-hellofiles \
 --dns-name-label aci-demo \
 --ports 80 \
 *--azure-file-volume-account-name $ACI*PERS_STORAGE_ACCOUNT_NAME* \

- --azure-file-volume-account-key $STORAGE_KEY* \
   *--azure-file-volume-share-name $ACI_PERS_SHARE_NAME* \
   *--azure-file-volume-mount-path\* /aci/logs/` 62. Create Azure Container App 1. Check the Container App Extension is the latest or not

  `az extension add --name containerapp --upgrade`

          2. Register Namespace

  `az provider register --namespace Microsoft.App`

62. Create Az Container App Environment

`az containerapp create --resource-group <resourceGroup> --name <containerAppName>`

63. Deploy image to container app

`az containerapp create --name <name> --resource-group <rg> --environment <ContainerAppEnv> --image <containerImage> --target-port <port> --ingress <internal | external> `

64. To create Cosmos db

`az cosmosdb create --name <accntName> --resource-group <rgName>`

65. To view the account details

`az cosmosdb show --name $accountName --resource-group $resourceGroup --query "documentEndpoint" --output tsv`

66. Get Cosmosdb Keys

`az cosmosdb keys list --name $accountName --resource-group $resourceGroup --query "primaryMasterKey" --output tsv`
