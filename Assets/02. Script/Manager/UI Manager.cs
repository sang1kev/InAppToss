using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private ScoreManager scoreManager;
    private PlayerCtrl player;

    [SerializeField] private TextMeshProUGUI inGame_noticeText;
    [SerializeField] private TextMeshProUGUI inGame_currScoreText;
    [SerializeField] private TextMeshProUGUI inGame_maxScoreText;

    [SerializeField] private TextMeshProUGUI Die_currScoreText;
    [SerializeField] private TextMeshProUGUI Die_maxScoreText;

    [SerializeField] private GameObject startSet;
    [SerializeField] private GameObject inGameSet;
    [SerializeField] private GameObject DieSet;
    [SerializeField] private GameObject joyStickUI;

    private bool hasDieUIShown = false;
    private float startTime = 3f;

    public bool IsGameStarted { get; private set; }

    private void Awake()
    {
        Instance = this;
        IsGameStarted = false;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();

        Init(false, true, false, false, false);
    }

    private void Update()
    {
        if (!IsGameStarted || player == null || hasDieUIShown)
            return;

        UpdateText();

        if (player.ISDead)
        {
            ShowDieUI();
        }
    }

    void Init(bool _gameStart, bool _start, bool _inGame, bool _die, bool _joyStick)
    {
        IsGameStarted = _gameStart;
        startSet.SetActive(_start);
        inGameSet.SetActive(_inGame);
        DieSet.SetActive(_die);
        joyStickUI.SetActive(_joyStick);
    }

    public void StartPhase()
    {
        if (IsGameStarted)
            return;

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        Init(true, false, true, false, false);

        startTime = 3f;

        while (startTime > 0f)
        {
            inGame_noticeText.text = Mathf.Ceil(startTime).ToString();
            startTime -= Time.deltaTime;
            yield return null;
        }

        joyStickUI.SetActive(true);

        inGame_noticeText.text = "";
    }

    private void UpdateText()
    {
        inGame_currScoreText.text = scoreManager.currScore.ToString() + " m";
        inGame_maxScoreText.text = scoreManager.maxScore.ToString() + " m";
    }

    private void ShowDieUI()
    {
        Init(false, false, false, true, false);
        Die_currScoreText.text = scoreManager.currScore.ToString() + " m";
        Die_maxScoreText.text = scoreManager.maxScore.ToString() + " m";

        hasDieUIShown = true;
    }

    public void RestartGame()
    {
        scoreManager.ResetScore();
        SceneManager.LoadScene(0);
    }
}
