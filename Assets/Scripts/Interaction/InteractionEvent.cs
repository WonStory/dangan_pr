using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogue; //여기서 꺼내오게끔 트리거 역할을 해야된다.

    public Dialogue[] GetDialogue()
    {
        dialogue.dialogues = Datamanager.instance.GetDialogue((int)dialogue.line.x, (int)dialogue.line.y); //플롯값을 인트로 강제 형변해줘야한다. (벡터값이므로)
        return dialogue.dialogues;
    
    
    }
}
