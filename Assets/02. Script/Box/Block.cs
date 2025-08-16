using System;
using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private UIManager uiManager;

    private Animator blockAnim;

    private void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
        uiManager = FindAnyObjectByType<UIManager>();
        blockAnim = GetComponent<Animator>();

        blockAnim.SetBool("isBroke", false);
    }

    private void Update()
    {
        if (uiManager != null && !uiManager.IsGameReady) 
            return;
        
        MoveBlock();
    }

    void MoveBlock()
    {
        if (!uiManager.IsGamePaused && !uiManager.IsGameOver)
        {
            transform.position += Vector3.down * BlockManager.moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            soundManager.EffectSoundPlay("BlockBreak");
            StartCoroutine(BrokeCoroutine());
        }

        if (other.CompareTag("DeadZone"))
        {
            blockAnim.SetBool("isBroke", false);
            ObjectPool.Instance.ReturnObject(this);
        }
    }

    IEnumerator BrokeCoroutine()
    {
        blockAnim.SetBool("isBroke", true);
        yield return new WaitForSeconds(0.3f);

        blockAnim.SetBool("isBroke", true);
        yield return new WaitForSeconds(0.2f);
        ObjectPool.Instance.ReturnObject(this);
    }
}
