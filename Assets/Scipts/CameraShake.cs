using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private static CameraShake _instance;

    public static CameraShake instance
    {
        get
        {
            if (_instance == null) _instance = FindAnyObjectByType<CameraShake>();
            return _instance;
        }    
    }
    //定義 Cinemachine 的虛擬攝影機柏林函數
    private CinemachineBasicMultiChannelPerlin perlin;

    private float amplitudeDefault, frequencyDefault;

    private void Awake()
    {
        //獲取 Cinemachine 虛擬攝影機組件
        perlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
        //儲存初始的振幅和頻率值
        amplitudeDefault = perlin.AmplitudeGain;
        frequencyDefault = perlin.FrequencyGain;
    }

    //啓動攝影機晃動效果的方法
    public void ShakeCamera(float duration, float amplitude, float frequency)
    {
        StartCoroutine(ShakeCoroutine(duration, amplitude, frequency));
    }

    //協同程序，用於執行攝影機晃動效果
    private IEnumerator ShakeCoroutine(float duration, float amplitude, float frequency)
    {
        //如果該組件存在，設定晃動參數
        perlin.AmplitudeGain = amplitude;   //設定振幅
        perlin.FrequencyGain = frequency;   //設定頻率

        //等待晃動持續的時間
        yield return new WaitForSeconds(duration);

        //恢復初始值，停止晃動
        perlin.AmplitudeGain = amplitudeDefault;
        perlin.FrequencyGain = frequencyDefault;
    }
}