using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoldingSystem : MonoBehaviour
{
    //折り畳み部分を取得
    private GameObject FoldinfImage;
    //折り畳み部分を表示するか非表示にするか
    private bool OpenFold = false;
    //ボタンの画像を回転
    private RectTransform button;

    private VerticalLayoutGroup contentLayout;

    void Start()
    {
        FoldinfImage = transform.Find("Image2").gameObject;
        button = transform.Find("Image/Button").GetComponent<RectTransform>();

        contentLayout = GameObject.Find("Content").GetComponent<VerticalLayoutGroup>();

        FoldinfImage.SetActive(false); // Image2（折り畳み部分）を非表示に設定
        button.localRotation = Quaternion.Euler(0, 0, -90); // ボタンの初期回転（閉じている状態）
    }

    void Update()
    {

    }

    public void OnClickSetInfo()
    {
        if (OpenFold)
        {
            button.localRotation = Quaternion.Euler(0, 0, -90);
            FoldinfImage.SetActive(false);

            StartCoroutine("LayoutRenewal");

            OpenFold = false;
        }
        else
        {
            button.localRotation = Quaternion.Euler(0, 0, 90);
            FoldinfImage.SetActive(true);

            StartCoroutine("LayoutRenewal");

            OpenFold = true;
        }
        contentLayout.enabled = false;
        contentLayout.enabled = true;
    }

    IEnumerator LayoutRenewal()
    {
        //1フレーム停止
        yield return null;

        //再開後の処理
        contentLayout.enabled = false;
        contentLayout.enabled = true;
    }
}
