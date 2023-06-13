using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviourController : MonoBehaviour
{
    private Rigidbody2D rigid2D;
    private Animator animator;

    private static readonly int onAttack = Animator.StringToHash("onAttack");
    private static readonly int onDefense = Animator.StringToHash("onDefense");
    private static readonly int onJump = Animator.StringToHash("onJump");
    private static readonly int blendAttack = Animator.StringToHash("IsAttack");

    [SerializeField] private PlayerData playerData;
    [SerializeField] private ParticleSystem particleObject;

    [Header("Ground Check")]
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask groundLayer;

    private float jumpPower;
    private float jumpSpecialPower;
    private float bouncePower;

    [HideInInspector] public bool isDefend = false;
    public bool IsGround { get; private set; } = false;

    private void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        jumpPower = playerData.jumpPower;
        jumpSpecialPower = playerData.jumpSpecialPower;
        bouncePower = playerData.bouncePower;

        IsGround = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Ground"))
        {
            IsGround = true;
        }

        if (collision.collider.gameObject.CompareTag("Enemy") && !isDefend)
        {
            OnDamaged();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Ground"))
        {
            IsGround = false;
        }
    }

    /// <summary>
    /// Shield.cs에서 호출하는 함수
    /// </summary>
    public void OnBounceByDefend()
    {
        rigid2D.AddForce(Vector2.down * bouncePower, ForceMode2D.Impulse);
    }

    private void OnDamaged()
    {
        GameScenePresenter.GetInstance.PlayerHPUpdate();
    }
    #region PlayerInputController 에서 호출하는 함수들
    public void Attack()
    {
        animator.SetTrigger(onAttack);
        animator.SetFloat(blendAttack, 0);
        SwordSlash().Forget();
    }
    private async UniTaskVoid SwordSlash()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0)
        {
            Debug.Log("2");
            particleObject.Play();
            await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());
        }
    }

    /// <summary>
    /// 공격 게이지가 100%가 되면 실행 가능한 특수 공격
    /// </summary>
    public void AttackSpecial()
    {
        animator.SetTrigger(onAttack);
        animator.SetFloat(blendAttack, 1);
        SwordSlashSpecial().Forget();
    }
    private async UniTaskVoid SwordSlashSpecial()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0)
        {
            Debug.Log("1");
            particleObject.Play();
            await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());
        }
    }
    public void Defense()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Defense"))
        {
            animator.SetTrigger(onDefense);
        }
    }

    public void Jump()
    {
        if (IsGround)
        {
            rigid2D.isKinematic = false;
            IsGround = false;
            rigid2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetTrigger(onJump);
        }
    }

    /// <summary>
    /// 점프 게이지가 100%가 되면 실행 가능한 특수 점프
    /// </summary>
    public void JumpSpecial()
    {
        rigid2D.isKinematic = false;
        rigid2D.AddForce(Vector2.up * jumpSpecialPower, ForceMode2D.Impulse);

        AttackSpecial();
    }
    #endregion
}
