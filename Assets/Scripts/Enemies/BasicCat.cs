using UnityEngine;
using UnityEngine.Pool;

public class BasicCat : BaseEnemy
{
    [SerializeField] private BasicCatData basicCatData;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private static readonly int onHit = Animator.StringToHash("onHit");

    private Rigidbody2D rigid2D;
    private Animator animator;


    //UnityEngine.Pool 관련 데이터
    private IObjectPool<BasicCat> managedPool;

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        basicCatData.OnAfterDeserialize();
        rigid2D.bodyType = RigidbodyType2D.Dynamic;
    }
    private void OnEnable()
    {
        basicCatData.OnAfterDeserialize();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shield"))
        {
            OnKnockBackByShield();
        }
        if (collision.CompareTag("Sword"))
        {
            AudioManager.GetInstance.EnemySFXPlay();
            OnKnockBackBySword();
        }
        if (collision.CompareTag("DieZone"))
        {
            this.DestroyManagedPool();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Shield"))
        {
            OnKnockBackByShield();
        }
        if (collision.CompareTag("Sword"))
        {
            AudioManager.GetInstance.EnemySFXPlay();
            OnKnockBackBySword();
        }
    }

    #region UnityEngine.Pool Functions
    public void SetManagedPool(IObjectPool<BasicCat> poolObj)
    {
        managedPool = poolObj;
    }

    public void DestroyManagedPool()
    {
        managedPool.Release(this);
    }
    #endregion

    public override void OnDamge(int damage)
    {
        HitDamagePoolManager.GetInstance.GetHitDamageText(this.transform, damage.ToString());
        OnKnockBackByShield();
        basicCatData.enemyHpRuntime -= damage;

        if (basicCatData.enemyHpRuntime <= 0)
        {
            OnDied();
        }
    }
    private void OnDied()
    {
        GameScenePresenter.GetInstance.UpdateScore();
        this.DestroyManagedPool();
    }

    private void OnKnockBackBySword()
    {
        rigid2D.AddForce(Vector2.up * knockBackSwordPower, ForceMode2D.Impulse);
    }
    
    /// <summary>
    /// Player가 쉴드로 밀쳐 밀쳐진 Enemy거나
    /// 혹은 밀쳐지는 상황에서 본인들끼리 충돌할 경우 호출
    /// </summary>
    private void OnKnockBackByShield()
    {
        animator.SetTrigger(onHit);
        rigid2D.AddForce(Vector2.up * knockBackShieldPower, ForceMode2D.Impulse);
    }
}
