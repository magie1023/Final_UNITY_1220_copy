using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    //單例模式 Singleton Pattern
    //使用時機：當此脚本只有一個實體物件時，并且其他脚本需要獲得這個脚本時
    //用來存放資料的靜態變數
    private static SceneLoader _instance;
    //唯讀屬性：讓外部取得此資料窗口
    public static SceneLoader instance
    {
        get
        {
            if (_instance == null)                                  //如果實體物件不存在
                _instance = FindAnyObjectByType<SceneLoader>();     //嘗試尋找場景中的實體物件
            return _instance;                                       //回傳實體物件
        }
    }

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField] private Image loadingBar;
    [SerializeField] private CanvasGroup group;

    /// <summary>
    /// 開始非同步載入場景
    /// </summary>
    /// <param name=""sceneName"">場景名稱</param>
    public void AsyncSceneLoader(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));          // 啟動協程來載入場景
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return StartCoroutine(FadeSystem.Fade(group));    //淡入載入畫面

        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName); // 開始非同步載入指定場景並且獲得載入資訊 AsyncOperation
        asyncOperation.allowSceneActivation = false;                                                        // 防止自動啟動場景

        while (!asyncOperation.isDone)                                         // 在場景載入時更新UI
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);     // 計算載入進度（0到0.9，然後在完成時為1）
            if (percentageText != null)                                         // 更新百分比文字和載入條填充量
                percentageText.text = $"{Mathf.RoundToInt(progress * 100)}% ";
            if (loadingBar != null)
                loadingBar.fillAmount = progress;
            if (asyncOperation.progress >= 0.9f)                                // 在完全載入時啟動場景
                asyncOperation.allowSceneActivation = true;
            yield return null;                                                  // 等待下一幀 null
        }
    }
}