using UnityEngine;

///<summary> 狀態機
public class StateMachine
{
    //記錄當前狀態
    private State currentState;

    //功能 = 方法 = 函式 = 函數 | method , function
    /// <summary> 
    /// 初始化狀態機
    /// </summary>
    /// <param name="firstState">第一個狀態</param>
    public void Initialize(State firstState)
    {
        //指定當前狀態
        currentState = firstState;
        //進入當前狀態
        currentState.Enter();
    }
    ///<summary> 
    ///切換狀態：先退出原本狀態進入新狀態
    ///</summary>
    ///param name="newState">新狀態</param>
    public void SwitchState(State newState)
    {
        //離開當前狀態
        currentState.Exit();
        //指定新狀態為當前狀態
        currentState = newState;
        //進入當前狀態
        currentState.Enter();
    }
    
    /// <summary> 
    /// 更新狀態：持續執行當前的狀態
    /// </summary>
    public void Update()
    {
        //更新當前狀態
        currentState.Update();
    }
}
