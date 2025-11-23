using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;

class Program
{
    // Semaphore to serialize speech synthesis operations
    private static readonly SemaphoreSlim synthesizeSemaphore = new SemaphoreSlim(1, 1);

    static async Task Main(string[] args)
    {
        Console.WriteLine("Azure Live Translation Demo");
        Console.WriteLine("===========================");
        Console.WriteLine("Translating Japanese audio to German with Text-to-Speech output\n");

        // Get Azure Speech Service credentials from environment variables
        var speechKey = Environment.GetEnvironmentVariable("AZURE_SPEECH_KEY");
        var speechRegion = Environment.GetEnvironmentVariable("AZURE_SPEECH_REGION");

        if (string.IsNullOrEmpty(speechKey) || string.IsNullOrEmpty(speechRegion))
        {
            Console.WriteLine("ERROR: Azure Speech Service credentials not found!");
            Console.WriteLine("Please set the following environment variables:");
            Console.WriteLine("  AZURE_SPEECH_KEY - Your Azure Speech Service key");
            Console.WriteLine("  AZURE_SPEECH_REGION - Your Azure Speech Service region (e.g., westeurope)");
            Console.WriteLine("\nExample:");
            Console.WriteLine("  export AZURE_SPEECH_KEY=your_key_here");
            Console.WriteLine("  export AZURE_SPEECH_REGION=westeurope");
            return;
        }

        await TranslateJapaneseToGermanAsync(speechKey, speechRegion);
    }

    static async Task TranslateJapaneseToGermanAsync(string speechKey, string speechRegion)
    {
        // Configure translation settings
        var translationConfig = SpeechTranslationConfig.FromSubscription(speechKey, speechRegion);
        
        // Set source language (Japanese)
        translationConfig.SpeechRecognitionLanguage = "ja-JP";
        
        // Add target language (German)
        translationConfig.AddTargetLanguage("de");
        
        // Configure voice for text-to-speech output in German
        translationConfig.VoiceName = "de-DE-KatjaNeural";

        // Use default microphone for audio input
        using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        
        // Create translation recognizer
        using var translationRecognizer = new TranslationRecognizer(translationConfig, audioConfig);

        // Subscribe to events for continuous recognition
        translationRecognizer.Recognizing += (s, e) =>
        {
            if (e.Result.Reason == ResultReason.TranslatingSpeech)
            {
                Console.WriteLine($"RECOGNIZING in Japanese: {e.Result.Text}");
                foreach (var translation in e.Result.Translations)
                {
                    Console.WriteLine($"    Translating to {translation.Key}: {translation.Value}");
                }
            }
        };

        translationRecognizer.Recognized += (s, e) =>
        {
            if (e.Result.Reason == ResultReason.TranslatedSpeech)
            {
                Console.WriteLine($"\nRECOGNIZED in Japanese: {e.Result.Text}");
                
                foreach (var translation in e.Result.Translations)
                {
                    var translatedText = translation.Value;
                    Console.WriteLine($"    TRANSLATED to {translation.Key}: {translatedText}");
                    
                    // Synthesize German translation to speech
                    if (translation.Key == "de")
                    {
                        // Queue speech synthesis to avoid concurrent operations and handle exceptions
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                await SynthesizeGermanSpeechAsync(speechKey, speechRegion, translatedText);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"    ERROR during speech synthesis: {ex.Message}");
                            }
                        });
                    }
                }
            }
            else if (e.Result.Reason == ResultReason.NoMatch)
            {
                Console.WriteLine($"NOMATCH: Speech could not be recognized.");
            }
        };

        translationRecognizer.Canceled += (s, e) =>
        {
            Console.WriteLine($"\nCANCELED: Reason={e.Reason}");

            if (e.Reason == CancellationReason.Error)
            {
                Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                Console.WriteLine($"CANCELED: Did you set the speech key and region correctly?");
            }
        };

        translationRecognizer.SessionStarted += (s, e) =>
        {
            Console.WriteLine("\nSession started.");
            Console.WriteLine("Speak in Japanese and press Enter when done...\n");
        };

        translationRecognizer.SessionStopped += (s, e) =>
        {
            Console.WriteLine("\nSession stopped.");
        };

        // Start continuous recognition
        await translationRecognizer.StartContinuousRecognitionAsync();

        Console.WriteLine("Listening... Press Enter to stop.\n");
        Console.ReadLine();

        // Stop recognition
        await translationRecognizer.StopContinuousRecognitionAsync();
    }

    static async Task SynthesizeGermanSpeechAsync(string speechKey, string speechRegion, string text)
    {
        // Use semaphore to serialize speech synthesis operations
        await synthesizeSemaphore.WaitAsync();
        try
        {
            // Configure speech synthesis
            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            speechConfig.SpeechSynthesisVoiceName = "de-DE-KatjaNeural";

            // Use default speaker for audio output
            using var audioConfig = AudioConfig.FromDefaultSpeakerOutput();
            using var speechSynthesizer = new SpeechSynthesizer(speechConfig, audioConfig);

            Console.WriteLine($"    SPEAKING: {text}");
            
            var result = await speechSynthesizer.SpeakTextAsync(text);

            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                Console.WriteLine($"    Speech synthesis completed.");
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                Console.WriteLine($"    CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"    CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                }
            }
        }
        finally
        {
            synthesizeSemaphore.Release();
        }
    }
}
