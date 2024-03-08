using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogueEvent; //여기서 꺼내오게끔 트리거 역할을 해야된다.

    public Dialogue[] GetDialogue()
    {
        DialogueEvent t_DialogueEvent = new DialogueEvent(); //임시변수를 할당해서 치환해준다. null값으로 복붙하지 않게
        t_DialogueEvent.dialogues = Datamanager.instance.GetDialogue((int)dialogueEvent.line.x, (int)dialogueEvent.line.y); //플롯값을 인트로 강제 형변해줘야한다. (벡터값이므로)
        for (int i = 0; i < dialogueEvent.dialogues.Length; i++)
        {
            t_DialogueEvent.dialogues[i].tf_target = dialogueEvent.dialogues[i].tf_target;
        }
        
        dialogueEvent.dialogues = t_DialogueEvent.dialogues;
        
        return dialogueEvent.dialogues;
    
    
    }
}
