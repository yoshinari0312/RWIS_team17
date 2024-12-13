using System.IO;
using UnityEngine;

public class GCPKeyLoader : MonoBehaviour
{
    public string LoadGCPKey()
    {
        TextAsset keyFile = Resources.Load<TextAsset>("rwis-442818-c2873364044e"); // JSONファイル名から拡張子を除く
        if (keyFile != null)
        {
            Debug.Log("GCP Key loaded successfully.");
            return keyFile.text;
        }
        else
        {
            Debug.LogError("GCP Key file not found!");
            return null;
        }
    }
}
