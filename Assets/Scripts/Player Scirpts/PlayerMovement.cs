using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private Animator animator;

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            moveDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            moveDirection += Vector3.back;
        if (Input.GetKey(KeyCode.A))
            moveDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            moveDirection += Vector3.right;

        if (moveDirection != Vector3.zero)
        {            
            transform.Translate(moveDirection.normalized * playerSpeed * Time.deltaTime, Space.World);

            transform.rotation = Quaternion.LookRotation(moveDirection);

            animator.SetFloat("Blend", 0.6f);
        }
        else
        {
            animator.SetFloat("Blend", 0);
        }
    }
}