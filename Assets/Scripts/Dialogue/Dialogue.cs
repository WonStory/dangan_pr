using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraType //열거형 자료라는 뜻 선택지처럼 고를 수 있게 해준다.
{
    ObjectFront,
    Reset,
    FadeOut,
    FadeIn,
    FlashOut,
    FlashIn,
    ShowCutScene,
    HideCutScene,
    AppearSlideCG,
    DisappearSlideCG,
    ChangeSlideCG,

}




[System.Serializable] //커스텀 클래스는 inspector창에서 수정이 안되기에 직접 설정(앞의 함수)을 해줘야된다.

public class Dialogue //모노비헤이비어를 상속 안받아서 스타트 업뎃 다 쓸모없음
{
    [Header("카메라가 타게팅 하는 대상")]
    public CameraType cameraType;
    public Transform tf_target;

    //[Tooltip("대사 치는 캐릭터 이름")]
    [HideInInspector]
    public string name; //이름은 받아야댐. 어떤 캐릭이 대사하는지

    //[Tooltip("대사 내용")] =>어차피 내가 채우는 변수가 아니라 카메라만 세팅하게끔 한다.
    [HideInInspector] 
    public string[] contexts;

    [HideInInspector]
    public string[] spriteName;

    [HideInInspector]
    public string[] VocieName;
}

[System.Serializable]

public class DialogueEvent
{
    public string name; //어느 이벤트인지 우리가 알아차리기 편하게 만드는 변수

    public Vector2 line; //x,y까지의 대사를 추출해서 빼올 수 있게 해준다.
    public Dialogue[] dialogues; //한명이 말하는게 아니기 때문에 배열로 만들어줘야한다.
}
