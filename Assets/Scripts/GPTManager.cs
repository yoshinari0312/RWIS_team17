using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;


public class GPTManager : MonoBehaviour
{
    [SerializeField] private PromptHandler promptHandler;
    [SerializeField] private SpeechManagerWithUI speechManager;

    // 質問項目リスト
    [SerializeField] private List<string> questionItems;
    private int currentQuestionIndex = 0;

    // ユーザーの回答を保持する変数
    private string userAnswer = "";
    // 固定変数
    [SerializeField] private string companyName = "SampleCompany";
    [SerializeField] private string userName = "SampleUser";

    private int followUpDepth = 0;

    private void Awake()
    {
        if (questionItems == null || questionItems.Count == 0)
        {
            questionItems = new List<string>
            {
                "あなたの強みを教えてください。",
                "これまでの経験で最も大きな挑戦は何ですか？",
                "この会社を志望した理由を教えてください。"
            };
            Debug.Log("Default questions initialized.");
        }
        Debug.Log("Awake: QuestionItems count = " + questionItems.Count);
    }

    public void StartInterview()
    {
        Debug.Log("StartInterview called. QuestionItems count: " + questionItems?.Count);

        if (questionItems == null || questionItems.Count == 0)
        {
            Debug.LogError("Question items list is empty.");
            return;
        }

        currentQuestionIndex = 0;
        followUpDepth = 0;
    }

    private void AskNextQuestion()
    {
        if (currentQuestionIndex < questionItems.Count)
        {
            string currentQuestion = questionItems[currentQuestionIndex];
            promptHandler.GenerateQuestion(companyName, currentQuestion, userName, response =>
            {
                if (!string.IsNullOrEmpty(response))
                {
                    Debug.Log("Generated Question: " + response);
                    JObject parsedJson = JObject.Parse(response);
                    string gptReply = (string)parsedJson["choices"][0]["message"]["content"];
                    Debug.Log($"Raw Response: {gptReply}");
                    Debug.Log("2222222");
                    speechManager.ReceiveGPTReply(gptReply);
                }
                else
                {
                    Debug.LogError("Failed to generate question.");
                }
            });
        }
        else
        {
            Debug.Log("Interview end. All questions have been completed.");
        }
    }

    // ユーザー回答をセットするメソッド
    public void SetUserAnswer(string userAnswer)
    {
        Debug.Log("User answer set: " + userAnswer);
        if (followUpDepth == 1 || followUpDepth == 2)
        {
            Debug.Log("Generating follow-up question...");
            followUpDepth++;
            promptHandler.GenerateFollowUp(companyName, questionItems[currentQuestionIndex], userAnswer, followUpDepth, response =>
            {
                if (!string.IsNullOrEmpty(response))
                {
                    Debug.Log($"Follow-up Question (Depth {followUpDepth}): " + response);
                    JObject parsedJson = JObject.Parse(response);
                    string gptReply = (string)parsedJson["choices"][0]["message"]["content"];
                    Debug.Log($"Raw Response: {gptReply}");
                    Debug.Log("111111");
                    speechManager.ReceiveGPTReply(gptReply);
                }
                else
                {
                    Debug.LogError("Failed to generate follow-up question.");
                }
            });
        }
        else
        {
            Debug.Log($"Question {currentQuestionIndex + 1} completed. Moving to next question.");
            followUpDepth = 0;
            AskNextQuestion();
            currentQuestionIndex++;
            followUpDepth++;
        }
    }
}
