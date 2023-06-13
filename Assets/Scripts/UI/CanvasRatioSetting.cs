using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasRatioSetting : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler;

    private float canvasAspectRatio = 9.0f / 16.0f; //Canvas에서 설정해둔 비율 -> 900:1600 을 표현
    private float screenAspectRatio = 0.0f; //현재 게임이 실행되는 창의 비율을 가져온다.

    private void Start()
    {
        if (canvasScaler == null)
        {
            canvasScaler = GetComponent<CanvasScaler>();
        }
        
        screenAspectRatio = (float)Screen.width / Screen.height;

        if (canvasAspectRatio < screenAspectRatio)
        {
            //현재 플레이중인 화면의 width가 더 넓은경우
            this.canvasScaler.matchWidthOrHeight = 1.0f; 
        }
        else
        {
            //현재 플레이중인 화면의 height 더 넓은경우
            this.canvasScaler.matchWidthOrHeight = 0.0f;
        }

    }
}
