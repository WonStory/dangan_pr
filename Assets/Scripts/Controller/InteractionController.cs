using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera cam;
    
    RaycastHit hitInfo;

    [SerializeField] GameObject go_NomalCrosshair;
    [SerializeField] GameObject go_interactiveCrosshair;
    [SerializeField] GameObject go_Crosshair; //크로스헤어 부모객체
    [SerializeField] GameObject go_Cursor; //커서, 대화할때 비활성화 하도록
    [SerializeField] GameObject go_FieldCursor;//일반 커서랑 필드커서를 구분

    [SerializeField] GameObject go_TargetNameBar;
    [SerializeField] TextMeshProUGUI txt_TargetName;

    bool isContact = false; //Contact함수의 길이가 길어지면 자꾸 검사하는게 쓸데없어짐. 따로 변수를 만들어서 관리
    public static bool isInteract = false; //인터렉트하고 있는지 불값으로 나타낸다.
    bool isTest = true;

    [SerializeField] ParticleSystem ps_QuestionEffect;

    [SerializeField] Image img_Interaction; //마름모꼴이 생겼다가 서서히 사라지도록
    [SerializeField] Image img_InteractionEffect;

    DialogueManager theDM;
/*
    public void HideUI()
    {
        go_Crosshair.SetActive(false);
        go_Cursor.SetActive(false);
        go_TargetNameBar.SetActive(false);

    }
*/ // 아래로 병합한 다음 지워준다.
    public void SettingUI(bool p_flag)
    {
        isTest = p_flag;
        isContact = !p_flag;
        go_Crosshair.SetActive(p_flag);
        
        if (!p_flag) //강제로 사라지게 만든다. 마우스를 올려놓고 하면 자국이 안사라진채로 스타트한다.
        {
            StopCoroutine("Interaction"); 
            Color color = img_Interaction.color;
            color.a =0;
            img_Interaction.color = color;
            go_TargetNameBar.SetActive(false); //타겟 네임바를 무조건 등장시키면 안되고 사라질 땐 알아서 사라지도록 한다.
            go_Cursor.SetActive(p_flag);
            go_FieldCursor.SetActive(p_flag);
        }
        else
        {
            if (CameraController.onlyView)
            {
                go_Cursor.SetActive(true);
            }
            else
            {
                go_FieldCursor.SetActive(true);
            }
            go_NomalCrosshair.SetActive(true);
            go_interactiveCrosshair.SetActive(false);
        }

        isInteract = !p_flag; //반대로 설정해두면 된다.
    }

    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>(); //다이아로그매니저가 있는 객체를 다 뒤져서 넣어줌
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInteract) //이걸로 계속 체크하지 않도록함
        {
            CheckObject();
            ClickLeftBtn();
        }
    }

    void CheckObject()
    {
        if (CameraController.onlyView)
        {
            Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,0);

            if (Physics.Raycast(cam.ScreenPointToRay(t_MousePos), out hitInfo, 100))//마우스 좌표를 환산해서 정면으로 치환시켜주는 투래이함수
            {
                if (isTest)
                {
                    Contact();
                }
            }
            else
            {
                NotContact();
            }
        }
        else
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, 10))//카메라 위치에서 카메라 정면으로 레이져를 쏘도록
            {
                if (isTest)
                {
                    Contact();
                }
            }
        }


    
    }

    void Contact() //객체가 닿은 부분
    {
        if (hitInfo.transform.CompareTag("Interaction"))
        {
            go_TargetNameBar.SetActive(true);
            txt_TargetName.text = hitInfo.transform.GetComponent<InteractionType>().GetName();
            if (!isContact) //일단 실행을 한 뒤에 트루가 되므로 계속해서 검사를 하지 않는다.(실행을 하지 않는다.)
            {
                isContact = true;
                go_interactiveCrosshair.SetActive(true);
                go_NomalCrosshair.SetActive(false);
                if (!CameraController.onlyView)
                {
                    StopCoroutine("Interaction");
                    StopCoroutine("InteractionEffect");
                    StartCoroutine("Interaction",true); //빠르게 반복하면 코루틴이 반복실행된다. 자연스럽게 생기고 낫컨택트일땐 자연스럽게 사라지고
                    StartCoroutine("InteractionEffect");
                }
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
            go_TargetNameBar.SetActive(false);
            isContact = false;
            go_interactiveCrosshair.SetActive(false);
            go_NomalCrosshair.SetActive(true);
            if (!CameraController.onlyView)
            {
                StopCoroutine("Interaction");
                StartCoroutine("Interaction",false);
            }
        }
        
    }

    IEnumerator Interaction(bool p_Appear)
    {
        Color color = img_Interaction.color; //자체가 가지고 있는 컬러를 넣어준다.
        if (p_Appear)
        {
            color.a = 0; //투명한 상태로 초기화
            while (color.a <1) //1이 완전히 불투명
            {
                color.a +=0.1f;
                img_Interaction.color = color;
                yield return null; //한프레임 대기시켜주는거임
            }
        }
        else
        {
            while (color.a > 0)
            {
                color.a -= 0.01f;
                img_Interaction.color = color;
                yield return null; //한프레임 대기시켜주는거임
            }
        }
    }

    IEnumerator InteractionEffect()
    {
        while (isContact && !isInteract)//상호작용이 가능한데 좌클릭을 안한 상태, 다시 초기화하고 반복
        {
            Color color = img_InteractionEffect.color;
            color.a = 0.5f;

            img_InteractionEffect.transform.localScale = new Vector3(1.3f,1.3f,1.3f);
            Vector3 t_scale = img_InteractionEffect.transform.localScale;

            while (color.a > 0)
            {
                color.a -= 0.005f;
                img_InteractionEffect.color = color;
                t_scale.Set(t_scale.x + Time.deltaTime, t_scale.y + Time.deltaTime, t_scale.z + Time.deltaTime);
                img_InteractionEffect.transform.localScale = t_scale;
                yield return null;
            }
            yield return null; //한프레임 대기
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

        StopCoroutine("Interaction"); //상호작용중일 때 코루틴을 우선 정지시키고(이펙트는 while문으로 이미 없어짐), 투명도를 바꾼다.
        Color color = img_Interaction.color;
        color.a =0;
        img_Interaction.color = color;

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
    
        yield return new WaitForSeconds(0.5f);

        InteractionEvent t_Event = hitInfo.transform.GetComponent<InteractionEvent>(); //자주 반복되면 더러워지므로



        if (hitInfo.transform.GetComponent<InteractionType>().isObject)
        {
            DialogueCall(t_Event);
        }
        else//캐릭터나 사물이 아닌 도어다 하면 else에 걸리도록
        {
            TransferCall();
        }
        
    }

    void TransferCall()
    {
        string t_SceneName = hitInfo.transform.GetComponent<InteractionDoor>().GetSceneName();
        string t_LocationName = hitInfo.transform.GetComponent<InteractionDoor>().GetLocationName();
        StartCoroutine(FindObjectOfType<TransferManager>().Transfer(t_SceneName, t_LocationName));
    }

    void DialogueCall(InteractionEvent p_Event)
    {
        theDM.SetNextEvent(p_Event.GetNextEvent());
        if (p_Event.GetAppearType() == AppearType.Appear) theDM.SetAppearObjects(p_Event.GetTargets());
        else if (p_Event.GetAppearType() == AppearType.Disappear) theDM.SetDisppearObjects(p_Event.GetTargets());
        theDM.ShowDialogue(p_Event.GetDialogue());
    }

}
