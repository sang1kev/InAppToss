using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MapChange : MonoBehaviour
{
    public GameObject[] maps;
    public GameObject fadeImage;

    private int currentIndex;
    
    private bool isFading;

    private void Start()
    {
        if(fadeImage == null) Debug.Log("fadeImage is null");
        
        currentIndex = 0;

        SetFadeAlpha(0);
    }

    public void ChangeMaps(int index)
    {
        if (maps == null || maps.Length == 0) return;
        if (isFading) return;
        if (currentIndex >= maps.Length) currentIndex = 0;
        
        StartCoroutine(FadeRoutine(index));
    }

    IEnumerator FadeRoutine(int index)
    {
        SetFadeAlpha(0);
        for (int i = 0; i < maps.Length; i++)
            maps[i].SetActive(false);
        
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / 1f);
            SetFadeAlpha(alpha);
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        currentIndex = index;
        maps[currentIndex].SetActive(true);
        
        elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsed / 1f);
            SetFadeAlpha(alpha);
            yield return null;
        }

        SetFadeAlpha(0);
    }
    
    void SetFadeAlpha(float alpha)
    {
        var color = fadeImage.GetComponent<SpriteRenderer>().color;
        color.a = alpha;
        fadeImage.GetComponent<SpriteRenderer>().color = color;
    }
}
