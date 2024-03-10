using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] float fadeSpeed;//천천히 자연스럽게 스프라이트 변화르 주기위한 변수

    bool CheckSameSprite(SpriteRenderer p_SpriteRenderer, Sprite p_Sprite) //같은 스프라이트로의 변화면 필요가 없다.
    {
        if (p_SpriteRenderer.sprite == p_Sprite)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public IEnumerator SpriteChangeCoroutine(Transform p_Target, string p_SpriteName)
    {
        SpriteRenderer[] t_SpriteRenderer = p_Target.GetComponentsInChildren<SpriteRenderer>(); //자식개체거를 가지고옴
        Sprite t_Sprite = Resources.Load("Characters/" + p_SpriteName, typeof(Sprite)) as Sprite; //타입을 확인하고 가져온다음 형변환을 통해 넣어준다.

        if (!CheckSameSprite(t_SpriteRenderer[0], t_Sprite)) //같지 않을 때만 변경
        {
            Color t_color = t_SpriteRenderer[0].color;
            Color t_ShadowColor = t_SpriteRenderer[1].color;
            t_color.a = 0;
            t_ShadowColor.a =0;
            t_SpriteRenderer[0].color = t_color; //스프라이트에 넣어줘서 투명해짐
            t_SpriteRenderer[1].color = t_ShadowColor;

            t_SpriteRenderer[0].sprite = t_Sprite;
            t_SpriteRenderer[1].sprite = t_Sprite;

            while (t_color.a < 1) //점점사라지게끔 fadespeed만큼
            {
                t_color.a += fadeSpeed;
                t_ShadowColor.a += fadeSpeed;
                t_SpriteRenderer[0].color = t_color;
                t_SpriteRenderer[1].color = t_ShadowColor;

                yield return null;
            }
        }
    }








}
