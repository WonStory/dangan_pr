using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    public void CameraTargetting(Transform p_Target, float p_CamSpeed = 0.05f)
    {
        if (p_Target != null)
        {
            StopAllCoroutines();//다시 재생될 때 코루틴이 꼬이지 않도록 정지시키고 실행한다.
            StartCoroutine(CameraTargettingCoroutine(p_Target,p_CamSpeed));
        }
        
    }

    IEnumerator CameraTargettingCoroutine(Transform p_Target, float p_CamSpeed = 0.05f)
    {
        Vector3 t_TargetPos = p_Target.position;
        Vector3 t_TargetFrontPos = t_TargetPos + p_Target.forward;//타게팅된 대상의 정면 위치를 기억시켜준다. 대상과 정해진 거리만큼 떨어뜨려논다.
        Vector3 t_Direction = (t_TargetPos - t_TargetFrontPos).normalized; //상대방 정면에서 바라보려면 방향성이 있어야된다. 경우에 따라서 천차만별이므로 정규화 시켜준다.

        while (transform.position != t_TargetFrontPos || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(t_Direction)) >= 0.5f)//각도차이가 거의 없을 때 까지라는 or조건문도 추가 시킨다.
        {
            transform.position = Vector3.MoveTowards(transform.position, t_TargetFrontPos, p_CamSpeed); //자기 위치에서 상대방 위치까지 캠스피드로
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(t_Direction),p_CamSpeed); //대상을 바라보도록 회전시켜줌
            yield return null;
        }


    }
}
