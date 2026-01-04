using UnityEngine;

///<summary> 
///狀態類別，包含進入、更新和離開狀態的方法
/// </summary>
public class State
{
    //protected 受保護的：允許子類別存取
    protected string name;                  //狀態名稱
    protected StateMachine stateMachine;    //狀態機器
    protected float timer; //計時器

    // virtual 虛擬：允許子類別覆寫此方法
    /// <summary> 進入狀態時的處理
    public virtual void Enter()
    {
        timer = 0f; //重置計時器
    }
    /// <summary> 更新狀態時的處理
    public virtual void Update()
    {
        //累加時間到計時器內
        // Time.deltaTime 每個影格的時間 大約是0.02秒
          timer += Time.deltaTime;
    }
    /// <summary> 離開狀態時的處理
    public virtual void Exit()
    {

    }
}