using UnityEngine;

///<summary>
/// 玩家跳躍
///</summary>
public class PlayerJump : PlayerState
{
    public PlayerJump(StateMachine stateMachine, Player player, string name) : base(stateMachine, player, name)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //添加向上的加速度
        // 保留原本的水平速度，僅設定垂直分量為跳躍高度
        Vector3 current = player.rig.linearVelocity;
        player.SetVelocity(new Vector3(current.x, player.jumpHeight, current.z));
        //勾選開關跳躍
        player.ani.SetBool(player.parJump, true);
        //設定重力浮點數為 1
        player.ani.SetFloat(player.parGravity, 1);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //設定剛體速度 為 右向 * 水平輸入 * 走路速度 + 前向 * 垂直輸入 * 走路速度
        player.SetVelocity(
            player.transform.right * inputH * player.walkSpeed +
            player.transform.forward * inputV * player.walkSpeed +
            player.transform.up * player.rig.linearVelocity.y);

        //面向攝影機
        player.LookAtCamera();

        #region 條件區域


        //如果 玩家的重力 小於0 就切換到下落狀態
        if (player.rig.linearVelocity.y < 0)
            stateMachine.SwitchState(player.fall);
        #endregion
    }
}
