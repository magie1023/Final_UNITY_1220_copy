using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///<summary>
///角色：角色基本資料與功能
///</summary>
//abstract 抽象 : 不能被實例化的類別，只能被繼承(不會放在任何物件上)
public abstract class  Character : MonoBehaviour
{
    #region 基本資料
    [field: Header("角色資料")]
    [field: SerializeField, Range(0, 10)]
    public float walkSpeed { get; private set; } = 2.5f;
    [SerializeField, Range(0, 500)]
    protected float hpMax = 500;

    protected float hp;

    public Animator ani { get; private set; }
    public Rigidbody rig { get; private set; }
    public string parHorizontal { get; private set; } = "水平";
    public string parVertical { get; private set; } = "垂直";
    public string parTriggerAttack { get; private set; } = "觸發攻擊";
    public string parTriggerDead { get; private set; } = "觸發死亡";

    public StateMachine stateMachine { get; protected set; }
    #endregion

    [SerializeField]
    protected Image imgHp;
    [SerializeField]
    protected TMP_Text textHp;
    [SerializeField]

    protected AudioClip soundHurt, soundDeath;

    protected virtual void Awake()
    {
        ani = GetComponent<Animator>();     //取得動畫原件
        rig = GetComponent<Rigidbody>();    //取得剛體原件

        hp = hpMax;                        //血量等於最大血量
        textHp.text = $"{hp} / {hpMax}";    //更新血量文字
    }

    ///<summary>
    ///受傷
    ///</summary>
    /// <param name="damage">傷害值</param>
    protected virtual void Damage(float damage)
    {
        if(hp <= 0)return;               //如果已經死亡就不執行下面的程式碼

        hp -= damage;                    //扣血
        hp = Mathf.Clamp(hp, 0, hpMax);  //限制血量在 0 ~ 最大值 之間
        imgHp.fillAmount = hp / hpMax;   //更新血條
        textHp.text = $"{hp} / {hpMax}"; //更新血量文字
        if (hp <= 0) Dead();             //死亡
        Debug.Log($"<color=#66f>{gameObject.name}，剩餘血量：{hp}</color>");
        SoundManager.instance.PlaySound(soundHurt, 0.7f, 1.3f);         //播放受傷音效
    }

    ///<summary>
    ///死亡
    ///</summary>
    protected virtual void Dead()
    {
        ani.SetTrigger(parTriggerDead);
        rig.isKinematic = true; //死亡後剛體改爲質量守恆
        enabled = false;      //停用此腳本
        SoundManager.instance.PlaySound(soundDeath, 0.7f, 1.3f);        //播放死亡音效
    }

    ///<summary>
    ///受傷效果：卡肉感
    ///</summary>
    protected IEnumerator DamageEffect(float timeScale, float duration)
    {
        Time.timeScale = timeScale;                     //時間變慢
        yield return new WaitForSeconds(duration);      //等待0.2秒
        Time.timeScale = 1f;                            //恢復正常速度
    }

    ///<summary>
    ///播放音效
    ///</summary>
    ///<param name="sound">音效檔</param>"
    protected void PlaySound(AudioClip sound)
    {
        SoundManager.instance.PlaySound(sound, 0.8f, 1.2f);
    }
}