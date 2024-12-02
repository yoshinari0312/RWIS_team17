using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpeechManagerWithUI : MonoBehaviour
{
    public Button recordButton; // 録音開始/停止用ボタン
    public Text statusText; // 状態や結果を表示するText
    private AudioClip recordedClip; // 録音した音声データ
    private bool isRecording = false; // 録音中かどうかのフラグ
    private float recordingStartTime; // 録音開始時間
    private bool isProcessing = false; // 処理中フラグ

    void Start()
    {
        // ボタンにクリックイベントを設定
        recordButton.onClick.AddListener(ToggleRecording);
        statusText.text = "準備完了"; // 初期状態を設定
    }

    public void ToggleRecording()
    {
        if (isProcessing) return; // 処理中なら何もしない
        isProcessing = true; // 処理中フラグを設定

        if (isRecording)
        {
            StopRecording();
        }
        else
        {
            StartRecording();
        }
        // ボタンのテキストを更新
        recordButton.GetComponentInChildren<Text>().text = isRecording ? "録音停止" : "録音開始";

        // 処理を少し遅らせて解除
        StartCoroutine(ResetProcessingFlag());
    }

    private IEnumerator ResetProcessingFlag()
    {
        yield return new WaitForSeconds(0.1f); // 0.1秒待機（適宜調整）
        isProcessing = false; // 処理中フラグを解除
    }

    void StartRecording()
    {
        if (isRecording)
        {
            Debug.LogWarning("既に録音中です。");
            return;
        }

        // デフォルトのマイクを使用して録音を開始
        recordedClip = Microphone.Start(null, false, 60, 16000);
        recordingStartTime = Time.time; // 録音開始時間を記録
        isRecording = true; // 録音中フラグを立てる
        Debug.Log("録音を開始しました...");
    }

    void StopRecording()
    {
        if (!isRecording)
        {
            Debug.LogWarning("録音は開始されていません。");
            return;
        }

        Microphone.End(null); // 録音停止
        float recordingDuration = Time.time - recordingStartTime; // 録音時間を計測
        int roundedDuration = Mathf.RoundToInt(recordingDuration); // 秒数を整数に丸める
        isRecording = false; // 録音中フラグを解除
        Debug.Log($"録音を停止しました。録音時間: {roundedDuration}秒");

        // 録音したAudioClipを処理
        if (recordedClip != null)
        {
            recordedClip = TrimAudioClip(recordedClip, recordingDuration);
            ProcessAudioClip(recordedClip);
        }
        else
        {
            Debug.LogError("録音クリップが無効です。");
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
        string base64Audio = AudioClipConverter.ConvertToBase64(clip);
        Debug.Log("音声データをBase64形式に変換しました。");
        statusText.text = "音声データを処理中...";
        StartCoroutine(SendToSpeechRecognition(base64Audio)); // SpeechRecognizerでAPI呼び出し
    }

    // SpeechRecognizerで音声認識
    IEnumerator SendToSpeechRecognition(string audioBase64)
    {
        SpeechRecognizer recognizer = new SpeechRecognizer();
        yield return recognizer.RecognizeSpeech(audioBase64);

        // 認識結果を更新
        string recognizedText = recognizer.GetRecognizedText();
        if (!string.IsNullOrEmpty(recognizedText))
        {
            statusText.text = $"認識結果: {recognizedText}";
        }
        else
        {
            statusText.text = "認識結果なし。";
        }
    }
}
