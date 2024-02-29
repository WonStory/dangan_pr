using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); //대사 리스트 생성.
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName); //그냥 선언만 하면 널값이 되니까 리소스 폴더를 로드한다.
        
        string[] data = csvData.text.Split(new char[]{'\n'}); //1열 2열 3열 쪼갠다는 의미로 보면된다.
        
        for (int i = 1; i < data.Length; )
        {
            string[] row = data[i].Split(new char[]{','}); //아이디, 캐릭터, 대사가 배열로 들어갈 것이다.
            Dialogue dialogue = new Dialogue();

            dialogue.name = row[1];

            List<string> contextList = new List<string>();

            do
            {
                contextList.Add(row[2]); //이름 한 줄당 대사가 하나밖에 안들어감.
                if (++i <data.Length)
                {
                    row = data[i].Split(new char[]{','}); //다음줄을 쪼개는 의미, 쪼갠다음 와일을 타고, 대사에 넣을지 말지 반복이 된다.
                }
                else
                {
                    break; //i가 렝스보다 크면 빠져나와야된다.
                }
            }while (row[0].ToString() == "");
            
            dialogue.contexts = contextList.ToArray();

            dialogueList.Add(dialogue);//세트로 묶여서 다이어로그에 들어가게 된다.
            
        }
        return dialogueList.ToArray(); //배열타입으로 변환
    }

    
}
