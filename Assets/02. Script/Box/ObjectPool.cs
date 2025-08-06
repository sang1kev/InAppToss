using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������Ʈ Ǯ ���� Ŭ����
/// </summary>
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance; // �̱��� �ν��Ͻ�

    [SerializeField] private GameObject blockPrefab; // ������ ��� ������
    private Queue<Block> poolingObjectQueue = new Queue<Block>(); // ����� ������� �����ϱ� ���� Queue

    public int ActiveBlockCount { get; private set; } // Ȱ��ȭ�� ��� ����

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Init(100);
    }

    private void Init(int initCount) // �ʱ� ����
    {
        for (int i = 0; i < initCount; i++) // initCount(10)���� �ݺ�
        {
            poolingObjectQueue.Enqueue(CreateBlock()); // ť�� ������ ����� ����
        }
    }

    private Block CreateBlock() // ��� ���� ����
    {
        var newObj = Instantiate(blockPrefab).GetComponent<Block>(); // ��� ���� ��, Block ������Ʈ ������
        newObj.gameObject.SetActive(false); // �̸� ���� ������Ʈ�� ��Ȱ��ȭ ���ѵ�
        newObj.transform.SetParent(transform); // ��� ������Ʈ�� �θ� ������Ʈ ����

        return newObj;
    }

    public static Block GetObject() // ��� ��� ����
    {
        Instance.ActiveBlockCount++; // ī��Ʈ ����(Ȱ��ȭ)

        if (Instance.poolingObjectQueue.Count > 0) // ť�� ����� 1���� ���ִٸ�
        {
            var obj = Instance.poolingObjectQueue.Dequeue(); // ť���� ����
            obj.transform.SetParent(Instance.transform); // �θ� ������Ʈ ����
            obj.gameObject.SetActive(true); // ��� ������Ʈ Ȱ��ȭ

            return obj;
        }
        else // ť �ȿ� 1���� ���ٸ�
        {
            var newObj = Instance.CreateBlock(); // ��� ����
            newObj.gameObject.SetActive(true); // ��� ������Ʈ Ȱ��ȭ
            newObj.transform.SetParent(Instance.transform); // �θ� ������Ʈ ����

            return newObj;
        }
    }

    public void ReturnObject(Block block) // ��� ��ȯ ���� ( �ν����� �� )
    {
        block.gameObject.SetActive(false);
        block.transform.SetParent(Instance.transform);
        poolingObjectQueue.Enqueue(block);
        ActiveBlockCount--; // ī��Ʈ ���� (��Ȱ��ȭ)
    }
}
