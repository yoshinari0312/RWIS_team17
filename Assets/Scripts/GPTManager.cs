using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPTManager : MonoBehaviour
{
    [SerializeField] private PromptHandler promptHandler;

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

    private void Start()
    {
        if (promptHandler == null)
        {
            Debug.LogError("PromptHandler is not assigned. Please assign it in the Inspector.");
            return;
        }
        Debug.Log("Starting Interview automatically...");
        StartInterview();
    }
    public void DebugSetAnswer(string answer)
    {
        if (string.IsNullOrEmpty(answer))
        {
            Debug.LogError("回答が空です。正しい回答を入力してください。");
            return;
        }

        SetUserAnswer(answer);
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
        AskNextQuestion();
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
                    Debug.Log($"回答をインスペクターで入力し、DebugSetAnswerメソッドを呼び出してください (例: DebugSetAnswer(\"あなたの回答\"))");
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
    public void SetUserAnswer(string answer)
    {
        userAnswer = answer;
        Debug.Log("User answer set: " + userAnswer);
        ReceiveAnswer(userAnswer);
    }


    public void ReceiveAnswer(string userAnswer)
    {
        if (followUpDepth < 2)
        {
            Debug.Log("Generating follow-up question...");
            followUpDepth++;
            promptHandler.GenerateFollowUp(companyName, questionItems[currentQuestionIndex], userAnswer, followUpDepth, response =>
            {
                if (!string.IsNullOrEmpty(response))
                {
                    Debug.Log($"Follow-up Question (Depth {followUpDepth}): " + response);
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
            currentQuestionIndex++;
            AskNextQuestion();
        }
    }

}
