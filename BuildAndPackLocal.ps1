# Publish as self-contained
dotnet publish .\src\Seq.Input.AzureEventHub\Seq.Input.AzureEventHub.csproj --self-contained --output .\obj\publish\

# Pack all that stuff
dotnet pack .\src\Seq.Input.AzureEventHub\Seq.Input.AzureEventHub.csproj --version-suffix "pre-20190316-001"
