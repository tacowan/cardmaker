# 🚀 Getting Started with CardMaker

Welcome to **CardMaker**! This guide will help you get up and running with the AI-powered Platform Engineering card generator.

## 📋 Prerequisites

Before you begin, ensure you have:

- **.NET 8.0 SDK** or later installed
- **Azure OpenAI** service access with API keys
- **Windows environment** (the project targets `net9.0-windows`)
- **Terminal/Console** that supports Unicode characters

## 🔧 Installation & Setup

### Step 1: Clone the Repository
```bash
git clone https://github.com/tacowan/cardmaker.git
cd cardmaker
```

### Step 2: Configure Azure OpenAI Settings

CardMaker requires Azure OpenAI configuration. Set up your user secrets:

```bash
# Set your Azure OpenAI API key
dotnet user-secrets set "AzureOpenAISettings:ApiKey" "<your-api-key>"

# Set your Azure OpenAI endpoint  
dotnet user-secrets set "AzureOpenAISettings:Endpoint" "<your-endpoint>"

# Set your chat model deployment name
dotnet user-secrets set "AzureOpenAISettings:ChatModelDeployment" "gpt-4o"
```

> 💡 **Tip**: Your endpoint should look like `https://your-resource.openai.azure.com/`

### Step 3: Build the Project
```bash
dotnet build
```

### Step 4: Run CardMaker
```bash
dotnet run
```

## 🎮 Using CardMaker

### Interactive Chat Interface

Once CardMaker starts, you'll see a console interface where you can chat with the AI:

```
Creating store...
Card Maker: Hello! What are you good at and why are we here?
> 
```

### Basic Commands

- **Chat naturally**: Just type your requests in plain English
- **Generate cards**: Ask for specific suits or roles
- **Save cards**: The AI will use tools to save JSON files
- **Exit**: Type `EXIT` to quit the application

### Example Session

```
> Create a hearts suit focused on Platform Engineering tools

Card Maker: I'll create a hearts suit featuring essential Platform Engineering tools! 
Let me generate cards for Azure Kubernetes Service, GitHub Codespaces, 
Azure Developer CLI, and more...

> Save the hearts cards to a file

Card Maker: I'll save these cards using the save_cards tool...
File successfully created at /path/to/hearts_cards.json
```

## 🎴 Understanding Card Output

CardMaker generates JSON files with card specifications:

```json
{
  "cards": [
    {
      "card_id": 1,
      "suit": "hearts",
      "rank": "Ace",
      "role": "Platform Engineer",
      "tool": "Azure Kubernetes Service (AKS)",
      "description": "Managed service for deploying and managing Kubernetes clusters, ensuring scalability and reliability.",
      "quote": "All we have to do is decide what to do with the clusters that are given to us.",
      "image": "AKS-Automatic-Icon.png",
      "qr_code": "1.png"
    }
  ]
}
```

## 🛠️ Available Tools

The AI assistant has access to several tools:

| Tool | Description |
|------|-------------|
| `save_cards` | Save card specifications to JSON files |
| `copy_to_clipboard` | Copy text to system clipboard |
| `load_cards` | Load existing card files |
| `create_qr_code` | Generate QR codes linking to documentation |

## 🎨 Terminal Formatting

CardMaker uses Unicode escape codes for beautiful console output:
- **Bold headings** with background colors
- **Underlined** important text  
- **Bullet points** for lists
- **Emoticons** for engagement

## 🔍 What's Next?

- Check out the [**Project Overview**](project-overview.md) to understand the architecture
- Review [**Configuration Guide**](configuration.md) for advanced settings
- Explore [**Tools Reference**](tools-reference.md) for detailed tool documentation

Need help? Visit the [**Troubleshooting Guide**](troubleshooting.md)! 🆘