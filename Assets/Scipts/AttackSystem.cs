// 1/4/2026 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    [Range(1, 100)]
    [SerializeField]
    private float attackPower = 100; // 可調整的攻擊力，範圍介於1到100之間

    // 公開唯讀屬性，提供攻擊力的值
    public float AttackPower
    {
        get { return attackPower; }
    }
}