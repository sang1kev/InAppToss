using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerCtrl player;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI noticeText;

    private GameObject startSet;
    private GameObject inGameSet;
    private GameObject joyStickUI;
    private Button startButton;
    
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
        joyStickUI = GameObject.Find("JoyStick");
        groundAnim = GameObject.Find("Ground").GetComponent<Animator>();
        startButton = GameObject.Find("Start Button").GetComponent<Button>();
        
        IsGameStarted = false;
        groundAnim.gameObject.SetActive(true);
        startSet.SetActive(true);
        inGameSet.SetActive(false);
        joyStickUI.SetActive(false);
    }
    
    void Update()
    {
        if (IsGameStarted) 
            return;

        startButton.onClick.AddListener(StartPhase);
    }

    void StartPhase()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        IsGameStarted = true;

    
        startSet.SetActive(false);
        inGameSet.SetActive(true);
        
        float startTime = 3f;

        while (startTime > 0f)
        {
            noticeText.text = Mathf.Ceil(startTime).ToString();
            startTime -= Time.deltaTime;
            yield return null;
        }

        joyStickUI.SetActive(true);

        noticeText.text = "";
    }

}
