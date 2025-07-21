using System;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    private enum PlayerState { IDLE, JUMP, DASH, FALL }
    private PlayerState pState;
    private Rigidbody2D pRb;
    private Animator animator;

    private Vector3 inputDir;

    void Start()
    {
        animator = GetComponent<Animator>();
        pRb = GetComponent<Rigidbody2D>();
    }

    void Update ()
    {
        Move();
        State();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, v, 0);
    }
    public void InputJoyStick(float x, float y)
    {
        inputDir = new Vector3(x, y, 0).normalized;
        animator.SetFloat("JoystickX", x);
        animator.SetFloat("JoystickY", y);

        if (inputDir.x != 0)
        {
            int isXPositive = inputDir.x > 0 ? 1 : -1;
            transform.localScale = new Vector3(isXPositive, 1, 1);
        }
    }

    private void State()
    {
        switch (pState)
        {
            case PlayerState.IDLE:
                Idle();
                break;
            case PlayerState.JUMP:
                Jump();
                break;
            case PlayerState.DASH:
                Dash();
                break;
            case PlayerState.FALL:
                Fall();
                break;
        }
    }

    private void Idle()
    {
        throw new NotImplementedException();
    }

    private void Jump()
    {
        throw new NotImplementedException();
    }

    private void Dash()
    {
        throw new NotImplementedException();
    }

    private void Fall()
    {
        throw new NotImplementedException();
    }
}
