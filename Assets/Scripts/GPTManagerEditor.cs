using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GPTManager))]
public class GPTManagerEditor : Editor
{
    private string debugAnswer = "";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GPTManager manager = (GPTManager)target;

        // 入力フィールドを作成
        debugAnswer = EditorGUILayout.TextField("Debug Answer", debugAnswer);

        // ボタンを作成
        if (GUILayout.Button("Submit Answer"))
        {
            // manager.DebugSetAnswer(debugAnswer);
            debugAnswer = ""; // 入力フィールドをクリア
        }
    }
}
