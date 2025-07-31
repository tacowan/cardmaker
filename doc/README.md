# 🃏 CardMaker Documentation

Welcome to the **CardMaker** project documentation! This AI-powered tool helps you create stunning Platform Engineering themed trading cards.

## 📚 Documentation Index

### 🚀 Getting Started
- [**Getting Started Guide**](getting-started.md) - Your first steps with CardMaker
- [**Configuration Guide**](configuration.md) - Setting up Azure OpenAI and user secrets

### 📖 Project Information  
- [**Project Overview**](project-overview.md) - Architecture and design philosophy
- [**Tools Reference**](tools-reference.md) - Available AI tools and functions

### 🛠️ Support
- [**Troubleshooting Guide**](troubleshooting.md) - Common issues and solutions

---

## 🎯 Quick Start

CardMaker is a console application that uses AI to help you create Platform Engineering themed playing cards. The cards feature tools, roles, and concepts from the platform engineering domain.

```bash
# 1. Configure your Azure OpenAI settings
dotnet user-secrets set "AzureOpenAISettings:ApiKey" "<your-api-key>"
dotnet user-secrets set "AzureOpenAISettings:Endpoint" "<your-endpoint>"  
dotnet user-secrets set "AzureOpenAISettings:ChatModelDeployment" "gpt-4o"

# 2. Run the application
dotnet run
```

## 🌟 What Makes CardMaker Special?

- **🤖 AI-Powered**: Leverages Azure OpenAI for intelligent card generation
- **🎨 Platform Engineering Themed**: Cards feature real tools and concepts
- **💾 JSON Output**: Generates data-merge ready specifications
- **🔧 Interactive Tools**: QR codes, clipboard integration, file operations
- **🎪 Console Magic**: Beautiful terminal formatting with Unicode escape codes

## 🎴 Card Output Example

CardMaker generates JSON specifications that can be used with design tools like Adobe Illustrator for data merging:

```json
{
  "card_id": 1,
  "suit": "hearts",
  "role": "Platform Engineer", 
  "tool": "Azure Kubernetes Service",
  "description": "Managed service for deploying and managing Kubernetes clusters",
  "quote": "Like a wise steward of the Shire, the Platform Engineer tends to their clusters..."
}
```

Ready to create your deck? Head over to the [**Getting Started Guide**](getting-started.md)! 🚀