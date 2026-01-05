using System;
using UnityEngine;


///<summary>
///玩家類別：記錄玩家資料與相關功能
/// </summary>
public class Player : Character
{
    private static Player _instance;
    public static Player instance
    {
        get
        {
            if (_instance == null) _instance = FindAnyObjectByType<Player>();
            return _instance;
        }
    }
    public event Action onDead;

    #region 玩家資料
    // 唯讀屬性：讓外部取得此資料窗口但不能修改
    // 序列化：讓私有欄位可以在編輯器中顯示與修改
    [field: Header("玩家資料")]
    //[field: SerializeField, Range(0, 10)]
   // public float walkSpeed { get; private set; } = 2.5f;
    [field: SerializeField, Range(3, 15)]
    public float runSpeed { get; private set; } = 5f;
    [field: SerializeField, Range(0, 20)]
    public float jumpHeight { get; private set; } = 7.5f;
    [field: SerializeField, Range(0, 30)]
    public float turnSpeed { get; private set; } = 15f;
    [field: SerializeField, Range(0, 3), Tooltip("中斷攻擊連段的時間")]
    public float BreakComboTime { get; private set; } = 1f;

   // public Animator ani { get; private set; }
   // public Rigidbody rig { get; private set; }  

   // public string parHorizontal { get; private set; } = "水平";
   // public string parVertical { get; private set; } = "垂直";
    public string parGravity { get; private set; } = "重力";
    public string parJump { get; private set; } = "開關跳躍";
    public string parAttackCombo { get; private set; } = "攻擊段數";
   // public string parTriggerAttack { get; private set; } = "觸發攻擊";
  //  public string parTriggerDead { get; private set; } = "觸發死亡";

    private Transform mainCam;
    #endregion

    #region 狀態資料
   // public StateMachine stateMachine { get; private set; }
    public PlayerIdle idle { get; private set; }
    public PlayerWalk walk { get; private set; }
    public PlayerRun run { get; private set; }
    public PlayerJump jump { get; private set; }
    public PlayerFall fall { get; private set; }
    public PlayerAttack attack { get; private set; }
    public PlayerDead dead { get; private set; }
    #endregion

    #region 檢查資料
    [Header("檢查資料")]
    [SerializeField, Range(0, 1)]
    private float groundCheckRadius = 0.2f;          //地面檢查半徑
    [SerializeField, Range(-2, 2)]
    private float groundCheckOffsetY;        //地面檢查 Y 軸偏移
    [SerializeField]
    private LayerMask layerCanJump;          //可跳躍圖層
    #endregion

    //ODGS 選取后繪製圖示
    private void OnDrawGizmosSelected()
    {
        //決定顔色
        Gizmos.color = new Color(0.5f, 1, 0.5f, 0.5f);
        //繪製球體
        Gizmos.DrawSphere(
            transform.position + new Vector3(0, groundCheckOffsetY, 0),
            groundCheckRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
     // 如果碰到物件 嘗試取得敵人攻擊物件 有資料 就造成傷害
        if (other.TryGetComponent(out AttackSystemEnemy attackObject))
        {
            Damage(attackObject.AttackPower);
        }
    }

    protected override void Awake()
    {
        base.Awake();                     //繼承父類別的Awake方法
        HideMouse();                            //隱藏滑鼠

       // ani = GetComponent<Animator>();         //取得動畫元件
       // rig = GetComponent<Rigidbody>();        //取得剛體元件

        mainCam = Camera.main.transform;       //取得主攝影機的變形元件 (貼 MainCamera 標簽)

        #region 狀態實例化
        // 實例化 new 該類別：讓此類別不用挂在物件上也可以在場景内執行
        stateMachine = new StateMachine();
        idle = new PlayerIdle(stateMachine, this, $"{name} 待機");
        walk = new PlayerWalk(stateMachine, this, $"{name} 走路");
        run = new PlayerRun(stateMachine, this, $"{name} 跑步");
        jump = new PlayerJump(stateMachine, this, $"{name} 跳躍");
        fall = new PlayerFall(stateMachine, this, $"{name} 落下");
        attack = new PlayerAttack(stateMachine, this, $"{name} 攻擊");
        dead = new PlayerDead(stateMachine, this, $"{name} 死亡");
        #endregion

        //初始化狀態機 為 待機狀態
        stateMachine.Initialize(idle);
    }

    private void Update()
    {
        stateMachine.Update();// 狀態機更新（邏輯、輸入相關）
        InputAttack();        //呼叫輸入攻擊方法

        // Debug.Log(CanJump());
    }

    /// <summary>
    /// 設定加速度
    /// </summary>
    /// <param name="direction">加速度方向</param>
    public void SetVelocity(Vector3 direction)
    {
        //剛體 的 綫性加速度 = 方向
        rig.linearVelocity = direction;
    }

    ///<summary>
    ///面向攝影機
    ///</summary>
    public void LookAtCamera()
    {
        //建立一個新的四元數 只使用攝影機的 Y 軸角度
        Quaternion camAngle = Quaternion.Euler(0, mainCam.eulerAngles.y, 0);
        //使用插值方式 讓玩家角度慢慢轉向攝影機角度
        transform.rotation = Quaternion.Slerp(transform.rotation, camAngle, turnSpeed * Time.deltaTime);
    }

    ///<summary>
    ///隱藏滑鼠
    ///</summary>
    public void HideMouse()
    {
        //隱藏滑鼠并鎖定在游戲視窗中心
        Cursor.visible = false;                 
        Cursor.lockState = CursorLockMode.Locked;
    }

    ///<summary>
    ///能否跳躍：檢查是否碰到可跳躍圖層物件
    ///</summary>
    public bool CanJump()
    {
        //檢查是否在地面上
        return Physics.CheckSphere(
            transform.position + new Vector3(0, groundCheckOffsetY, 0),
            groundCheckRadius, layerCanJump);
    }

 

    /// <summary>
    /// 輸入攻擊按鍵并進入攻擊狀態
    /// </summary>
    private void InputAttack()
    {
        // 如果在攻擊中就跳出
        if (attack.isAttacking) return;

        //如果 按下滑鼠左鍵 並且 可以跳躍（在地面上）
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanJump())
        {
            // 狀態機 切換到 攻擊狀態
            stateMachine.SwitchState(attack);
        }
    }

    protected override void Damage(float damage)
    {
        base.Damage(damage);
        CameraShake.instance.ShakeCamera(0.2f, 7, 10f);
        StartCoroutine(DamageEffect(0.5f, 0.2f));

    }

    protected override void Dead()
    {
        base.Dead();
        gameObject.layer = 0; //死亡後設置為預設圖層
        onDead?.Invoke(); //觸發死亡事件(?指的是如果有訂閱此事件才觸發)

        Cursor.visible = true; //顯示滑鼠
        Cursor.lockState = CursorLockMode.None; //取消鎖定滑鼠
        GameFlowManager.instance.ShowFinish("逃脫失敗!"); 
    }
}
