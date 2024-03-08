using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject go_DialogueBar;
    [SerializeField] GameObject go_DialogueNameBar;

    [SerializeField] TextMeshProUGUI txt_Dialogue;
    [SerializeField] TextMeshProUGUI txt_Name;

    Dialogue[] dialogues;

    bool isDialogue = false; //대화중인지 아닌지 판단
    bool isNext = false; //특정키 입력 대기중, 트루가되면 입력이 가능한 상태

    [Header("텍스트 출력 딜레이")]
    [SerializeField] float textDelay;

    int lineCount = 0; //대화 카운트
    int contextCount = 0;//대사 카운트, 여러 대사를 할 수 있음으로

    InteractionController theIC;

    void Start()
    {
        theIC = FindObjectOfType<InteractionController>();
    }

    void Update() //매프레임 키가 입력되었는지 판별을 해줘야된다.
    {
        if (isDialogue)
        {
            if (isNext)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isNext = false;
                    txt_Dialogue.text = ""; //여백으로 초기화 해준다음 다음게 입력 가능하다고 알려준다.
                    if (++contextCount < dialogues[lineCount].contexts.Length)
                    {
                        StartCoroutine(TypeWriter());
                    }
                    else
                    {
                        contextCount = 0;
                        if (++lineCount < dialogues.Length)
                        {
                            StartCoroutine(TypeWriter());
                        }
                        else{ //여기선 이제 더이상의 대화가 없으므로 대화를 끝내줘야된다.
                            EndDialogue();
                        }
                    }

                    
                }
            }
        }
    }

    public void ShowDialogue(Dialogue[] p_dialogues) //다이어로그 내용들을 받아준다.
    {
        isDialogue =true; //조건문을 걸었기 때문에 트루로 설정해준다.
        txt_Dialogue.text = ""; //대화랑 이름 초기화
        txt_Name.text = "";
        theIC.SettingUI(false); //크로스헤어와 화살표 숨기기
        dialogues = p_dialogues;

        StartCoroutine(TypeWriter());
    }

    void EndDialogue()
    {
        isDialogue =false;
        contextCount = 0;
        lineCount = 0;
        dialogues =null; //대화를 간직하고 있을 필요가 없음
        isNext = false;
        SettingUI(false);
        theIC.SettingUI(true);
    }

    IEnumerator TypeWriter() //텍스트 출력 코루틴
    {
        SettingUI(true); //텍스트가 출력될 때 떠야되므로 여기에 넣어줌
        string t_ReplaceText = dialogues[lineCount].contexts[contextCount]; //한줄이 여기에 들어가게된다.
        t_ReplaceText = t_ReplaceText.Replace("`",","); //컴마를 인식 못하는걸 여기서 치환해준다.

        txt_Name.text = dialogues[lineCount].name; //이름을 넣어줌
        for (int i = 0; i < t_ReplaceText.Length; i++)
        {
            txt_Dialogue.text += t_ReplaceText[i];//한글자씩 추가시켜준다. 그리고 대기시킴
            yield return new WaitForSeconds(textDelay);
        }

        isNext = true;
        yield return null;
    }

    void SettingUI(bool p_flag)
    {
        go_DialogueBar.SetActive(p_flag);
        go_DialogueNameBar.SetActive(p_flag);
    }
}
