using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datamanager : MonoBehaviour
{
    public static Datamanager instance;

    [SerializeField] string CSV_FileName;

    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>(); //이벤트, 대화 라고 생각하면 편하다.

    public bool[] eventFlags = new bool[100]; //다양한 대화이벤트가 있을수록 많아짐 or 초기화시키면서 사용해도된다.

    public static bool isFinish = false; //전부 저장이 됐는지 아닌지 판단해주는 bool

    void Awake() //스타트보다 먼저 시작된다.
    {
        if (instance == null) //딱한번만 스타트하면 되므로 instance가 처음엔 널이다.
        {
            instance = this; //전부 등록된 데이터가 들어가게 된다.
            DialogueParser theParser = GetComponent<DialogueParser>();
            Dialogue[] dialogues = theParser.Parse(CSV_FileName);
            for (int i = 0; i < dialogues.Length; i++)
            {
                dialogueDic.Add(i+1,dialogues[i]);
            }
            isFinish = true;
        }
    }


    public Dialogue[] GetDialogue(int _StartNum, int _EndNum) //리턴타입을 배열로한다.(대화니까), 대사가 시작, 종료하는 시점
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); //배열의 끝을 모를 땐 일단 리스트

        for (int i = 0; i <= (_EndNum - _StartNum); i++) //_EndNum - _StartNum 몇번째부터 몇번째까지이므로
        {
            dialogueList.Add(dialogueDic[_StartNum + i]);
        }
        return dialogueList.ToArray();
    }
}
