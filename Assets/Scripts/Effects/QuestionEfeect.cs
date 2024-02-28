using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionEfeect : MonoBehaviour
{
    [SerializeField] float moveSpped;
    Vector3 targetPos = new Vector3();

    [SerializeField] ParticleSystem ps_Effect;

    public void SetTarget(Vector3 _target) //타겟의 위치설정
    {
        targetPos = _target; //목표물의 위치
    }


   
    // Update is called once per frame
    void Update()
    {
        if (targetPos != Vector3.zero)
        {if ((transform.position - targetPos).sqrMagnitude >= 0.1f) //최대한 가까워질 때 까지 가도록(0.1까지 lerf하도록)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpped);//시작점부터 목적지까지 무브스피드만큼쪼개서
        }
        else //충돌한거랑 다름없으므로 충돌이펙트를 켜주고, 충돌체 위치설정(현재 자기 위치로),
        //그리고 재생 시켜준다. 목적지까지 도달했으므로 목적지를 zero를 만들어주고 비활성화시킴 
        {
            ps_Effect.gameObject.SetActive(true);
            ps_Effect.transform.position = transform.position;
            ps_Effect.Play();
            targetPos = Vector3.zero;
            gameObject.SetActive(false);
            }

        }
    }
}
