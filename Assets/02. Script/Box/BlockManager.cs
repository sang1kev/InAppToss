using System.Collections;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private enum LevelType { Lv1, Lv2, Lv3, Lv4, Infinity }
    [SerializeField] private LevelType e_level = LevelType.Lv1;

    private PlayerCtrl playerCtrl;
    
    [Header("ï¿½Ìµï¿½ ï¿½Óµï¿½"), Space(5)]
    public static float moveSpeed = 1.4f;
    private float currentTime;

    [Header("ï¿½ï¿½ï¿½ï¿½ ï¿½Ã½ï¿½ï¿½ï¿½"), Space(5)]
    [SerializeField] private float levelUpTime = 20f;
    [SerializeField] private float speedAcceleration = 0.5f;
    [SerializeField] private float createDelay = 0.4f;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int maxBlockCount = 6;

    private void Start()
    {
        playerCtrl = FindFirstObjectByType<PlayerCtrl>();
        
        StartCoroutine(CreateBlockLoop());
    }

    private void Update()
    {
        if (UIManager.Instance != null && !UIManager.Instance.IsGameStarted) 
            return;
        
        currentTime += Time.deltaTime;

        if (currentTime >= levelUpTime)
        {
            moveSpeed += speedAcceleration;
            currentTime = 0f;

            CheckLevelUp();
        }
    }

    void CheckLevelUp()
    {
        if (moveSpeed >= 10.0f && currentLevel < 5) e_level = LevelType.Infinity;
        else if (moveSpeed >= 8.0f && currentLevel < 4) e_level = LevelType.Lv4;
        else if (moveSpeed >= 5.0f && currentLevel < 3) e_level = LevelType.Lv3;
        else if (moveSpeed >= 2.5f && currentLevel < 2) e_level = LevelType.Lv2;

        switch (e_level)
        {
            case LevelType.Lv1:
                SetLevelSystem(1, 6,15f, 0.5f, 0.4f);
                break;
            case LevelType.Lv2:
                SetLevelSystem(2, 5, 15f, 0.55f, 0.35f);
                break;
            case LevelType.Lv3:
                SetLevelSystem(3, 4, 20f, 0.7f, 0.3f);
                break;
            case LevelType.Lv4:
                SetLevelSystem(4, 3, 20f, 0.9f, 0.2f);
                break;
            case LevelType.Infinity:
                SetLevelSystem(5, 3, 25f, 1.0f, 0.1f);
                break;
        }
    }

    private void SetLevelSystem(int _level, int _maxBlockCount,
        float _levelUpTime, float _speedAcceleration, float _createDelay)
    {
        currentLevel = _level;
        maxBlockCount = _maxBlockCount;
        levelUpTime = _levelUpTime;
        speedAcceleration = _speedAcceleration;
        createDelay = _createDelay;
    }

    IEnumerator CreateBlockLoop()
    {
        while (true)
        {
            if (UIManager.Instance == null || !UIManager.Instance.IsGameStarted)
            {
                yield return null; // ´ÙÀ½ ÇÁ·¹ÀÓ±îÁö ´ë±â ÈÄ ´Ù½Ã È®ÀÎ
                continue;
            }

            if (ObjectPool.Instance.ActiveBlockCount < maxBlockCount)
            {
                Block newBlock = ObjectPool.GetObject();
                float ranPos = Random.Range(-4f, 4f);
                float yPos = playerCtrl.transform.position.y + 10f;
                    
                newBlock.transform.position = new Vector3(ranPos, yPos, 0f);
                yield return new WaitForSeconds(createDelay);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}