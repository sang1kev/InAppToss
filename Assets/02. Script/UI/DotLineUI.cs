using System.Collections.Generic;
using UnityEngine;

public class DotLineUI : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private int maxDots = 10;     // 최대 점 개수
    [SerializeField] private float dotSpacing = 30f; // 점 간격 (픽셀)

    private List<RectTransform> dotsDash = new List<RectTransform>();
    private List<RectTransform> dotsNoDash = new List<RectTransform>();
    private Camera mainCam;
    private PlayerCtrl playerCtrl;

    void Start()
    {
        mainCam = Camera.main;
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();

        // 점 미리 생성
        for (int i = 0; i < maxDots; i++)
        {
            GameObject dotDash = Instantiate(dotPrefab, transform);
            dotDash.SetActive(false);
            dotsDash.Add(dotDash.GetComponent<RectTransform>());
        }
        for (int i = 0; i < maxDots; i++)
        {
            GameObject dotNoDash = Instantiate(dotPrefab, transform);
            dotNoDash.SetActive(false);
            dotsNoDash.Add(dotNoDash.GetComponent<RectTransform>());
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

        //List<RectTransform> dots = playerCtrl.ISDashAvail ? dotsDash : dotsNoDash;

        for (int i = 0; i < maxDots; i++)
        {
            if (i < activeDots)
            {
                dotsDash[i].gameObject.SetActive(true);
                Vector3 pos = startScreenPos + direction * (i * dotSpacing);
                
                dotsDash[i].position = pos;
            }
            else
            {
                dotsDash[i].gameObject.SetActive(false);
            }
        }
    }

    public void HideDots()
    {
        //List<RectTransform> dots = playerCtrl.ISDashAvail ? dotsDash : dotsNoDash;
        foreach (var dot in dotsDash)
        {
            dot.gameObject.SetActive(false);
        }
    }
}
