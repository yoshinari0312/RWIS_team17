using System.Collections;
using UnityEngine;
using System.Linq;

public class SpeechManager : MonoBehaviour
{
    private AudioClip recordedClip; // 録音した音声データ
    private bool isRecording = false; // 録音中かどうかのフラグ
    private float recordingStartTime; // 録音開始時間

    // 録音で使用するマイク名
    public string microphoneName = "MacBook Proのマイク";

    void Update()
    {
        // スペースキーが押されたら録音の開始/停止を切り替える
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isRecording)
            {
                StopRecording();
            }
            else
            {
                StartRecording();
            }
        }
    }

    void StartRecording()
    {
        // 利用可能なマイクデバイスを取得
        string[] devices = Microphone.devices;

        if (devices.Length > 0)
        {
            // 指定したマイクが存在するか確認
            if (System.Array.Exists(devices, device => device == microphoneName))
            {
                Debug.Log($"マイク「{microphoneName}」を使用します。");

                // 指定したマイクで録音を開始
                recordedClip = Microphone.Start(microphoneName, false, 60, 16000);
                recordingStartTime = Time.time; // 録音開始時間を記録
                Debug.Log("録音を開始しました...");
                isRecording = true; // 録音中フラグを立てる
            }
            else
            {
                Debug.LogError($"指定されたマイク「{microphoneName}」は見つかりませんでした。利用可能なマイク:");
                foreach (string device in devices)
                {
                    Debug.Log($" - {device}");
                }
            }
        }
        else
        {
            Debug.LogError("利用可能なマイクが見つかりませんでした。");
        }
    }

    // マイク録音の停止
    void StopRecording()
    {
        if (Microphone.IsRecording(microphoneName))
        {
            Microphone.End(microphoneName); // 録音停止
            float recordingDuration = Time.time - recordingStartTime; // 録音時間を計測
            int roundedDuration = Mathf.RoundToInt(recordingDuration); // 秒数を整数に丸める
            isRecording = false; // 録音中フラグを解除
            Debug.Log($"録音を停止しました。録音時間: {roundedDuration}秒");

            // 録音データをチェック
            float[] samples = new float[recordedClip.samples * recordedClip.channels];
            recordedClip.GetData(samples, 0);

            // 音量を計算
            float maxVolume = samples.Max(Mathf.Abs);
            Debug.Log($"最大音量: {maxVolume}");
            
            if (maxVolume > 0.01f) // 音声があると判断するしきい値
            {
                Debug.Log("音声が検出されました。");
            }
            else
            {
                Debug.LogWarning("録音データに音声が含まれていないようです。");
            }

            // 録音したクリップをトリミング
            recordedClip = TrimAudioClip(recordedClip, recordingDuration);
            ProcessAudioClip(recordedClip); // 次の処理に進む
        }
        else
        {
            Debug.LogWarning($"マイク「{microphoneName}」で録音は行われていません。");
        }
    }

    // AudioClipをトリミング
    AudioClip TrimAudioClip(AudioClip clip, float duration)
    {
        int samples = Mathf.FloorToInt(clip.samples * (duration / clip.length)); // 必要なサンプル数を計算
        float[] data = new float[samples];
        clip.GetData(data, 0); // 必要な範囲のデータを取得

        AudioClip trimmedClip = AudioClip.Create(clip.name + "_trimmed", samples, clip.channels, clip.frequency, false);
        trimmedClip.SetData(data, 0); // 新しいAudioClipにデータを設定

        Debug.Log($"AudioClipを{Mathf.RoundToInt(duration)}秒にトリミングしました。");
        return trimmedClip;
    }


    // 録音したAudioClipを処理
    void ProcessAudioClip(AudioClip clip)
    {
        string base64Audio = AudioClipConverter.ConvertToBase64(clip); // Base64形式に変換
        Debug.Log("音声データをBase64形式に変換しました。");
        StartCoroutine(SendToSpeechRecognition(base64Audio)); // SpeechRecognizerでAPI呼び出し
    }

    // SpeechRecognizerで音声認識
    IEnumerator SendToSpeechRecognition(string audioBase64)
    {
        SpeechRecognizer recognizer = new SpeechRecognizer(); // SpeechRecognizerがMonoBehaviourでない場合
        yield return recognizer.RecognizeSpeech(audioBase64); // API呼び出し
    }
}
