using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToResult : MonoBehaviour
{
    public void EndInputText()
    {
        // 結果シーンに移動
        SceneManager.LoadScene("Result");
    }
}
