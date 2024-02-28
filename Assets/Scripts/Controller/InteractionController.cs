using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera cam;
    
    RaycastHit hitInfo;

    [SerializeField] GameObject go_NomalCrosshair;
    [SerializeField] GameObject go_interactiveCrosshair;
    [SerializeField] GameObject go_Crosshair; //크로스헤어 부모객체
    [SerializeField] GameObject go_Cursor; //커서, 대화할때 비활성화 하도록


    bool isContact = false; //Contact함수의 길이가 길어지면 자꾸 검사하는게 쓸데없어짐. 따로 변수를 만들어서 관리
    public static bool isInteract = false; //인터렉트하고 있는지 불값으로 나타낸다.

    [SerializeField] ParticleSystem ps_QuestionEffect;

    DialogueManager theDM;

    public void HideUI()
    {
        go_Crosshair.SetActive(false);
        go_Cursor.SetActive(false);
    }

    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>(); //다이아로그매니저가 있는 객체를 다 뒤져서 넣어줌
    }

    // Update is called once per frame
    void Update()
    {
        CheckObject();
        ClickLeftBtn();
    }

    void CheckObject()
    {
        Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,0);

        if (Physics.Raycast(cam.ScreenPointToRay(t_MousePos), out hitInfo, 100))//마우스 좌표를 환산해서 정면으로 치환시켜주는 투래이함수
        {
            Contact();
        }
        else
        {
            NotContact();
        }
    
    }

    void Contact()
    {
        if (hitInfo.transform.CompareTag("Interaction"))
        {
            if (!isContact) //일단 실행을 한 뒤에 트루가 되므로 계속해서 검사를 하지 않는다.(실행을 하지 않는다.)
            {
                isContact = true;
                go_interactiveCrosshair.SetActive(true);
                go_NomalCrosshair.SetActive(false);
            }
            
        }
        else
        {
            NotContact();
        }
    }

    void NotContact()
    {
        if (isContact)
        {
            isContact = false;
            go_interactiveCrosshair.SetActive(false);
            go_NomalCrosshair.SetActive(true);
        }
        
    }

    void ClickLeftBtn()
    {
        if (!isInteract) //상호작용 중이 아닐 때만 클릭이 가능하도록
        {
            if (Input.GetMouseButtonDown(0)) //마우스클릭을 나타내는 함수고, 0은 좌클릭을 나타낸다.
            {
                if (isContact)
                {
                    interact();
                }
            }
        }
    }

    void interact()
    {
        isInteract= true;

        ps_QuestionEffect.gameObject.SetActive(true);
        Vector3 t_targetPos = hitInfo.transform.position; //부딫힌 녀석의 인포를 가져오고
        ps_QuestionEffect.GetComponent<QuestionEfeect>().SetTarget(t_targetPos); //붙여둔 스크립트를 가져와서 퍼블릭으로 설정한 셋타겟에 타겟포스를 넘겨준다.
        ps_QuestionEffect.transform.position = cam.transform.position;//캠의 위치로 바꿔준다.(현재 위치에서 던지는 것 처럼 하기위해)
    
        StartCoroutine(WaitCollision());
    }

    IEnumerator WaitCollision()
    {
        yield return new WaitUntil(()=>QuestionEfeect.isCollide); //정해져있지 않고 부딪힐 때 까지 기다려야하므로 특정조건까지 기다리는 언틸을 쓴다.
        QuestionEfeect.isCollide =false; //다시 기본값으로 충돌하면 다시 트루가 댐
    
        theDM.ShowDialogue();
    }
}
