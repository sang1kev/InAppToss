using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float restartYPos; // 블록 초기 Y 값
    [SerializeField] private float returnYPos; // 블록이 최대로 이동할 수 있는 좌표값

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
}
