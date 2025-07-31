# 🔧 Tools Reference

CardMaker provides several AI-powered tools that the assistant can use during conversations. This reference documents all available tools and their functionality.

## 🎴 Core Card Tools

### save_cards
Saves a complete card suit to a local JSON file.

**Function Signature:**
```csharp
[KernelFunction("save_cards")]
public async Task<string> save_cards(string json_document, string suit)
```

**Parameters:**
- `json_document` (string): Complete JSON specification for all cards in the suit
- `suit` (string): Card suit name (hearts, clubs, spades, diamonds)

**Returns:**
- Success message with full file path

**Example Usage:**
```
> Save the hearts cards I just created

AI: I'll save the hearts suit using the save_cards tool...
File successfully created at /path/to/hearts_cards.json
```

**Output Format:**
```json
{
  "suit": "hearts",
  "theme": "Platform Engineering Tools",
  "cards": [
    {
      "card_id": 1,
      "rank": "Ace",
      "role": "Platform Engineer", 
      "tool": "Azure Kubernetes Service",
      "description": "Managed service for deploying...",
      "quote": "All we have to do is decide...",
      "image": "AKS-Automatic-Icon.png"
    }
  ]
}
```

---

### load_cards
Loads an existing card suit from a local JSON file.

**Function Signature:**
```csharp
[KernelFunction("load_cards")]
public async Task<string> load_card(string fileName)
```

**Parameters:**
- `fileName` (string): Name of the JSON file to load

**Returns:**
- Complete JSON content of the card file

**Example Usage:**
```
> Load the existing hearts cards

AI: I'll load the hearts cards file...
[JSON content displayed]
```

---

## 📋 Utility Tools

### copy_to_clipboard
Copies text content to the system clipboard for easy sharing.

**Function Signature:**
```csharp
[KernelFunction("copy_to_clipboard")]
public async Task<string> copy_to_clipboard(string text)
```

**Parameters:**
- `text` (string): Text content to copy to clipboard

**Returns:**
- Confirmation message

**Example Usage:**
```
> Copy the last card specification to my clipboard

AI: I'll copy that to your clipboard...
Text copied to clipboard.
```

**Supported Content:**
- JSON card specifications
- Individual card descriptions
- Quotes and text content
- Any string data

---

## 🔍 QR Code Tools

### create_qr_code
Generates QR codes that link to Microsoft Learn documentation searches.

**Function Signature:**
```csharp
[KernelFunction("create_qr_code")]
public async Task<string> create_qr_code(string query, int card_id)
```

**Parameters:**
- `query` (string): Search term for Bing documentation search
- `card_id` (int): Unique identifier for the QR code image file

**Returns:**
- Full file path to the generated QR code PNG image

**Generated URL Format:**
```
https://www.bing.com/search?q={query}+site:learn.microsoft.com&shm=cr&form=DEEPSH
```

**Example Usage:**
```
> Create a QR code for Azure Kubernetes Service documentation

AI: I'll create a QR code linking to AKS documentation...
QR code saved to /path/to/1.png
```

**QR Code Specifications:**
- **Format**: PNG image
- **Error Correction**: Q level (25% recovery)
- **Module Size**: 20 pixels per module
- **Colors**: Black on white background
- **Quiet Zone**: Enabled for better scanning

---

## 🎭 Persona Tools

### guest_quote (Semantic Kernel Function)
Generates Gandalf-inspired quotes related to Platform Engineering concepts.

**YAML Configuration:**
```yaml
name: guest_quote
template: |
  You are a quote generator pretending to be Gandalf, the wizard.
  You have vast knowledge of Platform Engineering practices.
  
  Significance of {{$role}} : {{$text}}
  Share a quote of no more than 60 words explaining how {{$role}} 
  achieves the goals of {{$suit_theme}}.
```

**Input Variables:**
- `card_number` (int): Card identifier
- `suit_theme` (string): Theme of the card suit
- `role` (string): Platform engineering role or tool
- `text` (string): Descriptive text about the role/tool

**Example Output:**
```
"All we have to do is decide what to do with the clusters that are given to us. 
The Platform Engineer, like a wise steward, tends to their Kubernetes realms 
with care and foresight."
```

---

## 🔨 Tool Integration Patterns

### Sequential Tool Usage
Tools can be chained together for complex workflows:

```
1. User: "Create a diamonds suit for DevOps tools"
2. AI generates card specifications
3. AI calls save_cards("diamonds", json_data)
4. AI calls create_qr_code("Azure Pipelines", 1)
5. AI calls copy_to_clipboard(summary)
```

### Error Handling
Tools include built-in error handling:

```csharp
try 
{
    await File.WriteAllTextAsync(filePath, json_document);
    return "File successfully created at " + fullPath;
}
catch (Exception ex)
{
    return $"Error saving file: {ex.Message}";
}
```

### File Path Management
All tools use consistent file path handling:

```csharp
var currentDirectory = Directory.GetCurrentDirectory();
var fullPath = Path.Combine(currentDirectory, fileName);
var absolutePath = Path.GetFullPath(fullPath);
```

---

## 🎨 Output Examples

### Card JSON Structure
```json
{
  "suit": "hearts",
  "theme": "Platform Engineering Tools", 
  "created": "2024-01-15T10:30:00Z",
  "cards": [
    {
      "card_id": 1,
      "rank": "Ace",
      "role": "Platform Engineer",
      "tool": "Azure Kubernetes Service (AKS)",
      "description": "Managed service for deploying and managing Kubernetes clusters, ensuring scalability and reliability.",
      "quote": "All we have to do is decide what to do with the clusters that are given to us. - Gandalf",
      "image": "AKS-Automatic-Icon.png",
      "qr_code": "1.png",
      "metadata": {
        "category": "Container Orchestration",
        "difficulty": "Intermediate",
        "documentation": "https://learn.microsoft.com/azure/aks/"
      }
    }
  ]
}
```

### QR Code Integration
Each card can include a QR code linking to relevant documentation:

- **Azure tools** → Microsoft Learn documentation
- **GitHub tools** → GitHub Docs
- **Open source tools** → Official project documentation

---

## 🚀 Extending Tools

### Adding Custom Tools

1. **Add to Tools.cs:**
```csharp
[KernelFunction("my_custom_tool")]
[Description("Description of what this tool does")]
public async Task<string> MyCustomTool(
    [Description("Parameter description")] string parameter)
{
    // Your implementation
    return "Result";
}
```

2. **Update Tools.txt resource** with tool description

3. **Register with Semantic Kernel** (automatically done via plugin loading)

### Tool Development Guidelines

- **Use descriptive names** that clearly indicate purpose
- **Add comprehensive descriptions** for AI understanding  
- **Include parameter descriptions** for proper usage
- **Return meaningful messages** for user feedback
- **Handle errors gracefully** with user-friendly messages
- **Follow async patterns** for non-blocking operations

### Integration with AI Agent

Tools are automatically available to the AI agent through:

```csharp
Tools tools = new();
kernel.Plugins.AddFromObject(tools);
```

The AI agent can:
- **Discover tools** through reflection and descriptions
- **Choose appropriate tools** based on user requests
- **Chain tool calls** for complex operations
- **Handle tool results** and format responses

Need help implementing custom tools? Check out the [**Project Overview**](project-overview.md) for architecture details! 🏗️