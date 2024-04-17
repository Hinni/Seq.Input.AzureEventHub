$appName = "Seq.Input.Azure.EventHub"

echo "run: Publishing app binaries"

& dotnet publish "$PSScriptRoot/src/$appName" -c Release -o "$PSScriptRoot/src/$appName/obj/publish" --version-suffix=local

if($LASTEXITCODE -ne 0) { exit 1 }    

& seqcli app run -d "$PSScriptRoot/src/$appName/obj/publish" `
        -p EventHubConnectionString=x `
        -p EventHubName=x `
        -p ConsumerGroupName=x `
        -p StorageConnectionString=x `
        -p StorageContainerName=x `
        2>&1 | `
    & seqcli print
    