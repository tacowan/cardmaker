# ⚙️ Configuration Guide

This guide covers all configuration aspects of CardMaker, from basic setup to advanced customization options.

## 🔐 Azure OpenAI Configuration

### Required Settings

CardMaker requires the following Azure OpenAI configuration:

| Setting | Description | Example |
|---------|-------------|---------|
| `ApiKey` | Your Azure OpenAI API key | `abc123...` |
| `Endpoint` | Your Azure OpenAI endpoint URL | `https://myresource.openai.azure.com/` |
| `ChatModelDeployment` | Deployment name for chat model | `gpt-4o` |

### Setting Up User Secrets

The recommended approach is using .NET User Secrets for secure configuration:

```bash
# Navigate to project directory
cd cardmaker

# Set API key
dotnet user-secrets set "AzureOpenAISettings:ApiKey" "your-actual-api-key-here"

# Set endpoint  
dotnet user-secrets set "AzureOpenAISettings:Endpoint" "https://your-resource.openai.azure.com/"

# Set model deployment
dotnet user-secrets set "AzureOpenAISettings:ChatModelDeployment" "gpt-4o"
```

### Alternative: Environment Variables

You can also use environment variables:

```bash
# Windows (Command Prompt)
set AzureOpenAISettings__ApiKey=your-api-key
set AzureOpenAISettings__Endpoint=https://your-resource.openai.azure.com/
set AzureOpenAISettings__ChatModelDeployment=gpt-4o

# Windows (PowerShell)
$env:AzureOpenAISettings__ApiKey="your-api-key"
$env:AzureOpenAISettings__Endpoint="https://your-resource.openai.azure.com/"
$env:AzureOpenAISettings__ChatModelDeployment="gpt-4o"

# Linux/macOS
export AzureOpenAISettings__ApiKey="your-api-key"
export AzureOpenAISettings__Endpoint="https://your-resource.openai.azure.com/"
export AzureOpenAISettings__ChatModelDeployment="gpt-4o"
```

## 🧠 AI Agent Configuration

### Model Parameters

The AI agent uses specific parameters for optimal card generation:

```csharp
new OpenAIAssistantDefinition(settings.AzureOpenAI.ChatModelDeployment)
{
    Name = "Card Maker",
    Instructions = cardMaker,          // Loaded from Agent_Prompt_v2.md
    Temperature = 0.4f,               // Lower = more consistent
    TopP = 0.9f,                      // Controls diversity
    EnableFileSearch = true,          // Vector store integration
    VectorStoreId = storeId          // Knowledge augmentation
}
```

### Customizing AI Behavior

#### Temperature (0.0 - 2.0)
- **0.0-0.5**: More focused, consistent responses
- **0.6-1.0**: Balanced creativity and consistency  
- **1.1-2.0**: More creative, varied responses

#### Top P (0.0 - 1.0)
- **0.1-0.5**: Conservative word choices
- **0.6-0.9**: Balanced vocabulary (recommended)
- **0.9-1.0**: Full vocabulary range

## 📝 Persona Customization

### Modifying the Gandalf Persona

Edit `Resources/Persona_Prompt.yaml`:

```yaml
name: guest_quote
template: |
  You are a quote generator pretending to be Gandalf, the wizard.
  You have vast knowledge of Platform Engineering practices.
  
  # Customize behavior here
  Write quotes in a {{$style}} manner about {{$topic}}.
  
template_format: semantic-kernel
input_variables:
  - name: style
    description: Quote style (wise, humorous, dramatic)
    is_required: false
  - name: topic  
    description: The subject matter
    is_required: true
execution_settings:
  default:
    temperature: 1.0    # Adjust for quote creativity
    TopP: 0.8          # Adjust for vocabulary range
```

### Adding Custom Personas

1. Create new YAML files in `Resources/`
2. Register in `Program.cs`:

```csharp
var customPersona = kernel.CreateFunctionFromPromptYaml(
    ContextUtils.EmbeddedResource("Custom_Persona.yaml")
);
kernel.Plugins.AddFromFunctions("custom_tool", [customPersona]);
```

## 🔧 Console Formatting

### Unicode Support

Ensure your terminal supports Unicode:

```csharp
// Set in Program.cs
Console.OutputEncoding = Encoding.UTF8;
```

### Custom Formatting

Modify `formatPrompt()` in `ContextUtils.cs`:

