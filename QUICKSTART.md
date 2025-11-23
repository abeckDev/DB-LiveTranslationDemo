# Quick Start Guide

## 5-Minute Setup

### 1. Get Azure Speech Service Credentials

1. Go to [Azure Portal](https://portal.azure.com)
2. Create a **Speech Service** resource
3. Copy your **Key** and **Region** from the resource

### 2. Set Environment Variables

```bash
# Linux/macOS
export AZURE_SPEECH_KEY="paste-your-key-here"
export AZURE_SPEECH_REGION="paste-your-region-here"

# Windows PowerShell
$env:AZURE_SPEECH_KEY="paste-your-key-here"
$env:AZURE_SPEECH_REGION="paste-your-region-here"
```

### 3. Run the Application

```bash
cd LiveTranslationDemo
dotnet run
```

### 4. Test It!

1. Wait for "Listening... Press Enter to stop."
2. Speak in Japanese into your microphone
3. Listen to the German translation through your speakers
4. Press Enter to stop

## Example Japanese Phrases

Try these simple phrases:

- **こんにちは** (konnichiwa) → Hallo
- **ありがとう** (arigatō) → Danke
- **おはよう** (ohayō) → Guten Morgen
- **さようなら** (sayōnara) → Auf Wiedersehen

## Common Issues

**No microphone detected**
- Check microphone permissions
- Ensure microphone is set as default input device

**Credentials error**
- Verify environment variables are set in current terminal session
- Check Azure portal for correct key and region

**No audio output**
- Check speakers are connected and enabled
- Verify system audio settings

## Need Help?

See the full [README.md](README.md) for detailed documentation.
