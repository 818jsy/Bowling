using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public PinController pinController; // 핀 컨트롤러 참조
    public TextMeshProUGUI firstThrowText; // 1차 점수 텍스트
    public TextMeshProUGUI secondThrowText; // 2차 점수 텍스트
    public TextMeshProUGUI totalScoreText; // 총 점수 텍스트

    private int totalScore = 0; // 총 점수
    private int currentThrow = 0; // 현재 투구 수 (0: 첫 번째, 1: 두 번째)
    private int firstThrowScore = 0; // 첫 번째 투구 점수
    private int secondThrowScore = 0; // 두 번째 투구 점수
    private bool isCheckingPins = false; // 핀 상태를 체크 중인지 여부

    void Start()
    {
        StartNewTurn();
        UpdateScoreUI(); // UI 초기화
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isCheckingPins)
        {
            ThrowBall();
        }
    }

    void ThrowBall()
    {
        currentThrow++; // 투구 횟수 증가
        isCheckingPins = true; // 핀 체크 중

        // 일정 시간 후 핀 상태 확인
        StartCoroutine(WaitAndCheckPins(4f)); // 4초 대기 후 핀 상태 확인
    }

    IEnumerator WaitAndCheckPins(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // 핀 상태 확인
        int standingPins = pinController.CountStandingPins();
        int knockedDownPins = 10 - standingPins;

        if (currentThrow == 1)
        {
            // 첫 번째 투구 처리
            firstThrowScore = knockedDownPins;
            totalScore += firstThrowScore;
            Debug.Log("First Throw: " + firstThrowScore + " pins knocked down!");
            Debug.Log("Total Score after First Throw: " + totalScore);
            
            secondThrowScore = 0;

            UpdateScoreUI(); // UI 업데이트

            if (standingPins == 0)
            {
                Debug.Log("Strike! 🎳");
                StartNewTurn(); // 스트라이크로 턴 종료
                yield break;
            }
        }
        else if (currentThrow == 2)
        {
            // 두 번째 투구 처리
            secondThrowScore = knockedDownPins - firstThrowScore;
            totalScore += secondThrowScore;
            Debug.Log("Second Throw: " + secondThrowScore + " pins knocked down!");

            UpdateScoreUI(); // UI 업데이트

            if (standingPins == 0)
            {
                Debug.Log("Spare! 🎳");
            }
            else
            {
                Debug.Log("End of Turn. Total Pins: " + (firstThrowScore + secondThrowScore));
            }

            Debug.Log("Total Score after Second Throw: " + totalScore);
            StartNewTurn(); // 새로운 턴 시작
        }

        isCheckingPins = false; // 핀 체크 완료
    }

    void StartNewTurn()
    {
        Debug.Log("Starting new turn...");
        currentThrow = 0;
        isCheckingPins = false;

        UpdateScoreUI(); // UI 초기화
        pinController.ResetPins(); // 핀 초기화
    }

    void UpdateScoreUI()
    {
        // UI 텍스트 업데이트
        if (firstThrowText != null)
        {
            firstThrowText.text = firstThrowScore.ToString();
        }

        if (secondThrowText != null)
        {
            secondThrowText.text = secondThrowScore.ToString();
        }

        if (totalScoreText != null)
        {
            totalScoreText.text = totalScore.ToString();
        }
    }
}
