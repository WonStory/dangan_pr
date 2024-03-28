using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDoor : MonoBehaviour
{
    [SerializeField] bool isOnlyView; //보통 문을 통과할 때(씬전환) 뷰타입이 바뀌므로 여기서 판단 bool을 만들어준다.
    [SerializeField] string sceneName; //장소 이동할 때 어느 씬으로 이동하게 할지 정해준다.
    [SerializeField] string locationName; //어디를 가는지 알려주는


    public string GetSceneName()
    {
        CameraController.onlyView = isOnlyView;
        return sceneName;
    }

    public string GetLocationName()
    {
        return locationName;
    }

}
