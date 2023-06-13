using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private PlayerBehaviourController playerController;

    [Header("Attack Btn과 관련된 데이터들")]
    [SerializeField] private RectTransform attackBtnRectTrans;
    [SerializeField] private Image attackFilledImg;
    [SerializeField] private Image attackFullImg;

    [Header("Jump Btn과 관련된 데이터들")]
    [SerializeField] private RectTransform jumpBtnRectTrans;
    [SerializeField] private Image jumpFilledImg;
    [SerializeField] private Image jumpFullImg;

    [Header("Btn의 움직일 범위")]
    [SerializeField] private float joyBtnMoveRange = 220.0f; //y축 최대 310 - 원래 y축 90 = 220


    //Joy Stick과 Drag와 관련된 데이터들
    private RectTransform joyBtnMoveRectTrans;

    //공격과 점프 full gauage image의 color data들
    private Color attackFullImgColor;
    private Color jumpFullImgColor;

    //공격과 점프시 게이지 충전량 -> 1.0f / 무기별 충전량 -> 현재는 임시 값으로 세팅
    private float attackGauage = 0.0f;
    private float jumpGauage = 0.0f;

    //공격과 점프의 게이지를 다 채웠을 경우 true가 됨
    private bool isAttackSpecial = false;
    private bool isJumpSpeical  = false;

    private void Awake()
    {
        joyBtnMoveRectTrans = GetComponent<RectTransform>();
    }
    private void Start()
    {
        if (playerController == null)
        {
            playerController = GameObject.Find("Character").GetComponent<PlayerBehaviourController>();
        }
        attackFilledImg.fillAmount = 0.0f;
        jumpFilledImg.fillAmount = 0.0f;

        attackFullImgColor = attackFullImg.color;
        jumpFullImgColor = jumpFullImg.color;
        attackFullImgColor.a = 0.0f;
        jumpFullImgColor.a = 0.0f;
        attackFullImg.color = attackFullImgColor;
        jumpFullImg.color = jumpFullImgColor;

        attackGauage = 0.1f;
        jumpGauage = 0.3f;

    }

    #region Button - EventTrigger System Functions
    public void AttackButtonDown(BaseEventData data)
    {
        Debug.Log("Attack 실행중");
        attackFilledImg.fillAmount += attackGauage;
        if (1.0f <= attackFilledImg.fillAmount)
        {
            isAttackSpecial = true;
            attackFullImgColor.a = 0.5f;
            attackFullImg.color = attackFullImgColor;
        }
        playerController.Attack();
    }

    public void AttackButtonDrag(BaseEventData data)
    {
        PointerEventData eventData = (PointerEventData)data;
        if (isAttackSpecial)
        {
            Vector2 inputPos = eventData.position - joyBtnMoveRectTrans.anchoredPosition;
            inputPos.x = 0;
            Vector2 inputDir = inputPos.y < joyBtnMoveRange ? inputPos : inputPos.normalized * joyBtnMoveRange;
            attackBtnRectTrans.anchoredPosition = inputDir;
            if (inputDir.y == attackBtnRectTrans.anchoredPosition.y)
            {
                AttackGauageReset();
                playerController.AttackSpecial();
            }
        }
    }

    public void AttackButtonDragEnd(BaseEventData data)
    {
        attackBtnRectTrans.anchoredPosition = Vector2.zero;
    }
    private void AttackGauageReset()
    {
        attackFilledImg.fillAmount = 0.0f;
        attackFullImgColor.a = 0.0f;
        attackFullImg.color = attackFullImgColor;
        isAttackSpecial = false;
    }

    public void DefenseButtonDown(BaseEventData data)
    {
        Debug.Log("Defense 실행중");
        playerController.Defense();
    }

    public void JumpButtonDown(BaseEventData data)
    {
        Debug.Log("Jump 실행중");
        if (playerController.IsGround)
        {
            jumpFilledImg.fillAmount += jumpGauage;
            if (1.0f <= jumpFilledImg.fillAmount)
            {
                isJumpSpeical = true;
                jumpFullImgColor.a = 0.5f;
                jumpFullImg.color = jumpFullImgColor;
            }
            playerController.Jump();
        }
    }

    public void JumpButtonDrag(BaseEventData data)
    {
        PointerEventData eventData = (PointerEventData)data;
        if (isJumpSpeical)
        {
            Vector2 inputPos = eventData.position - joyBtnMoveRectTrans.anchoredPosition;
            inputPos.x = 0;
            Vector2 inputDir = inputPos.y < joyBtnMoveRange ? inputPos : inputPos.normalized * joyBtnMoveRange;
            jumpBtnRectTrans.anchoredPosition = inputDir;
            if (inputDir.y == jumpBtnRectTrans.anchoredPosition.y)
            {
                JumpGauageReset();
                playerController.JumpSpecial();
            }
        }
    }

    public void JumpButtonDragEnd(BaseEventData data)
    {
        jumpBtnRectTrans.anchoredPosition = Vector2.zero;
    }

    private void JumpGauageReset()
    {
        jumpFilledImg.fillAmount = 0.0f;
        jumpFullImgColor.a = 0.0f;
        jumpFullImg.color = jumpFullImgColor;
        isJumpSpeical = false;
    }
    #endregion
}
