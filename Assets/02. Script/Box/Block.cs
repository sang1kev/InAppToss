using System;
using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;

    [SerializeField]private GameObject[] itemCanvs;

    private Animator blockAnim;

    private void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
        blockAnim = GetComponent<Animator>();

        blockAnim.SetBool("isBroke", false);
    }

    private void Update()
    {
        if (UIManager.Instance != null && !UIManager.Instance.IsGameReady) 
            return;
        
        MoveBlock();
    }

    void MoveBlock()
    {
        if (!UIManager.Instance.IsGamePaused)
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
            for (int i = 0; i < itemCanvs.Length; i++)
            {
                itemCanvs[i].SetActive(false);
            }
            soundManager.EffectSoundPlay("BlockBreak");
            StartCoroutine(BrokeCoroutine());
        }

        if (other.CompareTag("DeadZone"))
        {
            for (int i = 0;  i < itemCanvs.Length; i++)
            {
                itemCanvs[i].SetActive(false);
            }
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
