using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using SimpleJSON;

public class SpeechRecognizer
{
    private string recognizedText; // 音声認識結果を格納する

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
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
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
                    // 最初の候補を取得
                    recognizedText = jsonResponse["results"][0]["alternatives"][0]["transcript"];
                    Debug.Log($"認識されたテキスト: {recognizedText}");
                }
                else
                {
                    recognizedText = ""; // 結果がない場合は空にする
                    Debug.LogWarning("音声認識の結果が見つかりませんでした。");
                }
            }
            else
            {
                recognizedText = ""; // エラー時も空にする
                Debug.LogError($"エラーが発生しました: {request.error}");
                Debug.Log($"レスポンス内容（JSON形式）: {request.downloadHandler.text}");
            }
        }
    }

    public string GetRecognizedText()
    {
        return recognizedText; // 結果を取得する
    }
}
