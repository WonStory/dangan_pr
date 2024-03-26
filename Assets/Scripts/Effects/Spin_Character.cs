using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spin_Character : MonoBehaviour
{
    [SerializeField] Transform tf_Target;

    bool spin = false; //트루일 경우 계속 돌도록
    public static bool isFinished = true;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tf_Target != null)
        {
            if (!spin)
            {
                Quaternion t_Rotation = Quaternion.LookRotation(tf_Target.position); //대상을 바라보도록
                Vector3 t_Euler = new Vector3(0, t_Rotation.eulerAngles.y, 0); //y축으로 회전해야 자연스럽다. x,z축은 고정
                transform.eulerAngles = t_Euler;
            }
            else
            {
                transform.Rotate(0, 90 * Time.deltaTime * 4, 0);
            }
            
        }
    }

    public IEnumerator SetAppearOrDisappear(bool p_Flag)
    {
        spin = true; //호출되었을때 빙글빙글돌도록 시작함.

        SpriteRenderer[] t_SpriteRenderer = GetComponentsInChildren<SpriteRenderer>();

        Color t_FrontColor = t_SpriteRenderer[0].color;
        Color t_RearColor = t_SpriteRenderer[1].color;

        if (p_Flag)
        {
            t_FrontColor.a = 0; t_RearColor.a = 0;
            t_SpriteRenderer[0].color = t_FrontColor; t_SpriteRenderer[1].color = t_RearColor;
        }

        float t_FadeSpeed = (p_Flag == true) ? 0.001f : -0.001f; //사라지거나 나타나거나

        yield return new WaitForSeconds(0.3f); //연출상의 문제로 잠시 대기시켜 준다.

        while (true)
        {
            if (p_Flag && t_FrontColor.a >= 1) break;
            else if (!p_Flag && t_FrontColor.a <= 0) break;//빠져나갈 땐 완성 등장했거나 완전 사라졌을 때 빠져나가도록.

            t_FrontColor.a += t_FadeSpeed; t_RearColor.a += t_FadeSpeed;
            t_SpriteRenderer[0].color = t_FrontColor; t_SpriteRenderer[1].color = t_RearColor;
            yield return null;

        }

        spin = false;
        isFinished = true;
        gameObject.SetActive(p_Flag); //스핀이 멈추면 해당 객체가 사라진다.
    }


}
