using UnityEngine;

/// <summary>
/// 界面跟隨3D 物件
/// </summary>
public class UIFollow3DObject : MonoBehaviour
{
    // 3D物件的Transform
    [Header("請拖拽慾跟隨的3D物件")]
    public Transform targetObject;
    // UI的RectTransform
    [Header("請拖拽慾跟隨的UI RectTransform")]
    public RectTransform uiElement;
    // 用於調整UI位置的偏移量
    [Header("UI的位移偏移量")]
    public Vector3 offset;
    // 用於攝影機
    public Camera mainCamera;

    private void Start()
    {
        //取得主攝影機
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        // 將3D物件的世界座標轉換為UI的屏幕座標
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetObject.position + offset);

        // 檢查目標物件是否在攝影機的前方
        if (screenPosition.z > 0)
        {
            //將UI元件的座標設置爲轉換後的熒幕座標
            uiElement.position = screenPosition;
        }
        else
        {

        }
    }

    /// <summary>
    /// 更新目標與位移
    /// </summary>
    /// <param name="_target">目標物件</param>
    /// <param name="_offset">位移</param>
    public void UpdateTargetAndOffset(Transform _target, Vector3 _offset)
    {
        targetObject = _target;
        offset = _offset;
    }
}