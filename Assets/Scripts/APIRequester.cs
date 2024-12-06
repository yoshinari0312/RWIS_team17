using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class APIRequester : MonoBehaviour
{
    private string apiKey = "";
    private string apiEndpoint = "https://api.openai.com/v1/chat/completions";


    public IEnumerator SendRequest(string jsonPayload, System.Action<string> onResponse)
    {
        var request = new UnityWebRequest(apiEndpoint, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        // request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");



        yield return request.SendWebRequest();

        // if (request.result == UnityWebRequest.Result.Success)
        // {
        //     Debug.Log("Response: " + request.downloadHandler.text);
        //     onResponse(request.downloadHandler.text);
        // }
        // else
        // {
        //     Debug.LogError("Error: " + request.error);
        //     onResponse(null);
        // }
        if (!request.isNetworkError && !request.isHttpError)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            onResponse(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"Error: {request.error}\nResponse: {request.downloadHandler.text}");
            onResponse(null);
        }


    }
}
