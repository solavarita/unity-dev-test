using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KickYouBall : MonoBehaviour
{
    [Header("Ball Detection")]
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject kickButton;    

    [Header("Khung thành")]
    [SerializeField] private Transform leftGoal;
    [SerializeField] private Transform rightGoal;

    [Header("Sút bóng")]
    [SerializeField] private Rigidbody[] ballRigidbody;
    [SerializeField] private float ballSpeed = 10f;
    [SerializeField] private Transform ballTarget;

    private float distanceToLeft, distanceToRight;

    private void Start()
    {
        kickButton.SetActive(false);
    }
    private void Update()
    {
        CheckNearbyBalls();
    }

    private void CheckGoalAndScore()
    {
        // Tìm tất cả quả bóng trong vùng
        Collider[] colliders = Physics.OverlapSphere(player.position, detectionRadius, targetLayer);

        Collider nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (var col in colliders)
        {
            float dist = Vector3.Distance(player.position, col.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = col;
            }
        }

        if (nearest == null)
        {
            Debug.Log("Không có quả nào trong vùng!");
            return;
        }

        // Tính khoảng cách tới 2 khung thành
        distanceToLeft = Vector3.Distance(player.position, leftGoal.position);
        distanceToRight = Vector3.Distance(player.position, rightGoal.position);

        if (distanceToLeft < distanceToRight)
        {
            Debug.Log("Sút vào khung thành TRÁI");
            MoveTheBall(nearest.attachedRigidbody, leftGoal.position);
            ChangeCameraTemporary(nearest.transform);
        }
        else
        {
            Debug.Log("Sút vào khung thành PHẢI");
            MoveTheBall(nearest.attachedRigidbody, rightGoal.position);
            ChangeCameraTemporary(nearest.transform);
        }
    }
    private void CheckFarestAndScore()
    {
        Rigidbody farestBall = null;
        float maxDistance = -Mathf.Infinity;

        foreach (Rigidbody ball in ballRigidbody)
        {
            if (ball == null) continue;

            float dist = Vector3.Distance(player.position, ball.position);
            if (dist > maxDistance)
            {
                maxDistance = dist;
                farestBall = ball;
            }
        }
        if (farestBall == null)
        {
            Debug.Log("Không tìm thấy quả bóng hợp lệ!");
            return;
        }

        distanceToLeft = Vector3.Distance(player.position, leftGoal.position);
        distanceToRight = Vector3.Distance(player.position, rightGoal.position);
        Vector3 targetGoal = distanceToLeft < distanceToRight ? leftGoal.position : rightGoal.position;

        Debug.Log("Sút quả bóng xa nhất: " + farestBall.name);
        MoveTheBall(farestBall, targetGoal);
        ChangeCameraTemporary(farestBall.transform);
    }

    private void MoveTheBall(Rigidbody ball, Vector3 targetPos)
    {
        if (ball == null) return;

        Vector3 direction = (targetPos - ball.position).normalized;
        ball.velocity = Vector3.zero; // Reset lực cũ
        ball.AddForce(direction * ballSpeed, ForceMode.Impulse);

        Debug.Log("Đã sút quả bóng: " + ball.name);        
    }

    private void ChangeCameraTemporary(Transform newTarget)
    {
        StartCoroutine(FocusOnBallCoroutine(newTarget));
    }

    private IEnumerator FocusOnBallCoroutine(Transform ballTransform)
    {
        CameraFollow.Instance.targetPlayer = ballTransform;

        yield return new WaitForSeconds(2f);

        CameraFollow.Instance.targetPlayer = player;
    }

    public void KickBall()
    {
        Debug.Log("Kicked your balls");

        Vector3 targetPos = Vector3.zero;
        CheckGoalAndScore();
    }
    public void AutoKickBall()
    {
        Debug.Log("Kicked the farest ball");
        CheckFarestAndScore();
    }

    private void ShowKickButton()
    {
        if (!kickButton.activeSelf)
        {
            kickButton.SetActive(true);
        }
        return;
    }
    private void HideKickButton()
    {
        if (kickButton.activeSelf)
        {
            kickButton.SetActive(false);
        }
        return;
    }

    private void CheckNearbyBalls()
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, detectionRadius, targetLayer);

        Collider nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (var hitCollier in colliders)
        {
            float _distance = Vector3.Distance(player.position, hitCollier.transform.position);
            if (_distance < minDistance)
            {
                minDistance = _distance;
                nearest = hitCollier;
            }
        }

        if (nearest != null)
        {
            ShowKickButton();
        }
        else
        {
            HideKickButton();
        }
        Debug.DrawRay(transform.position, Vector3.up * 0.1f, Color.green);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
