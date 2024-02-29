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

    InteractionController theIC;

    void Start()
    {
        theIC = FindObjectOfType<InteractionController>();
    }

    public void ShowDialogue(Dialogue[] p_dialogues) //다이어로그 내용들을 받아준다.
    {
        txt_Dialogue.text = ""; //대화랑 이름 초기화
        txt_Name.text = "";
        theIC.HideUI(); //크로스헤어와 화살표 숨기기
        dialogues = p_dialogues;

        SettingUI(true);
    }

    void SettingUI(bool p_flag)
    {
        go_DialogueBar.SetActive(p_flag);
        go_DialogueNameBar.SetActive(p_flag);
    }
}
