using System.Collections;
using UnityEngine;

///<summary> 處理 Canvas Group 畫布群組元件淡入與淡出的系統
public class FadeSystem
{
    // static 代表此方法屬於類別本身，而不是類別的實例
    //呼叫方式：StartCoroutine(FadeSystem.Fade(参数1，参数2，参数3));
    /// </summary> 淡入淡出
    public static IEnumerator Fade(CanvasGroup group, bool fadeIn = true, float interval = 0.03f)
    {
        //如果FadeIn為真，則 increase 設為 0.1，否則設為 -0.1
        var increase = fadeIn ? 0.1f : -0.1f;

        //進行 10 次淡入或淡出
        for (int i = 0; i < 10; i++)
        {
            group.alpha += increase;
            yield return new WaitForSeconds(interval);
        }

        //設定畫布群組元件的互動性和阻擋射線屬性
        group.interactable = fadeIn;
        group.blocksRaycasts = fadeIn;
    }
}
