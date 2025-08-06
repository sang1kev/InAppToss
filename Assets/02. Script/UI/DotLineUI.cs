using System.Collections.Generic;
using UnityEngine;

public class DotLineUI : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private int maxDots = 20;     // 최대 점 개수
    [SerializeField] private float dotSpacing = 30f; // 점 간격 (픽셀)

    private List<RectTransform> dots = new List<RectTransform>();
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        // 점 미리 생성
        for (int i = 0; i < maxDots; i++)
        {
            GameObject dot = Instantiate(dotPrefab, transform);
            dot.SetActive(false);
            dots.Add(dot.GetComponent<RectTransform>());
        }
    }

    public void UpdateDots(Vector3 worldStartPos, Vector3 worldEndPos)
    {
        // 캐릭터 월드 좌표 → UI 좌표로 변환
        Vector3 startScreenPos = mainCam.WorldToScreenPoint(worldStartPos);
        Vector3 endScreenPos = mainCam.WorldToScreenPoint(worldEndPos);

        Vector3 direction = (endScreenPos - startScreenPos).normalized;

        float distance = Vector3.Distance(startScreenPos, endScreenPos);

        int activeDots = Mathf.Min(maxDots, Mathf.FloorToInt(distance / dotSpacing));

        for (int i = 0; i < maxDots; i++)
        {
            if (i < activeDots)
            {
                dots[i].gameObject.SetActive(true);
                Vector3 pos = startScreenPos + direction * (i * dotSpacing);
                
                dots[i].position = pos;
            }
            else
            {
                dots[i].gameObject.SetActive(false);
            }
        }
    }

    public void HideDots()
    {
        foreach (var dot in dots)
        {
            dot.gameObject.SetActive(false);
        }
    }
}
