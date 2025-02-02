## Introduction
The Model Command Line Interface (M-CLI) is a minimalist tool designed for developers and AI enthusiasts. It enables private 1:1 communication with an AI language model hosted in the cloud, while allowing local file access and saving, clipboard integration, and console-specific output.  This project is a specific application of an M-CLI that helps the user craft a deck of cards with a Platform Engineering theme.  The artifacts are JSON files with the card specifications.  These can be used in a data-merge with a visual template to generate printable images (ex. Illustrator).

## Features
- **Private Communication**: Interact with the AI model in a secure and private manner.
- **Local File Access and Saving**: Save artifacts generated by the model locally and retrieve them easily.
- **Clipboard Integration**: Seamlessly copy and paste text between the M-CLI and other applications.
- **Console-Specific Output**: Generate output optimized for console applications, including color-coded text and progress bars.
- **Minimal Dependencies**: Reduce dependencies to the bare necessities, ensuring a simple and efficient user experience.


## Configuration
This sample requires configuration setting in order to connect to remote services. You will need to define settings for Azure Open AI.
```powershell
// Generated by Copilot
dotnet user-secrets set "AzureOpenAISettings:ApiKey" "<api-key>" 
dotnet user-secrets set "AzureOpenAISettings:Endpoint" "<model-endpoint>"
dotnet user-secrets set "AzureOpenAISettings:ChatModelDeployment" "gpt-4o"
```
