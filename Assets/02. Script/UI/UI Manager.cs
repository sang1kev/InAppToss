using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private PlayerCtrl player;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI noticeText;

    private GameObject startSet;
    private GameObject inGameSet;
    
    private Animator groundAnim;
    
    public bool IsGameStarted { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
        
        startSet = GameObject.Find("Start Set");
        inGameSet = GameObject.Find("InGame");
        groundAnim = GameObject.Find("Ground").GetComponent<Animator>();
        
        IsGameStarted = false;
        groundAnim.gameObject.SetActive(true);
        startSet.SetActive(true);
        inGameSet.SetActive(false);
    }
    
    void Update()
    {
        if (IsGameStarted) 
            return;

        // 모바???�치 체크
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            StartCoroutine(StartGame());
        }

        // PC ?�스?�용 마우???�릭 체크
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        IsGameStarted = true;
    
        startSet.SetActive(false);
        inGameSet.SetActive(true);
        
        float startTime = 3f;

        // 3�?카운?�다??
        while (startTime > 0f)
        {
            noticeText.text = Mathf.Ceil(startTime).ToString();
            startTime -= Time.deltaTime;
            yield return null;
        }

        noticeText.text = "";
    }

}
