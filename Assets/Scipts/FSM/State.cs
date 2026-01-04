using UnityEngine;

///<summary> 
///狀態類別，包含進入、更新和離開狀態的方法
/// </summary>
public class State
{
    //protected 受保護的：允許子類別存取
    protected string name;                  //狀態名稱
    protected StateMachine stateMachine;    //狀態機器
    
    // virtual 虛擬：允許子類別覆寫此方法
    /// <summary> 進入狀態時的處理
    public virtual void Enter()
    {

    }
    /// <summary> 更新狀態時的處理
    public virtual void Update()
    {

    }
    /// <summary> 離開狀態時的處理
    public virtual void Exit()
    {

    }
}