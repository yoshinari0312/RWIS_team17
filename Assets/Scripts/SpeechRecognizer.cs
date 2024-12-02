using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using SimpleJSON;

public class SpeechRecognizer
{
    public IEnumerator RecognizeSpeech(string audioBase64)
    {
        string apiKey = "AIzaSyCnoTSkifSkBKsdQruNSEkUD9X0lY5hQhw";
        string url = $"https://speech.googleapis.com/v1/speech:recognize?key={apiKey}";

        string jsonRequest = $@"
        {{
            ""config"": {{
                ""encoding"": ""LINEAR16"",
                ""sampleRateHertz"": 16000,
                ""languageCode"": ""ja-JP""
            }},
            ""audio"": {{
                ""content"": ""{audioBase64}""
            }}
        }}";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"レスポンス全文: {request.downloadHandler.text}");

                var jsonResponse = JSON.Parse(request.downloadHandler.text);
                if (jsonResponse["results"] != null && jsonResponse["results"].Count > 0)
                {
                    string transcript = jsonResponse["results"][0]["alternatives"][0]["transcript"];
                    Debug.Log($"認識されたテキスト: {transcript}");
                }
                else
                {
                    Debug.LogWarning("音声認識の結果が見つかりませんでした。");
                    Debug.Log($"レスポンス内容（JSON形式）: {request.downloadHandler.text}");
                }
            }else
            {
                Debug.LogError($"エラーが発生しました: {request.error}");
                Debug.Log($"レスポンス内容（JSON形式）: {request.downloadHandler.text}");
            }
        }
    }
}
