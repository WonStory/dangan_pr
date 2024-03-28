using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static bool onlyView = true; //방에서 시작하니까 트루로 시작하는데 이걸로 if문을 걸어서 카메라세팅을 조작

    Vector3 originPos; //카메라가 있던 위치, 각도
    Quaternion originRot;

    InteractionController theIC;
    PlayerController thePlayer;

    Coroutine coroutine;

    void Start()
    {
        theIC = FindObjectOfType<InteractionController>();
        thePlayer = FindObjectOfType<PlayerController>();
    }

    public void CamOriginSetting()
    {
        originPos = transform.position;
        if (onlyView)
        {
            originRot = Quaternion.Euler(0,0,0); //정면을 보도록, 씬전환할 때는 이게 악영향을 준다.(특히 이동 가능한 공간에선 더더욱)
        }
        else
        {
            originRot = transform.rotation; //우리가 위치 세팅을 해놨기 떄문에(거의 초기화 수준) 지금 가지고 있는 로테이션을 주면된다.
        }
    }

    public void CameraTargetting(Transform p_Target, float p_CamSpeed = 0.1f, bool p_isReset = false, bool p_isFinish = false)
    {
        
        if (!p_isReset) //프론트의 경우
        {
            if (p_Target != null)
            {
                StopAllCoroutines();//다시 재생될 때 코루틴이 꼬이지 않도록 정지시키고 실행한다.
                coroutine = StartCoroutine(CameraTargettingCoroutine(p_Target,p_CamSpeed));
            }
        }
        else
        {
            if (coroutine !=null) //지금 동작중이라는 것
            {
                StopCoroutine(coroutine);
            }
            StartCoroutine(CameraResetCoroutine(p_CamSpeed,p_isFinish));
        }
        
    }

    IEnumerator CameraTargettingCoroutine(Transform p_Target, float p_CamSpeed = 0.05f)
    {
        Vector3 t_TargetPos = p_Target.position;
        Vector3 t_TargetFrontPos = t_TargetPos + p_Target.forward;//타게팅된 대상의 정면 위치를 기억시켜준다. 대상과 정해진 거리만큼 떨어뜨려논다.(멀리보고싶으면 포워드에 곱해준다)
        Vector3 t_Direction = (t_TargetPos - t_TargetFrontPos).normalized; //상대방 정면에서 바라보려면 방향성이 있어야된다. 경우에 따라서 천차만별이므로 정규화 시켜준다.

        while (transform.position != t_TargetFrontPos || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(t_Direction)) >= 0.5f)//각도차이가 거의 없을 때 까지라는 or조건문도 추가 시킨다.
        {
            transform.position = Vector3.MoveTowards(transform.position, t_TargetFrontPos, p_CamSpeed); //자기 위치에서 상대방 위치까지 캠스피드로
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(t_Direction),p_CamSpeed); //대상을 바라보도록 회전시켜줌
            yield return null;
        }


    }

    IEnumerator CameraResetCoroutine(float p_CamSpeed = 0.05f, bool p_isFinish = false)
    {
        yield return new WaitForSeconds(0.5f); //잠시 대기시켰다가 초기화 하고싶음 =>리셋이 연속 두번되면 어색함
        while (transform.position != originPos || Quaternion.Angle(transform.rotation, originRot) >= 0.5f)//각도차이가 거의 없을 때 까지라는 or조건문도 추가 시킨다.
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, p_CamSpeed); //자기 위치에서 상대방 위치까지 캠스피드로
            transform.rotation = Quaternion.Lerp(transform.rotation, originRot,p_CamSpeed); //대상을 바라보도록 회전시켜줌 , 목적지만 바꿔준다
            yield return null;
        }
        transform.position = originPos; //자기위치로

        if (p_isFinish)
        {//모든 대화가 끝났으면 리셋
            thePlayer.Reset();
            //theIC.SettingUI(true);무조건 띄우면 안되므로 다른코드로 대체한다
            InteractionController.isInteract = false; //이러면 무한대기를 만족하게되므로 다음 이벤트로 진행된다. 그리고 원래 띄우던 셋팅UI는 다이어로그 매니저 else로 보낸다.
        }
    }
}
