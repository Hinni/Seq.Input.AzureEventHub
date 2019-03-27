# Publish as self-contained
dotnet publish .\src\Seq.Input.AzureEventHub\Seq.Input.AzureEventHub.csproj --framework netstandard2.0 --self-contained --output .\obj\publish\netstandard2.0
dotnet publish .\src\Seq.Input.AzureEventHub\Seq.Input.AzureEventHub.csproj --framework net462 --self-contained --output .\obj\publish\net462

# Pack all that stuff
dotnet pack .\src\Seq.Input.AzureEventHub\Seq.Input.AzureEventHub.csproj --version-suffix "pre-20190327-001"
