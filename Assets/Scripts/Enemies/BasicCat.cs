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
    private bool isPoolRelease = false;

    private void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        basicCatData.OnAfterDeserialize();
        isPoolRelease = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Shield"))
        {
            Debug.Log(collision.tag);
            OnBounce();
        }
    }

    #region UnityEngine.Pool Functions
    public void SetManagedPool(IObjectPool<BasicCat> poolObj)
    {
        managedPool = poolObj;
    }

    public void DestroyManagedPool()
    {
        if (!isPoolRelease)
        {
            managedPool.Release(this);
            isPoolRelease = true;
        }
    }
    #endregion
    public override void OnDamge(int damage)
    {
        Debug.Log("검에 맞고 있습니다");
        basicCatData.enemyHpRuntime -= damage;
        OnBounce();
        GameScenePresenter.GetInstance.ScoreUpdate(1.2f);
        if (basicCatData.enemyHpRuntime <= 0)
        {
            OnDied();
        }
    }

    private void OnDied()
    {
        this.DestroyManagedPool();
    }

    /// <summary>
    /// Player가 쉴드로 밀쳐 밀쳐진 Enemy거나
    /// 혹은 밀쳐지는 상황에서 본인들끼리 충돌할 경우 호출
    /// </summary>
    private void OnBounce()
    {
        animator.SetTrigger(onHit);
        rigid2D.AddForce(Vector2.up * bouncePower, ForceMode2D.Impulse);
    }
}
