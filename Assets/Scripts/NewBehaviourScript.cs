using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public InputField UserName;
    public InputField OrgName;
    public InputField GptInput;
    public InputField GcpInput;

    void Start()
    {
        UserName = UserName.GetComponent<InputField>();
        OrgName = OrgName.GetComponent<InputField>();
        GptInput = GptInput.GetComponent<InputField>();
        GcpInput = GcpInput.GetComponent<InputField>();
    }

    // スタートボタンを押したときに呼ばれる関数
    public void EndInputText()
    {
        Debug.Log("ユーザー名: "+ UserName.text);
        UserName.text = ""; // フィールドの値消す

        Debug.Log("企業名: " + OrgName.text);
        OrgName.text = "";

        Debug.Log("GPT: " + GptInput.text);
        GptInput.text = "";

        Debug.Log("GCP: " + GcpInput.text);
        GcpInput.text = "";

        // ゲームシーンに移動
        SceneManager.LoadScene("Game"); // シーン名は適宜変更
    }
}
