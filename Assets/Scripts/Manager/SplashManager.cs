using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{

    [SerializeField] Image image; //이미지로 페이드인 아웃을 조절하는 느낌으로

    [SerializeField] Color colorWhite;
    [SerializeField] Color colorBlack;

    [SerializeField] float fadeSpeed;
    [SerializeField] float fadeSlowSpeed; //느리게 시킬 때랑 빠르게 시킬때를 구분해서

    public static bool isFinished = false;

    public IEnumerator Splash() //순간의 번쩍임을 이 함수로 표현
    {
        StartCoroutine(FadeOut(true, false));
        yield return new WaitUntil(()=>isFinished);//완전 페이드 아웃 되고 실행되어야되므로 대기
        StartCoroutine(FadeIn(true, false));
    }

    public IEnumerator FadeOut(bool _isWhite, bool _isSlow) //화이트냐 아니냐, 느리게 페이드아웃할건지 아닌지
    {
        Color t_Color = (_isWhite == true) ? colorWhite : colorBlack;
        t_Color.a = 0;

        image.color = t_Color;

        while (t_Color.a < 1)
        {
            t_Color.a += (_isSlow == true) ? fadeSlowSpeed : fadeSpeed;
            image.color = t_Color;
            yield return null;
        }
        isFinished = true;
    }

    public IEnumerator FadeIn(bool _isWhite, bool _isSlow) //화이트냐 아니냐, 느리게 페이드아웃할건지 아닌지
    {
        Color t_Color = (_isWhite == true) ? colorWhite : colorBlack;
        t_Color.a = 1;

        image.color = t_Color;

        while (t_Color.a > 0)
        {
            t_Color.a -= (_isSlow == true) ? fadeSlowSpeed : fadeSpeed;
            image.color = t_Color;
            yield return null;
        }
        isFinished = true;
    }


}
