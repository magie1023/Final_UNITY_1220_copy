using UnityEngine;

///<summary>
///敵人狀態
/// </summary>
public class EnemyState : State
{
    protected Enemy enemy;

    public EnemyState(Enemy enemy, StateMachine stateMachine, string name)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.name = name;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log($"<color=#f77>進入狀態：{name}</color>");
    }
}