using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickManager : MonoBehaviour
{
    public static KickManager Instance { get; private set; }
    [SerializeField] private GameObject _kickButton;
    [SerializeField] private GameObject _autoKickButton;
    private BallInteraction _currentBall;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        _kickButton.SetActive(false);
    }
    public void ShowKickButton(BallInteraction ball)
    {
        _currentBall = ball;
        _kickButton.SetActive(true);
    }
    public void HideKickButton(BallInteraction ball)
    {
        if (_currentBall == ball)
        {
            _currentBall = null;
            _kickButton.SetActive(false);
        }
    }
    public void KickBall()
    {
        if (_currentBall != null)
        {
            _currentBall.Kick(_currentBall.FindNearestGoal());
        }
    }
    public void AutoKick()
    {
        BallInteraction farthest = BallInteraction.FindFarthestBall();
        if (farthest == null)
            return;

        farthest.Kick(farthest.FindNearestGoal());
    }

}
