using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDoor : MonoBehaviour
{

    [SerializeField] string sceneName; //장소 이동할 때 어느 씬으로 이동하게 할지 정해준다.
    [SerializeField] string locationName; //어디를 가는지 알려주는


    public string GetSceneName()
    {
        return sceneName;
    }

    public string GetLocationName()
    {
        return locationName;
    }

}
