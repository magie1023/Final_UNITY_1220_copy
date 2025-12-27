using UnityEngine;
using UnityEngine.UI;

//繼承 MonoBehaviour 允許此類別挂在游戲物件上
///<summary> 
///主選單管理器
///繼續游戲、開始游戲、選項、製作團隊與退出按鈕
///</summary>
public class MainMenuManager : MonoBehaviour
{
    //private 代表只能在此類別内存取并且隱藏
    //允許在 Unity 編輯器中設置私有變數
    [SerializeField] private Button btnLoad;     //繼續游戲按鈕
    [SerializeField] private Button btnNew;      //開始游戲按鈕
    [SerializeField] private Button btnOptions;  //選項按鈕
    [SerializeField] private Button btnCredit;  //製作團隊按鈕
    [SerializeField] private Button btnQuit;     //退出按鈕
    [SerializeField] private Button btnBackOption;
    [SerializeField] private Button btnBackCredits;
    [SerializeField] private CanvasGroup groupOption;
    [SerializeField] private CanvasGroup groupCredit;

    private void Awake()
    {
        //Debug.Log("哈嘍，沃德 :D");
        //為按鈕添加點擊事件監聽器
        //使用 Lambda 表達式來調用方法
        //StartCoroutine 用於啟動協程
        //控制界面的淡入與淡出
        btnOptions.onClick.AddListener(() => StartCoroutine(FadeSystem.Fade(groupOption, interval: 0.05f)));
        btnBackOption.onClick.AddListener(() => StartCoroutine(FadeSystem.Fade(groupOption, false)));
        btnCredit.onClick.AddListener(() => StartCoroutine(FadeSystem.Fade(groupCredit, interval: 0.05f)));
        btnBackCredits.onClick.AddListener(() => StartCoroutine(FadeSystem.Fade(groupCredit, false)));
        //點擊退出按鈕時退出應用程序
        //Application.Quit() 會在編輯器中停止播放模式，在打包后的應用程序中退出程序
        btnQuit.onClick.AddListener(() =>
        {
            Application.Quit();
            Debug.Log("<color=#ff3>退出游戲</color>");
        });
        // 點擊開始游戲按鈕時，載入游戲場景
        btnNew.onClick.AddListener(() => SceneLoader.instance.AsyncSceneLoader("游戲場景"));

    }
}