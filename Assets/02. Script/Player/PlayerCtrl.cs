using System;
using System.Collections;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Animator playerAnim;
    private Animator groundAnim;

    private Vector3 inputDir;

    [SerializeField] private float playerForce = 1f;

    public bool isGameStart = false;
    private bool isDashAvail = false;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        groundAnim = GameObject.Find("Ground").GetComponent<Animator>();
    }

    private void Dash()
    {
        Vector3 velocity = inputDir * playerForce;

        //중력에 의한 force 초기화
        playerRb.linearVelocity = Vector3.zero;

        playerRb.AddForceX(velocity.x * 0.5f, ForceMode2D.Impulse);
        playerRb.AddForceY(velocity.y, ForceMode2D.Impulse);

        isDashAvail = false;
    }

    public void InputJoyStick(float x, float y)
    {
        if (!isDashAvail)
        {
            return;
        }

        inputDir = Vector3.zero;
        playerRb.AddForce(Vector3.zero);

        inputDir = new Vector3(x, y, 0);

        playerAnim.SetTrigger("Dash");

        if (isGameStart && inputDir.x != 0)
        {
            int isXPositive = inputDir.x > 0 ? 1 : -1;
            transform.localScale = new Vector3(isXPositive, 1, 1);
        }

        Dash();
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isDashAvail = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isDashAvail = false;
            StartCoroutine(GroundCollapse());
        }
    }

    IEnumerator GroundCollapse()
    {
        yield return new WaitForSeconds(2.25f);

        groundAnim.SetTrigger("Broke");

        yield return new WaitForSeconds(2.25f);
        groundAnim.gameObject.SetActive(false);
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
}
