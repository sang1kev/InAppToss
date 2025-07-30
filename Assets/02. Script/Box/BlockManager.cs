// BlockManager.cs

using System;
using System.Collections;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance;

    private enum PosType { Left, Right }
    private enum LevelType { Lv1, Lv2, Lv3, Lv4, Infinity }
    private PosType e_pos = PosType.Left;
    [SerializeField] private LevelType e_level = LevelType.Lv1;

    [Header("생성 위치 값"), Space(5)]
    [SerializeField] private Vector3 leftPos;
    [SerializeField] private Vector3 rightPos;

    [Header("이동 속도"), Space(5)]
    public float moveSpeed = 1.4f;
    private float currentTime;

    [Header("레벨 시스템"), Space(5)]
    [SerializeField] private float levelUpTime = 20f; // 레벨업 주기(초)
    [SerializeField] private float createDelay = 1.5f; // 블록 생성 주기
    [SerializeField] private float speedAcceleration = 0.2f; // 블록 이동속도 가속 변수
    [SerializeField] private int currentLevel = 1; // 레벨 시스템
    [SerializeField] private int maxBlockCount = 6; // 현재 레벨에서 허용하는 최대 블록 수

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(CreateBlockLoop());
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= levelUpTime)
        {
            moveSpeed += speedAcceleration;
            currentTime = 0f;

            CheckLevelUp(); // 속도가 오를 때마다 레벨업 체크
        }
    }

    void CheckLevelUp() // 레벨업 관리 함수
    {
        // 속도에 따라 레벨과 블록 수를 조절 ( 아래 if문 부터 순차적으로 올라옴 )
        if (moveSpeed >= 10.0f && currentLevel < 5) e_level = LevelType.Infinity;
        else if (moveSpeed >= 8.0f && currentLevel < 4) e_level = LevelType.Lv4;
        else if (moveSpeed >= 5.0f && currentLevel < 3) e_level = LevelType.Lv3;
        else if (moveSpeed >= 2.5f && currentLevel < 2) e_level = LevelType.Lv2;

        switch (e_level)
        {
            case LevelType.Lv1:
                SetLevelSystem(1, 6, 1.5f, 15f, 0.2f);
                break;
            case LevelType.Lv2:
                SetLevelSystem(2, 5, 1.4f, 15f, 0.25f);
                break;
            case LevelType.Lv3:
                SetLevelSystem(3, 4, 1.3f, 20f, 0.3f);
                break;
            case LevelType.Lv4:
                SetLevelSystem(4, 3, 1.2f, 25f, 0.4f);
                break;
            case LevelType.Infinity:
                SetLevelSystem(5, 3, 0.2f, 50f, 0.5f);
                break;
        }
    }

    // 레벨 시스템 설정 로직
    private void SetLevelSystem(int _level, int _maxBlockCount,
        float _createDelay, float _levelUpTime, float _speedAcceleration)
    {
        currentLevel = _level;
        maxBlockCount = _maxBlockCount;
        createDelay = _createDelay;
        levelUpTime = _levelUpTime;
        speedAcceleration = _speedAcceleration;

    }

    // 블록 생성 로직
    IEnumerator CreateBlockLoop()
    {
        // 게임이 끝날 때까지 계속 반복
        while (true)
        {
            // 현재 활성화된 블록 수가 최대치보다 적을 때만 새로 생성
            if (ObjectPool.Instance.ActiveBlockCount < maxBlockCount)
            {
                Block newBlock = ObjectPool.GetObject();

                switch (e_pos)
                {
                    case PosType.Left:
                        newBlock.transform.position = leftPos;
                        break;
                    case PosType.Right:
                        newBlock.transform.position = rightPos;
                        break;
                }

                e_pos = e_pos == PosType.Left ? PosType.Right : PosType.Left;

                yield return new WaitForSeconds(createDelay);
            }
            else
            {
                // 블록이 최대치라면 잠시 기다렸다가 다시 체크
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}