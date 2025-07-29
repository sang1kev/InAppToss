using System;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Animator playerAnim;

    private Vector3 inputDir;

    [SerializeField] private float playerForce = 1f;

    public bool isGameStart = false;
    private bool isDashAvail = false;
    private bool isBoxHit = false;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
    }

    // 업데이트 주기마다 높이가 달라지는 점을 고치기 위해 Fixed사용
    void FixedUpdate()
    {
        if (isGameStart && isDashAvail)
        {
            Dash();
        }
    }

    private void Dash()
    {
        Vector3 velocity = inputDir * playerForce;
        playerRb.AddForceX(velocity.x * 0.5f, ForceMode2D.Impulse);
        playerRb.AddForceY(velocity.y, ForceMode2D.Impulse);
    }

    public void InputJoyStick(float x, float y)
    {
        inputDir = new Vector3(x, y, 0);

        if (isGameStart && inputDir.x != 0)
        {
            int isXPositive = inputDir.x > 0 ? 1 : -1;
            transform.localScale = new Vector3(isXPositive, 1, 1);
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isDashAvail = true;
            inputDir = Vector3.zero;
            playerRb.AddForce(Vector3.zero);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isDashAvail = false;
            playerAnim.SetTrigger("Dash");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            isDashAvail = true;
            inputDir = Vector3.zero;
            playerRb.AddForce(Vector3.zero);
            playerAnim.SetTrigger("AttWorked");

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            isDashAvail = false;
        }
    }
}
