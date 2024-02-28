using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionType : MonoBehaviour
{
   
    public bool isDoor;//문인지 구별
    public bool isObject;//일반 사물, 사람인지 구별

    [SerializeField] string interactionName;//크로스헤어 갖다댄 객체 이름을 여기로 받아옴

    public string GetName()
    {
        return interactionName;
    }
    

}
