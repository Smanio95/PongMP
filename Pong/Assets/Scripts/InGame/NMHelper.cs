using Mirror;
using Mirror.Examples.Pong;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NMHelper : NetworkBehaviour
{
    [Header("Spawn pos")]
    [SerializeField] Transform leftPos;
    [SerializeField] Transform rightPos;

    public Transform LeftPos { get => leftPos; }
    public Transform RightPos { get => rightPos; }

    [Header("UI")]
    [SerializeField] TMP_Text score1Txt;
    [SerializeField] TMP_Text score2Txt;
    [SerializeField] TMP_Text winTxt;

    [SyncVar(hook = nameof(UpdateScore1))]
    private int score1;
    [SyncVar(hook = nameof(UpdateScore2))]
    private int score2;

    [Header("WinCond")]
    [SerializeField] int maxPoints = 5;

    public int PlayerIndex { get; set; } // 0 - player1, 1 - player2

    public Ball Ball { get; set; }

    public delegate void Restart();
    public static Restart OnRestart;

    private void OnEnable()
    {
        Ball.OnScore += OnScore;
    }

    private void OnDisable()
    {
        Ball.OnScore -= OnScore;
    }

    // this function is only executed on server as the event is launched only on server side
    private void OnScore(bool isLeft)
    {
        if (isLeft)
        {
            score1++;
        }
        else
        {
            score2++;
        }

        if(score1 >= maxPoints)
        {
            ShowEndTxt(0);
        }
        else if(score2 >= maxPoints)
        {
            ShowEndTxt(1);
        }
        else
        {
            Ball.Move();
        }
    }

    [ClientRpc]
    private void ShowEndTxt(int index)
    {
        winTxt.transform.parent.gameObject.SetActive(true);

        winTxt.text = index == PlayerIndex ? "Hai vinto" : "Hai perso";
    }

    void UpdateScore1(int _, int newScore)
    {
        score1Txt.text = newScore.ToString();
    }

    void UpdateScore2(int _, int newScore)
    {
        score2Txt.text = newScore.ToString();
    }

    // ipotethically both clients can call this function that is fired inside the reset button
    // once the game is over. Nevertheless, as the panel where this button is placed will be
    // deactivated once the button is clicked, it is only fired one time (! the only way to fire 
    // this event simultaneously is to click the button in the exact same frame !)
    [Command(requiresAuthority = false)]  
    public void ResetGame()
    {
        score1 = score2 = 0;

        Ball.ResetBall();

        DeactivatePanel();
    }

    [ClientRpc]
    private void DeactivatePanel()
    {
        winTxt.transform.parent.gameObject.SetActive(false);

        OnRestart?.Invoke();
    }

}
