using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

///<summary>
///敵人
/// </summary>
public class Enemy : Character
{
    #region 資料
    [field: Header("敵人資料")]
    [field: SerializeField, Tooltip("待機時間範圍")]
    public Vector2 idleTimeRange { get; private set; } = new Vector2(1f, 3f);
    [field: SerializeField, Tooltip("巡邏隨機時間範圍")]
    public Vector2 wanderTimeRange { get; private set; } = new Vector2(3f, 5f);
    [SerializeField, Tooltip("巡邏的中心點")]
    private Vector3 wanderCenter;
    [SerializeField, Tooltip("巡邏的半徑"), Range(0, 10)]
    private float wanderRadius = 5f;
    [SerializeField, Tooltip("追蹤半徑"), Range(0, 15)]
    private float trackRadius = 6.5f;
    [field: SerializeField, Range(0, 10)]
    public float trackSpeed { get; private set; } = 5f;
    [field: SerializeField, Range(0, 5)]
    public float attackRadius { get; private set; } = 1.5f;
    [field: SerializeField, Range(0, 15)]
    public float turnSpeed { get; private set; } = 3f;
    [field: SerializeField, Range(0, 5)]
    public float attackCD { get; private set; } = 3f;

    private LayerMask targetLayer = 1 << 7; //目標圖層 

    /// <summary>
    /// 巡邏的目標點
    /// </summary>
    public Vector3 wanderTarget { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public Transform traPlayer { get; private set; }
    #endregion
    #region 狀態機
    public EnemyIdle idle { get; private set; }
    public EnemyWander wander { get; private set; }
    public EnemyTrack track { get; private set; }
    public EnemyAttack attack { get; private set; }
    public EnemyDead dead { get; private set; }
    #endregion

    [SerializeField]
    private GameObject prefabHp;

    /// <summary>
    /// 群組_所有敵人的血條
    /// </summary>
    private Transform rootHp;

    private UIFollow3DObject uiFollow3D;

    [field: SerializeField]
    public CanvasGroup groupHp { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        //如果碰撞到玩家的攻擊物件 就造成傷害
        if (other.TryGetComponent(out AttackSystemPlayer attackSystem))
        {
            Damage(attackSystem.AttackPower);
        }
    }

    /// <summary>
    /// 選取物件時繪製圖示
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0.5f, 0.3f);
        Gizmos.DrawSphere(wanderCenter, wanderRadius);

        Gizmos.color = new Color(0.5f, 0.3f, 1, 0.8f);
        Gizmos.DrawSphere(wanderTarget, 0.7f);

        Gizmos.color = new Color(1, 0.6f, 0.6f, 0.3f);
        Gizmos.DrawSphere(transform.position, trackRadius);
    }


    protected override void Awake()
    {
        rootHp = GameObject.Find("群組_所有敵人的血條").transform;
        GameObject tempHp = Instantiate(prefabHp, rootHp);
        imgHp = tempHp.transform.Find("圖片_血條_敵人").GetComponent<Image>();
        textHp = tempHp.transform.Find("文字_血量_敵人").GetComponent<TMPro.TMP_Text>();
        groupHp = tempHp.GetComponent<CanvasGroup>();

        uiFollow3D = tempHp.GetComponent<UIFollow3DObject>();
        uiFollow3D.UpdateTargetAndOffset(transform, new Vector3(0, 2.5f, 0));

        base.Awake();

       // Debug.Log($"血條 prefab: {prefabHp}, root: {rootHp}, imgHp: {imgHp}, groupHp: {groupHp}");


        agent = GetComponent<NavMeshAgent>();
        traPlayer = GameObject.Find("角色").transform;

        //訂閲玩家死亡事件 並切換到待機狀態
        Player.instance.onDead += () =>
        {
            stateMachine.SwitchState(idle);
        };

        //狀態機初始化
        stateMachine = new StateMachine();
        //實例化狀態
        idle = new EnemyIdle(this, stateMachine, $"{name} 待機");
        wander = new EnemyWander(this, stateMachine, $"{name} 巡邏");
        track = new EnemyTrack(this, stateMachine, $"{name} 追蹤");
        attack = new EnemyAttack(this, stateMachine, $"{name} 攻擊");
        dead = new EnemyDead(this, stateMachine, $"{name} 死亡");
        //狀態機啓動
        stateMachine.Initialize(idle);
    }

    private void Update()
    {
        stateMachine.Update();

        //Debug.Log($"<color=#6f6>玩家是否在追蹤範圍内{CheckPlayerInTrackRange()}</color>");
    }

    /// <summary>
    /// 獲得巡邏目標點
    /// </summary>
    public void SetWanderTarget()
    {
        //在圓形範圍内隨機選取一個點作為巡邏目標點
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        wanderTarget = wanderCenter + new Vector3(randomPoint.x, 0, randomPoint.y);
        //判斷是否在導覽網格内
        if(NavMesh.SamplePosition(wanderTarget, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            wanderTarget = hit.position;
    }

    /// <summary>
    /// 檢查玩家是否在追蹤範圍内
    /// </summary>
    public bool CheckPlayerInTrackRange()
    {
        //以巡邏中心點為圓心，追蹤半徑為半徑，檢查目標圖層是否有目標物件
        Collider[] results = Physics.OverlapSphere(
            wanderCenter, trackRadius, targetLayer);
        return results.Length > 0;
    }

    /// <summary>
    /// 面向玩家
    /// </summary>
    public void LookAtPlayer()
    {
        Vector3 direction = (traPlayer.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            lookRotation, Time.deltaTime * turnSpeed);
    }

    protected override void Damage(float damage)
    {
        base.Damage(damage);
        if (hp <= 0) return;
        StartCoroutine(FadeSystem.Fade(groupHp));
        CameraShake.instance.ShakeCamera(0.2f, 7, 10f);
        StartCoroutine(DamageEffect(0.3f, 0.2f));
    }

    protected override void Dead()
    {
        base.Dead();
        StopAllCoroutines();
        StartCoroutine(FadeSystem.Fade(groupHp, false));
        GameFlowManager.instance.KillEnemy();
    }

}