```csharp
public string formatPrompt()
{
    return $"""
    Please format your response using these codes:
    
    Use {B.fgCya}{B.bgBla}Cyan Headers{Fmt.clear} for special emphasis.
    Use {Fmt.i}Italic{Fmt._i} for quotes.
    Use {Fmt.strike}Strike{Fmt._strike} for deprecated items.
    
    Always respond using plain text, no markdown.
    """;
}
```

## 📁 File Output Configuration

### Default Output Locations

Cards are saved to the current working directory:

```csharp
string filePath = $"{suit}_cards.json";
await File.WriteAllTextAsync(filePath, json_document);
```

### Custom Output Directory

Modify the `save_cards` tool in `Tools.cs`:

```csharp
[KernelFunction("save_cards")]
public async Task<string> save_cards(
    string json_document, 
    string suit,
    [Description("Output directory")] string outputDir = "cards")
{
    Directory.CreateDirectory(outputDir);
    string filePath = Path.Combine(outputDir, $"{suit}_cards.json");
    await File.WriteAllTextAsync(filePath, json_document);
    return $"File saved to {Path.GetFullPath(filePath)}";
}
```

## 🖼️ QR Code Configuration

### QR Code Parameters

Customize QR code generation in `create_qr_code`:

```csharp
QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
QRCode qrCode = new QRCode(qrCodeData);
Bitmap qrCodeImage = qrCode.GetGraphic(
    pixelsPerModule: 20,     // Size of each module
    darkColor: Color.Black,  // Dark squares color
    lightColor: Color.White, // Light squares color
    drawQuietZone: true      // Add border
);
```

### Custom QR Code URLs

Modify the search URL template:

```csharp
// Current: Bing search with site filter
var url = $"https://www.bing.com/search?q={query}+site:learn.microsoft.com";

// Custom: Direct documentation links
var url = $"https://docs.microsoft.com/azure/{query}";

// Custom: Internal knowledge base
var url = $"https://your-company.com/docs/{query}";
```

## 🔊 Logging Configuration

### Enable Detailed Logging

Uncomment logging setup in `Program.cs`:

```csharp
var builder = Kernel.CreateBuilder();
builder.Services.AddSingleton(ContextUtils.getLoggerFactory());
```

### Configure Log Levels

Modify `getLoggerFactory()` in `ContextUtils.cs`:

```csharp
builder.SetMinimumLevel(LogLevel.Information);  // Change from LogLevel.None
```

### OpenTelemetry Tracing

Enable sensitive data logging:

```csharp
AppContext.SetSwitch(
    "Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", 
    true
);
```

## 🗂️ Vector Store Configuration

### Uploading Knowledge Documents

Uncomment and modify in `Program.cs`:

```csharp
Dictionary<string, OpenAIFile> fileReferences = [];
OpenAIFileClient fileClient = clientProvider.Client.GetOpenAIFileClient();

// Upload your custom knowledge base
Stream stream = File.OpenRead("your-knowledge-base.pdf");
OpenAIFile fileInfo = await fileClient.UploadFileAsync(
    stream, 
    "your-knowledge-base.pdf", 
    FileUploadPurpose.Assistants
);
await storeClient.AddFileToVectorStoreAsync(storeId, fileInfo.Id, waitUntilCompleted: true);
```

### Managing Vector Store Lifecycle

The application automatically cleans up resources, but you can customize:

```csharp
// Keep vector store for reuse
// Comment out cleanup in finally block:
// await storeClient.DeleteVectorStoreAsync(storeId),
```

## 🔍 Advanced Configuration

### Custom Tools

Add new tools by extending `Tools.cs`:

```csharp
[KernelFunction("your_custom_tool")]
[Description("Description of your tool")]
public async Task<string> YourCustomTool(
    [Description("Parameter description")] string parameter)
{
    // Your implementation
    return "Result";
}
```

### Model Selection

Change the model in `Program.cs`:

```csharp
builder.AddAzureOpenAIChatCompletion(
    deploymentName: "gpt-4o-2",      // Your deployment name
    apiKey: settings.AzureOpenAI.ApiKey,
    endpoint: settings.AzureOpenAI.Endpoint,
    modelId: "gpt-4o"                // Optional model override
);
```

Need help with configuration? Check the [**Troubleshooting Guide**](troubleshooting.md)! 🆘