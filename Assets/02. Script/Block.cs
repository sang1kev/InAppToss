using System;
using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float restartYPos; // 블록 초기 Y 값
    [SerializeField] private float returnYPos; // 블록이 최대로 이동할 수 있는 좌표값

    private bool isBroke = false;
    
    private Animator blockAnim;

    private void Start()
    {
        blockAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        MoveBlock();
    }

    void MoveBlock() // 블록 이동 함수
    {
        // returnVec을 넘어서거나 같아질 때
        if (transform.position.y <= returnYPos)
        {
            // 위치를 재지정하는 대신, 오브젝트 풀에 반환
            ObjectPool.Instance.ReturnObject(this);
        }
        else
        {
            transform.position += Vector3.down * BlockManager.Instance.moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(IBrokeCoroutine());
        }
    }

    IEnumerator IBrokeCoroutine()
    {
        blockAnim.SetBool("isBroke", true);
        yield return new WaitForSeconds(0.2f);
        
        // 위치를 재지정하는 대신, 오브젝트 풀에 반환
        blockAnim.SetBool("isBroke", false);
        ObjectPool.Instance.ReturnObject(this);
    }
}
