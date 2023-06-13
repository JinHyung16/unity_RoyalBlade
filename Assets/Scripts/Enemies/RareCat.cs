using UnityEngine;
using UnityEngine.Pool;

public class RareCat : BaseEnemy
{
    [SerializeField] private RareCatData rareCatData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private static readonly int onHit = Animator.StringToHash("onHit");

    private Rigidbody2D rigid2D;
    private Animator animator;

    //UnityEngine.Pool 관련 데이터
    private IObjectPool<RareCat> managedPool;
    private bool isPoolRelease = false;

    private void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        rareCatData.OnAfterDeserialize();
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
    public void SetManagedPool(IObjectPool<RareCat> poolObj)
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
        rareCatData.enemyHpRuntime -= damage;
        OnBounce();
        GameScenePresenter.GetInstance.ScoreUpdate(1.8f);
        if (rareCatData.enemyHpRuntime <= 0)
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
