using UnityEngine;

public class MicrophoneTest : MonoBehaviour
{
    private AudioClip audioClip;
    
    // 録音で使用するマイク名
    public string microphoneName = "Built-in Microphone";

    void Start()
    {
        // マイクデバイスのリストを表示
        foreach (var device in Microphone.devices)
        {
            Debug.Log($"Microphone detected: {device}");
        }

        // マイクから音声録音開始
        StartRecording();
    }

    void StartRecording()
    {
        if (Microphone.devices.Length > 0)
        {
            audioClip = Microphone.Start(null, true, 10, 16000);
            Debug.Log("Recording started.");
        }
        else
        {
            Debug.LogError("No microphone detected.");
        }
    }

    void StopRecording()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
            Debug.Log("Recording stopped.");
        }
    }
}
