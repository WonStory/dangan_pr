using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] bool isAutoEvent = false; //이녀석이 트루면 자동으로 실행되는 이벤트라고 알아먹도록한다.


    [SerializeField] DialogueEvent dialogueEvent; //여기서 꺼내오게끔 트리거 역할을 해야된다.



    void Start()
    {
        bool t_Flag = CheckEvent();

        gameObject.SetActive(t_Flag);
    }

    bool CheckEvent()
    {
        bool t_Flag = true;

        //등장조건과 일치하지 않을 경우 등장시키지 않음.
        for (int i = 0; i < dialogueEvent.eventTiming.eventConditions.Length; i++) //조건을 배열로 만들었기 때문(컨디션 조건에 따라 달라진다)
        {
            if (Datamanager.instance.eventFlags[dialogueEvent.eventTiming.eventConditions[i]] != dialogueEvent.eventTiming.conditionFlag)
            {
                t_Flag = false; //등장조건이 일치하지 않으므로 false로 설정하여 등장하지 않음
                break;
            }
        }

        //등장 조건과 관계없이, 퇴장조건과 일치할 경우, 무조건 등장시키지 않음.
        if (Datamanager.instance.eventFlags[dialogueEvent.eventTiming.eventEndNum]) //특정이벤트를 봤다면 false
        {
            t_Flag = false;
        }
        return t_Flag;
    }

    public Dialogue[] GetDialogue()
    {
        Datamanager.instance.eventFlags[dialogueEvent.eventTiming.eventNum] = true;
        DialogueEvent t_DialogueEvent = new DialogueEvent(); //임시변수를 할당해서 치환해준다. null값으로 복붙하지 않게
        t_DialogueEvent.dialogues = Datamanager.instance.GetDialogue((int)dialogueEvent.line.x, (int)dialogueEvent.line.y); //플롯값을 인트로 강제 형변해줘야한다. (벡터값이므로)
        for (int i = 0; i < dialogueEvent.dialogues.Length; i++)
        {
            t_DialogueEvent.dialogues[i].tf_target = dialogueEvent.dialogues[i].tf_target;
            t_DialogueEvent.dialogues[i].cameraType = dialogueEvent.dialogues[i].cameraType;
        }
        
        dialogueEvent.dialogues = t_DialogueEvent.dialogues;
        
        return dialogueEvent.dialogues;
    
    
    }

    public AppearType GetAppearType() //인터렉션 이벤트창에서 넘겨줘야되므로 (위에는 다이아로그만 넘겨줌)
    {
        return dialogueEvent.appearType;
    }

    public GameObject[] GetTargets()
    {
        return dialogueEvent.go_Targets;
    }

    public GameObject GetNextEvent()
    {
        return dialogueEvent.go_NextEvent;
    }


    void Update()
    {
        if (isAutoEvent && Datamanager.isFinish && TransferManager.isFinished) //데이터 파싱이 다 끝나면 호출할 수 있도록 데이터매니저의 isfinished도 신경써준다. (오류방지용)
        {
            DialogueManager theDM = FindObjectOfType<DialogueManager>(); //대화매니저에 넘겨준다
            DialogueManager.isWaiting = true;
            
            if (GetAppearType() == AppearType.Appear) theDM.SetAppearObjects(GetTargets());
            else if (GetAppearType() == AppearType.Disappear) theDM.SetDisppearObjects(GetTargets()); //여기자체에 변수가 있기도하고 이러면 필요할 때마다 기능을 수행할 수 있다.
            theDM.SetNextEvent(GetNextEvent()); //dialogue매니저에 세팅이 된다.
            theDM.ShowDialogue(GetDialogue());
        
            gameObject.SetActive(false); //냅두면 계속반복할 것이므로 아예 삭제해버린다.
        }

    
    }







}
