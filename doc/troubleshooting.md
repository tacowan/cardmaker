# 🆘 Troubleshooting Guide

Having issues with CardMaker? This guide covers common problems and their solutions.

## 🚨 Common Issues

### Build and Runtime Issues

#### ❌ .NET SDK Version Mismatch
**Error:**
```
error NETSDK1045: The current .NET SDK does not support targeting .NET 9.0
```

**Solution:**
1. Check your .NET SDK version:
```bash
dotnet --list-sdks
```

2. If you only have .NET 8.0, modify `cardmaker.csproj`:
```xml
<TargetFramework>net8.0-windows</TargetFramework>
```

3. Or install .NET 9.0 SDK from [Microsoft's download page](https://dotnet.microsoft.com/download)

---

#### ❌ Missing Azure OpenAI Configuration
**Error:**
```
System.InvalidOperationException: Unable to bind to type 'AzureOpenAISettings'
```

**Solution:**
Ensure all required settings are configured:

```bash
# Check current user secrets
dotnet user-secrets list

# Set missing values
dotnet user-secrets set "AzureOpenAISettings:ApiKey" "your-key"
dotnet user-secrets set "AzureOpenAISettings:Endpoint" "your-endpoint"  
dotnet user-secrets set "AzureOpenAISettings:ChatModelDeployment" "gpt-4o"
```

---

#### ❌ Console Encoding Issues
**Problem:** Unicode characters not displaying correctly

**Solution:**
1. **Windows Terminal/PowerShell:** Already supports UTF-8
2. **Command Prompt:** Use `chcp 65001` before running
3. **VS Code Terminal:** Set terminal encoding to UTF-8

---

### Azure OpenAI Issues

#### ❌ Authentication Failures
**Error:**
```
Unauthorized: Access denied due to invalid authentication token
```

**Solutions:**
1. **Verify API Key:**
   - Check Azure Portal → OpenAI Service → Keys and Endpoint
   - Ensure you're using the correct key

2. **Check Endpoint Format:**
   ```
   ✅ Correct: https://your-resource.openai.azure.com/
   ❌ Wrong: https://your-resource.openai.azure.com/openai/
   ```

3. **Validate Deployment Name:**
   - Must match exactly what's configured in Azure
   - Case-sensitive

---

#### ❌ Rate Limiting
**Error:**
```
Too Many Requests: Rate limit exceeded
```

**Solutions:**
1. **Check Quota:** Azure Portal → OpenAI Service → Quotas
2. **Reduce Usage:** Lower conversation frequency
3. **Upgrade Tier:** Consider higher rate limits

---

#### ❌ Model Deployment Issues
**Error:**
```
The model 'gpt-4o' was not found
```

**Solutions:**
1. **Verify Deployment:** Azure Portal → OpenAI Service → Deployments
2. **Check Model Name:** Must match deployment name exactly
3. **Update Configuration:**
   ```bash
   dotnet user-secrets set "AzureOpenAISettings:ChatModelDeployment" "your-actual-deployment-name"
   ```

---

### File Operations Issues

#### ❌ Permission Denied
**Error:**
```
UnauthorizedAccessException: Access to the path is denied
```

**Solutions:**
1. **Run as Administrator** (Windows)
2. **Check File Permissions** (Linux/macOS):
   ```bash
   chmod 755 /path/to/cardmaker
   ```
3. **Use Different Directory:**
   ```csharp
   string outputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "cardmaker");
   ```

---

#### ❌ File Already Exists
**Problem:** Cannot overwrite existing card files

**Solution:**
Modify `save_cards` tool to handle overwrites:
```csharp
if (File.Exists(filePath))
{
    string backup = $"{filePath}.backup.{DateTime.Now:yyyyMMdd-HHmmss}";
    File.Move(filePath, backup);
}
```

---

### Network and Connectivity

#### ❌ Vector Store Creation Timeout
**Error:**
```
TimeoutException: The operation timed out
```

**Solutions:**
1. **Check Internet Connection**
2. **Retry Later:** Azure service might be busy
3. **Disable Vector Store:** Comment out vector store code temporarily

---

#### ❌ QR Code Generation Fails
**Error:**
```
System.Drawing is not supported on this platform
```

**Solution (Linux/macOS):**
```bash
# Install required packages
sudo apt-get install libgdiplus  # Ubuntu/Debian
brew install mono-libgdiplus      # macOS
```

---

## 🔧 Debugging Tips

### Enable Detailed Logging

1. **Uncomment logging in Program.cs:**
```csharp
builder.Services.AddSingleton(ContextUtils.getLoggerFactory());
```

2. **Set log level in ContextUtils.cs:**
```csharp
builder.SetMinimumLevel(LogLevel.Information);
```

3. **Enable sensitive data logging:**
```csharp
AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);
```

### Trace Network Requests

Monitor HTTP traffic to debug Azure OpenAI issues:

```csharp
// Add to Program.cs
using var httpClientHandler = new HttpClientHandler()
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
};

// Enable request/response logging
httpClientHandler.Properties["TraceRequests"] = true;
```

### Test Configuration

Create a minimal test to verify settings:

```csharp
var settings = new Settings();
Console.WriteLine($"Endpoint: {settings.AzureOpenAI.Endpoint}");
Console.WriteLine($"Deployment: {settings.AzureOpenAI.ChatModelDeployment}");
Console.WriteLine($"API Key Length: {settings.AzureOpenAI.ApiKey?.Length ?? 0}");
```

---

## 📊 Performance Issues

### Slow Response Times

**Causes:**
- Large vector store files
- High model temperature
- Complex prompts

**Solutions:**
1. **Optimize Prompts:** Make them more specific
2. **Reduce Temperature:** Lower to 0.1-0.3 for faster responses
3. **Remove Vector Store:** Comment out file upload code

### Memory Usage

**High Memory Consumption:**

1. **Monitor Usage:**
```csharp
GC.Collect();
var memoryUsage = GC.GetTotalMemory(false);
Console.WriteLine($"Memory: {memoryUsage / 1024 / 1024} MB");
```

2. **Dispose Resources:**
```csharp
using var httpClient = new HttpClient();
// Resources automatically disposed
```

---

## 🔍 Advanced Debugging

### Semantic Kernel Diagnostics

Enable detailed SK logging:

```csharp
var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(resourceBuilder)
    .AddSource("Microsoft.SemanticKernel*")
    .AddConsoleExporter()
    .SetSampler(new AlwaysOnSampler())  // Capture all traces
    .Build();
```

### OpenAI API Response Analysis

Log raw API responses:

```csharp
public async Task LogApiResponse(HttpResponseMessage response)
{
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine($"Status: {response.StatusCode}");
    Console.WriteLine($"Content: {content}");
    Console.WriteLine($"Headers: {string.Join(", ", response.Headers)}");
}
```

### Tool Execution Tracing

Add logging to tool methods:

```csharp
[KernelFunction("save_cards")]
public async Task<string> save_cards(string json_document, string suit)
{
    Console.WriteLine($"[DEBUG] Saving {json_document.Length} chars to {suit}_cards.json");
    
    try 
    {
        string filePath = $"{suit}_cards.json";
        await File.WriteAllTextAsync(filePath, json_document);
        
        Console.WriteLine($"[DEBUG] Successfully saved to {Path.GetFullPath(filePath)}");
        return "File successfully created at " + Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + filePath;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] Save failed: {ex.Message}");
        throw;
    }
}
```

---

## 📞 Getting Additional Help

### Check Project Resources
- [**Project Overview**](project-overview.md) - Architecture details
- [**Configuration Guide**](configuration.md) - Setup instructions
- [**Tools Reference**](tools-reference.md) - Function documentation

### Azure OpenAI Resources
- [Azure OpenAI Documentation](https://docs.microsoft.com/azure/cognitive-services/openai/)
- [Azure OpenAI Pricing](https://azure.microsoft.com/pricing/details/cognitive-services/openai-service/)
- [Azure Support](https://azure.microsoft.com/support/)

### .NET Resources  
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [Semantic Kernel Documentation](https://learn.microsoft.com/semantic-kernel/)
- [.NET Support Forums](https://docs.microsoft.com/answers/topics/dotnet.html)

### Community Support
- Create an issue in the GitHub repository
- Check existing issues for similar problems
- Share detailed error messages and environment info

---

## 📝 Reporting Issues

When reporting issues, please include:

1. **Environment Information:**
   ```bash
   dotnet --version
   dotnet --list-sdks
   ```

2. **Error Messages:** Full stack traces

3. **Configuration:** Sanitized settings (remove API keys)

4. **Steps to Reproduce:** Detailed reproduction steps

5. **Expected vs Actual Behavior**

Still stuck? Don't hesitate to reach out for help! 🙋‍♂️