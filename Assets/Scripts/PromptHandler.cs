using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class PromptHandler : MonoBehaviour
{
    [SerializeField] private APIRequester apiRequester;

    public void GenerateQuestion(string companyName, string question, string userName, System.Action<string> onResponse)
    {
        Debug.Log("GenerateQuestion called, attempting to send request...");
        var requestBody = new
        {
            model = "gpt-4o-mini",
            messages = new List<object>
            {
                new { role = "system", content = $"あなたは面接官です。会社名は{companyName}、応募者は{userName}です。" },
                new { role = "user", content = $"質問項目: {question}" }
            },
            temperature = 0.7,
            max_tokens = 100,
            top_p = 1.0,
            frequency_penalty = 0.0,
            presence_penalty = 0.0
        };

        string jsonPayload = JsonConvert.SerializeObject(requestBody);
        // Debug.Log("Request Payload: " + jsonPayload);


        // 呼び出し部分
        StartCoroutine(apiRequester.SendRequest(jsonPayload, onResponse)); // StartCoroutine が必要
        Debug.Log("Starting Coroutine to call SendRequest...");
        StartCoroutine(apiRequester.SendRequest(jsonPayload, onResponse)); // 必ずコルーチンで呼び出す
        // Debug.Log("Starting Coroutine to call SendRequest...");

    }

    public void GenerateFollowUp(string companyName, string question, string userAnswer, int depth, System.Action<string> onResponse)
    {
        Debug.Log($"GenerateFollowUp called (Depth: {depth})...");
        var requestBody = new
        {
            model = "gpt-4o-mini",
            messages = new List<object>
            {
                new { role = "system", content = $"あなたは面接官です。会社名は{companyName}、応募者はユーザーです。" },
                new { role = "assistant", content = $"最初の質問: {question}" },
                new { role = "user", content = $"回答: {userAnswer}" },
                new { role = "assistant", content = $"これに基づいた深掘り質問をしてください。深掘りレベル: {depth}" }
            },
            temperature = 0.7,
            max_tokens = 100,
            top_p = 1.0,
            frequency_penalty = 0.0,
            presence_penalty = 0.0
        };

        string jsonPayload = JsonConvert.SerializeObject(requestBody);
        StartCoroutine(apiRequester.SendRequest(jsonPayload, onResponse)); // 必ずコルーチンで呼び出す
        Debug.Log("Starting Coroutine to call SendRequest for follow-up...");

        // apiRequester.SendRequest(jsonPayload, onResponse);
    }
}
