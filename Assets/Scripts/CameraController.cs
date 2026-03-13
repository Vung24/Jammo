using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset;
    private Transform _target;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    void Start()
    {
        _target = _player;
    }
    // Update is called once per frame
    void Update()
    {
        if (_target == null) return;
        transform.position = _target.position + _offset;
        transform.LookAt(_target);
    }
    public void FollowBall(Transform ball)
    {
        _target = ball;
    }
    public void FollowPlayer()
    {
        _target = _player;
    }
    public void FollowPlayerDelay(float delay)
    {
        StartCoroutine(ReturnPlayer(delay));
    }
    private IEnumerator ReturnPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        FollowPlayer();
    }
}
