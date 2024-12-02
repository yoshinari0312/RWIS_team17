using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class SpeechSynthesizer
{
    public IEnumerator SynthesizeSpeech(string text, Action<AudioClip> onAudioReady)
    {
        string apiKey = "AIzaSyCnoTSkifSkBKsdQruNSEkUD9X0lY5hQhw";
        string url = $"https://texttospeech.googleapis.com/v1/text:synthesize?key={apiKey}";

        string jsonRequest = $@"
        {{
            ""input"": {{""text"": ""{text}""}},
            ""voice"": {{
                ""languageCode"": ""ja-JP"",
                ""name"": ""ja-JP-Wavenet-B""
            }},
            ""audioConfig"": {{
                ""audioEncoding"": ""LINEAR16"",
                ""sampleRateHertz"": 16000
            }}
        }}";

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, jsonRequest))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var jsonResponse = JSON.Parse(request.downloadHandler.text);
                string audioContent = jsonResponse["audioContent"];
                AudioClip audioClip = ConvertToAudioClip(audioContent);
                onAudioReady?.Invoke(audioClip);
            }
            else
            {
                onAudioReady?.Invoke(null);
            }
        }
    }

    private AudioClip ConvertToAudioClip(string base64Audio)
    {
        byte[] audioBytes = Convert.FromBase64String(base64Audio);
        float[] samples = new float[audioBytes.Length / 2];
        for (int i = 0; i < samples.Length; i++)
        {
            short sample = BitConverter.ToInt16(audioBytes, i * 2);
            samples[i] = sample / 32768f;
        }

        AudioClip audioClip = AudioClip.Create("SynthesizedAudio", samples.Length, 1, 16000, false);
        audioClip.SetData(samples, 0);
        return audioClip;
    }
}
