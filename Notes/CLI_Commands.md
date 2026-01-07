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
