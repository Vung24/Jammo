using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class BallInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform[] _goals;
    [SerializeField]private ParticleSystem _goalEffect;
    private float _kickForce = 40f;
    private float _kickUpward = 0.2f;
    private Rigidbody _rb;
    private float _radius = 1f;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        bool playerNear = Physics.CheckSphere(transform.position, _radius, _playerLayer);
        if (playerNear)
            KickManager.Instance.ShowKickButton(this);
        else
            KickManager.Instance.HideKickButton(this);
    }

    public Transform FindNearestGoal()
    {
        if (_goals == null || _goals.Length == 0)
            return null;

        Transform nearest = null;
        float bestSqr = float.MaxValue;

        for (int i = 0; i < _goals.Length; i++)
        {
            Transform goal = _goals[i];
            if (goal == null) continue;

            float sqr = (goal.position - transform.position).sqrMagnitude;

            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                nearest = goal;
            }
        }
        return nearest;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            CameraController.Instance.FollowPlayerDelay(2f);
            GameObject goalObject = other.gameObject.transform.parent.gameObject;
            GoalController goalController = goalObject.GetComponent<GoalController>();
            if (goalController != null)
            {
                goalController.PlayEffect();
            }
        }
    }
    public static BallInteraction FindFarthestBall()
    {
        PlayerController player = PlayerController.Instance;

        if (player == null)
            return null;

        BallInteraction[] balls = FindObjectsOfType<BallInteraction>();
        if (balls == null || balls.Length == 0)
            return null;

        BallInteraction farthest = null;
        float bestSqr = -1f;

        Vector3 playerPos = player.transform.position;

        for (int i = 0; i < balls.Length; i++)
        {
            BallInteraction ball = balls[i];
            if (ball == null || !ball.isActiveAndEnabled) continue;

            float sqr = (ball.transform.position - playerPos).sqrMagnitude;

            if (sqr > bestSqr)
            {
                bestSqr = sqr;
                farthest = ball;
            }
        }
        return farthest;
    }
    public void Kick(Transform goal)
    {
        if (goal == null) return;

        Vector3 dir = (goal.position - transform.position).normalized;
        dir += Vector3.up * _kickUpward;

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        _rb.AddForce(dir.normalized * _kickForce, ForceMode.Impulse);
        CameraController.Instance.FollowBall(transform);
    }
}
