using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 오브젝트 풀 관리 클래스
/// </summary>
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance; // 싱글톤 인스턴스
    
    [SerializeField] private GameObject blockPrefab; // 생성할 블록 프리팹
    private Queue<Block> poolingObjectQueue = new Queue<Block>(); // 블록을 순서대로 관리하기 위한 Queue

    public int ActiveBlockCount { get; private set; } // 활성화된 블록 개수
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Init(10);
    }
    
    private void Init(int initCount) // 초기 설정
    {
        for (int i = 0; i < initCount; i++) // initCount(10)까지 반복
        {
            poolingObjectQueue.Enqueue(CreateObj()); // 큐에 생성된 블록을 넣음
        }
    }
    
    private Block CreateObj() // 블록 생성 로직
    {
        var newObj = Instantiate(blockPrefab).GetComponent<Block>(); // 블록 생성 후, Block 컴포넌트 가져옴
        newObj.gameObject.SetActive(false); // 미리 게임 오브젝트를 비활성화 시켜둠
        newObj.transform.SetParent(transform); // 블록 오브젝트의 부모 오브젝트 지정
        
        return newObj;
    }
    
    public static Block GetObject() // 블록 사용 로직
    {
        Instance.ActiveBlockCount++; // 카운트 증가(활성화)
        
        if(Instance.poolingObjectQueue.Count > 0) // 큐에 블록이 1개라도 들어가있다면
        {
            var obj = Instance.poolingObjectQueue.Dequeue(); // 큐에서 꺼냄
            obj.transform.SetParent(Instance.transform); // 부모 오브젝트 지정
            obj.gameObject.SetActive(true); // 블록 오브젝트 활성화
            
            return obj;
        }
        else // 큐 안에 1개도 없다면
        {
            var newObj = Instance.CreateObj(); // 블록 생성
            newObj.gameObject.SetActive(true); // 블록 오브젝트 활성화
            newObj.transform.SetParent(Instance.transform); // 부모 오브젝트 지정
            
            return newObj;
        }
    }
    
    public void ReturnObject(Block block) // 블록 반환 로직 ( 부숴졌을 때 )
    {
        block.gameObject.SetActive(false);
        block.transform.SetParent(Instance.transform);
        poolingObjectQueue.Enqueue(block);
        ActiveBlockCount--; // 카운트 차감 (비활성화)
    }
}
