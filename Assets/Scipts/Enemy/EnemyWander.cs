using UnityEngine;

///<summary>
///敵人巡邏
///</summary>
public class EnemyWander : EnemyState
{
    private float wanderTime;

    public EnemyWander(Enemy enemy, StateMachine stateMachine, string name) : base(enemy, stateMachine, name)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //設定代理器速度為巡邏速度
        enemy.agent.speed = enemy.walkSpeed;

        //設定巡邏目標點
        enemy.SetWanderTarget();
        //在巡邏時間範圍内隨機一個巡邏時間
        wanderTime = Random.Range(enemy.wanderTimeRange.x, enemy.wanderTimeRange.y);
        //Debug.Log($"<color=#f7f>巡邏時間：{wanderTime} </color>");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //設定代理器目標為巡邏目標點
        enemy.agent.SetDestination(enemy.wanderTarget);
        //設定動畫垂直參數爲代理器速度百分比
        enemy.ani.SetFloat(enemy.parVertical, 
            enemy.agent.velocity.magnitude / enemy.agent.speed);

        #region 條件區域
        //如果計時器大於巡邏時間就切換到待機狀態
        if (timer > wanderTime) stateMachine.SwitchState(enemy.idle);

        //如果玩家進入追蹤範圍就切換到追蹤狀態
        if (enemy.CheckPlayerInTrackRange()) stateMachine.SwitchState(enemy.track);
        else enemy.StartCoroutine(FadeSystem.Fade(enemy.groupHp, false));
        #endregion
    }
}

