using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Message : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmp_mes;
    [SerializeField]
    private string[] mes = new string[5]; // メッセージ格納配列
    [SerializeField]
    private int num;
    [SerializeField]
    private int i;
    [SerializeField]
    private int j;

    [SerializeField]
    private float mes_wait;
    [SerializeField]
    private float ti;

    private bool isAllMessagesDisplayed = false;

    private void Start()
    {
        mes[0] = "小野寺";
        mes[1] = "佳成";
        mes[2] = "おのでら";
        mes[3] = "おので...";
        mes[4] = "おの.....";

        num = 0;
        i = 0;
        j = 0;

        mes_wait = 0.1f;
        ti = 0f;

        isAllMessagesDisplayed = false;
    }

    private void Update()
    {
        // 時間経過による文字の表示
        ti += Time.deltaTime;
        if (i < mes.Length) // 配列範囲内のメッセージがある場合のみ処理
        {
            num = mes[i].Length;
            tmp_mes.text = mes[i];

            if (ti >= mes_wait)
            {
                j++;
                if (j > num)
                {
                    j = num;
                }

                ti = 0f;
            }

            tmp_mes.maxVisibleCharacters = j;

            // クリックして次のメッセージへ
            if (Input.GetMouseButtonDown(0))
            {
                i++;
                j = 0;

                // すべてのメッセージが表示された場合
                if (i >= mes.Length)
                {
                    isAllMessagesDisplayed = true;
                    Debug.Log("すべてのメッセージが表示されました");
                }
            }
        }
    }

    public void EndInputText()
    {
        if (isAllMessagesDisplayed)
        {
            Debug.Log("Resultシーンに遷移します");
            SceneManager.LoadScene("Result");
        }
        else
        {
            Debug.Log("メッセージがまだすべて表示されていません");
        }
    }
}