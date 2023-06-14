using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerBehaviourController : MonoBehaviour
{
    private Rigidbody2D rigid2D;
    private Animator animator;

    private static readonly int onAttack = Animator.StringToHash("onAttack");
    private static readonly int onDefense = Animator.StringToHash("onDefense");
    private static readonly int onJump = Animator.StringToHash("onJump");
    private static readonly int blendAttack = Animator.StringToHash("IsAttack");
    private static readonly int onDefenseSp = Animator.StringToHash("onDefenseSp");

    [SerializeField] private PlayerData playerData;
    [SerializeField] private ParticleSystem slashParticle;
    [SerializeField] private ParticleSystem ghostTrailParticle;

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
        playerData.OnAfterDeserializeHP();

        jumpPower = playerData.jumpPower;
        jumpSpecialPower = playerData.jumpSpecialPower;
        bouncePower = playerData.bouncePower;

        IsGround = true;
        rigid2D.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Ground"))
        {
            IsGround = true;
        }

        if (collision.gameObject.CompareTag("Enemy") && !isDefend)
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
    /// Animation Clip에서 Event함수로 등록해서 실행한다.
    /// Character Animation의 Attack01, Attack02에 등록되어 사용중
    /// </summary>
    private void SlashParticleEvent()
    {
        if (!slashParticle.isPlaying)
        {
            slashParticle.Play();
        }
        else
        {
            slashParticle.Pause();
            slashParticle.Play();
        }
    }

    private void GhostParticleEvent()
    {
        if (!ghostTrailParticle.isPlaying)
        {
            ghostTrailParticle.Play();
        }
        else
        {
            ghostTrailParticle.Pause();
            ghostTrailParticle.Play();
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
        playerData.playerHpRuntime -= 1;
        GameScenePresenter.GetInstance.UpdatePlayerHP(playerData.playerHpRuntime);
    }
    #region PlayerInputController 에서 호출하는 함수들
    public void Attack()
    {
        animator.SetTrigger(onAttack);
        animator.SetFloat(blendAttack, 0);
    }

    /// <summary>
    /// 공격 게이지가 100%가 되면 실행 가능한 특수 공격
    /// </summary>
    public void AttackSpecial()
    {
        animator.SetTrigger(onAttack);
        animator.SetFloat(blendAttack, 1);
    }

    public void Defense()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Defense"))
        {
            animator.SetTrigger(onDefense);
        }
    }

    public void DefenseSpecial()
    {
        rigid2D.AddForce(Vector2.up * jumpSpecialPower, ForceMode2D.Impulse);
        animator.SetTrigger(onDefenseSp);
    }

    public void Jump()
    {
        if (IsGround)
        {
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
        rigid2D.AddForce(Vector2.up * jumpSpecialPower, ForceMode2D.Impulse);
        AttackSpecial();
    }
    #endregion
}
