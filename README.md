# Azure Live Translation Demo

This application demonstrates Azure's Live Translation capabilities by translating Japanese audio input to German in real-time and using text-to-speech to speak out the translation.

## Features

- **Real-time Speech Recognition**: Recognizes Japanese speech from microphone input
- **Live Translation**: Translates Japanese to German on-the-fly while audio is streaming
- **Text-to-Speech Output**: Converts translated German text to speech using Azure's neural voices
- **Continuous Recognition**: Processes audio continuously until stopped

## Prerequisites

1. **Azure Speech Service**: You need an Azure subscription with Speech Service enabled
   - Create a Speech Service resource in Azure Portal
   - Note your Speech Service key and region

2. **.NET 8.0 SDK**: Required to build and run the application
   - Download from: https://dotnet.microsoft.com/download

## Setup

### 1. Clone the Repository

```bash
git clone https://github.com/abeckDev/DB-LiveTranslationDemo.git
cd DB-LiveTranslationDemo
```

### 2. Set Environment Variables

Set your Azure Speech Service credentials as environment variables:

**Linux/macOS:**
```bash
export AZURE_SPEECH_KEY=your_speech_service_key_here
export AZURE_SPEECH_REGION=your_region_here
```

**Windows (PowerShell):**
```powershell
$env:AZURE_SPEECH_KEY="your_speech_service_key_here"
$env:AZURE_SPEECH_REGION="your_region_here"
```

**Windows (Command Prompt):**
```cmd
set AZURE_SPEECH_KEY=your_speech_service_key_here
set AZURE_SPEECH_REGION=your_region_here
```

Replace:
- `your_speech_service_key_here` with your actual Azure Speech Service key
- `your_region_here` with your Azure region (e.g., `westeurope`, `eastus`, `westus2`)

### 3. Build the Application

```bash
cd LiveTranslationDemo
dotnet build
```

### 4. Run the Application

```bash
dotnet run
```

## Usage

1. Run the application using `dotnet run`
2. Wait for the "Listening... Press Enter to stop." message
3. Speak in Japanese into your microphone
4. The application will:
   - Display the recognized Japanese text
   - Show the German translation
   - Speak the German translation through your speakers
5. Press Enter to stop the application

## How It Works

The application uses Azure Cognitive Services Speech SDK to:

1. **Speech Recognition**: Captures audio from the default microphone and recognizes Japanese speech using the `ja-JP` language model

2. **Real-time Translation**: Translates the recognized Japanese text to German (`de`) as the audio streams

3. **Text-to-Speech Synthesis**: Converts the translated German text to natural-sounding speech using the `de-DE-KatjaNeural` voice

4. **Continuous Processing**: All of this happens continuously and in real-time while you speak

## Technical Details

- **Source Language**: Japanese (`ja-JP`)
- **Target Language**: German (`de`)
- **TTS Voice**: Katja (Neural) - `de-DE-KatjaNeural`
- **Framework**: .NET 8.0
- **SDK**: Microsoft.CognitiveServices.Speech 1.47.0

## Example Output

```
Azure Live Translation Demo
===========================
Translating Japanese audio to German with Text-to-Speech output

Session started.
Speak in Japanese and press Enter when done...

Listening... Press Enter to stop.

RECOGNIZING in Japanese: こんにちは
    Translating to de: Hallo

RECOGNIZED in Japanese: こんにちは
    TRANSLATED to de: Hallo
    SPEAKING: Hallo
    Speech synthesis completed.
```

## Troubleshooting

### No audio input detected
- Check your microphone permissions
- Ensure your default microphone is properly configured
- Test microphone in other applications

### "Speech key and region" error
- Verify environment variables are set correctly
- Check your Azure Speech Service key is valid
- Confirm the region matches your Azure resource

### Build errors
- Ensure .NET 8.0 SDK is installed: `dotnet --version`
- Try `dotnet restore` before building

## References

- [Azure Speech Service Documentation](https://learn.microsoft.com/azure/ai-services/speech-service/)
- [How to Translate Speech](https://learn.microsoft.com/azure/ai-services/speech-service/how-to-translate-speech)
- [Speech SDK for C#](https://learn.microsoft.com/azure/ai-services/speech-service/quickstarts/setup-platform?pivots=programming-language-csharp)

## License

MIT