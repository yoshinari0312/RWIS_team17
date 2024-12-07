using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;


public class APIRequester : MonoBehaviour
{
    private string apiKey = "SECRET";
    private string apiEndpoint = "https://api.openai.com/v1/chat/completions";


    public IEnumerator SendRequest(string jsonPayload, System.Action<string> onResponse)
    {
        Debug.Log("SendRequest method entered...");
    
        // UnityWebRequest を using ステートメントで囲むことでリソースを確実に解放
        using (var request = new UnityWebRequest(apiEndpoint, "POST"))
        {
            // リクエストデータの設定
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonPayload));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

            Debug.Log("Sending API request...");

            // リクエストを送信
            yield return request.SendWebRequest();

            // エラー処理
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}\nResponse: {request.downloadHandler.text}");
                onResponse(null); // エラー時に null を返す
            }
            else
            {
                // 正常なレスポンスを受け取った場合
                Debug.Log("Response received: " + request.downloadHandler.text);
                onResponse(request.downloadHandler.text); // レスポンスをコールバックに渡す
            }
        } // ここで UnityWebRequest のリソースが解放される
    }

    public IEnumerator SendRequestWithRetry(string jsonPayload, System.Action<string> onResponse, int maxRetries = 3)
    {
        int attempt = 0;

        while (attempt < maxRetries)
        {
            attempt++;
            Debug.Log($"Attempt {attempt} to send request...");

            yield return SendRequest(jsonPayload, response =>
            {
                if (!string.IsNullOrEmpty(response))
                {
                    onResponse(response); // 成功したらレスポンスを返す
                    attempt = maxRetries; // 成功時にループを終了
                }
                else if (attempt >= maxRetries)
                {
                    Debug.LogError("All retries failed.");
                    onResponse(null); // 失敗時に null を返す
                }
            });
        }
    }


}