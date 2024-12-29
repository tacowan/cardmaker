
## Configuration
This sample requires configuration setting in order to connect to remote services. You will need to define settings for Azure Open AI.
```powershell
// Generated by Copilot
dotnet user-secrets set "AzureOpenAISettings:ApiKey" "<api-key>" 
dotnet user-secrets set "AzureOpenAISettings:Endpoint" "<model-endpoint>"
dotnet user-secrets set "AzureOpenAISettings:ChatModelDeployment" "gpt-4o"
```
