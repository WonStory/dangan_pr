using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideManager : MonoBehaviour
{
    [SerializeField] Image img_SlideCG;
    [SerializeField] Animation anim; //애니메이션 컴포넌트를 넣었으므로 그걸 컨트롤할 변수도 넣어준다.

    public static bool isFinished = true; //전부 등장하고 나서 텍스트를 출력할 수 있도록 해준다.
    public static bool isChanged = false; //체인지슬라이드 함수가 전체적으로 끝났는지 알려주는 불

    public IEnumerator AppearSlide(string p_SlideName)
    {
        Sprite t_Sprite = Resources.Load<Sprite>("Slide_Image/" + p_SlideName);
        if (t_Sprite != null)
        {
            img_SlideCG.gameObject.SetActive(true);
            img_SlideCG.sprite = t_Sprite;
            anim.Play("Appear");
        }
        else
        {
            Debug.LogError(p_SlideName + "에 해당하는 이미지 파일이 없습니다.");
        }

        yield return new WaitForSeconds(0.5f); //슬라이드 cg가 전부 등장할 때까지 기다리기

        isFinished = true;
        
    }

    public IEnumerator DisappearSlide()//얘는 파라미터를 받을 필요가 없음
    {
        anim.Play("Disappear");
        yield return new WaitForSeconds(0.5f); //슬라이드 cg가 전부 사라질 때까지 기다리기
        img_SlideCG.gameObject.SetActive(false);
        isFinished = true;
    }

    public IEnumerator ChangeSlide(string p_SlideName)
    {
        isFinished = false;
        StartCoroutine(DisappearSlide());
        yield return new WaitUntil(()=>isFinished);

        isFinished = false;
        StartCoroutine(AppearSlide(p_SlideName));
        yield return new WaitUntil(()=>isFinished);

        isChanged = true;
    }

}
