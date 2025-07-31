using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;
    [SerializeField] public Transform targetPlayer;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -5f);

    private void Start()
    {
        Instance = this;
    }
    private void LateUpdate()
    {
        if (targetPlayer == null) return;

        Vector3 goalPos = targetPlayer.position + offset;

        transform.position = Vector3.Lerp(transform.position, goalPos, smoothSpeed * Time.deltaTime);
    }

    IEnumerator FollowBall()
    {
        yield return null;
    }
}
