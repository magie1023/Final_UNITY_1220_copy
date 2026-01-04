using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戲流程管理器：管理游戲的主要流程和狀態
/// 游戲勝利與失敗、重新與退出處理
/// </summary>
public class GameFlowManager : MonoBehaviour
{
    private static GameFlowManager _instance;
    public static GameFlowManager instance
    {
        get
        {
            if (_instance == null) _instance = FindAnyObjectByType<GameFlowManager>(); 
            return _instance;
        }
    }

    private CanvasGroup groupFinish;
    private TMP_Text textFinish;
    private Button btnReplay, btnQuit;
    private TMP_Text textEnemyCount;
    private int enemyCountMax;
    private int enemyCountKill;

    private void Awake()
    {
        groupFinish = GameObject.Find("群組_結束畫面").GetComponent<CanvasGroup>();
        textFinish = GameObject.Find("文字_結束標題").GetComponentInChildren<TMP_Text>();
        btnReplay = GameObject.Find("按鈕_重新挑戰").GetComponent<Button>();
        btnQuit = GameObject.Find("按鈕_退出").GetComponent<Button>();
        textEnemyCount = GameObject.Find("文字_清除所有敵人").GetComponentInChildren<TMP_Text>();
        enemyCountMax = GameObject.FindGameObjectsWithTag("敵人").Length;
        textEnemyCount.text = $"清除所有敵人 ： 0 / {enemyCountMax}";

        btnReplay.onClick.AddListener(() =>
        {
            SceneLoader.instance.AsyncSceneLoader("游戲場景");
        });

        btnQuit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    ///<summary>
    ///擊殺敵人
    ///</summary>
    public void KillEnemy()
    {
        enemyCountKill++;
        textEnemyCount.text = $"清除所有敵人 ： {enemyCountKill} / {enemyCountMax}";

        if (enemyCountKill >= enemyCountMax)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            ShowFinish("任務完成！");
        }
    }

    /// <summary>
    /// 顯示結束畫面
    /// </summary>
    /// <param name="title">結束標題</param>
    public void ShowFinish(string title)
    {
        textFinish.text = title;
        StartCoroutine(FadeSystem.Fade(groupFinish));
    }
}
