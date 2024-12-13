using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField]
    private Text numberText; // 表示先のTextコンポーネントを格納する変数

    private void Start()
    {
        // 1から100の範囲で乱数を生成
        int randomNumber = Random.Range(1, 101);

        numberText.text = $" {randomNumber} 点 ! \n 非常に素晴らしいです。";
    }
}
