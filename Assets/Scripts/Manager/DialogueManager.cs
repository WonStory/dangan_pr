using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public static bool isWaiting = false; //너무 갑자기 시작하지 않도록 잠시 기다리도록한다.


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

    //이벤트 끝나면 등장시키거나 퇴장시키는 오브젝트들.

    GameObject[] go_Objects; //등장(퇴장)시킬 사물 혹은 캐릭터 배열
    byte appearTypeNumber; //0일 경우 변화x, 1일경우 등장, 2일 경우 퇴장
    const byte NONE = 0, APPEAR =1, DISAPPEAR = 2; //숫자대신 변수명을 가지고 있는 상수를 이용할 것이다.

    public void SetAppearObjects(GameObject[] p_Targets)
    {
        go_Objects = p_Targets;
        appearTypeNumber = APPEAR;
    }

    public void SetDisppearObjects(GameObject[] p_Targets)
    {
        go_Objects = p_Targets;
        appearTypeNumber = DISAPPEAR;
    }



    InteractionController theIC;
    CameraController theCam;
    SplashManager theSplashManager;
    SpriteManager theSpriteManager;
    CutSceneManager theCutSceneManager;
    SlideManager theSlideManager;

    void Start()
    {
        theIC = FindObjectOfType<InteractionController>();
        theCam = FindObjectOfType<CameraController>();
        theSpriteManager = FindObjectOfType<SpriteManager>();
        theSplashManager = FindObjectOfType<SplashManager>();
        theCutSceneManager = FindObjectOfType<CutSceneManager>();
        theSlideManager = FindObjectOfType<SlideManager>();
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
                            StartCoroutine(CameraTargettingType());
                        }
                        else{ //여기선 이제 더이상의 대화가 없으므로 대화를 끝내줘야된다.
                            StartCoroutine(EndDialogue());
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
        
        StartCoroutine(StartDialogue());
    }

    IEnumerator StartDialogue()
    {
        if (isWaiting) yield return new WaitForSeconds(0.5f);
        isWaiting = false; //한번호출 됐으니까 false로 바꿔준다.
    
        theCam.CamOriginSetting(); //어디로 돌아갈지 저장해둠
        //theCam.CameraTargetting(dialogues[lineCount].tf_target);시작할 때도 설정을 해준다.
        StartCoroutine(CameraTargettingType()); //쇼다이어로그에서 여기로 옮겨준다.
    }

    IEnumerator CameraTargettingType()
    {
        switch(dialogues[lineCount].cameraType)
        {
            case CameraType.FadeIn : SettingUI(false); SplashManager.isFinished = false; StartCoroutine(theSplashManager.FadeIn(false,true)); yield return new WaitUntil(()=>SplashManager.isFinished); break; //검은색화면이라 폴스에 슬로우가 아니므로 트루로
            case CameraType.FadeOut : SettingUI(false); SplashManager.isFinished = false; StartCoroutine(theSplashManager.FadeOut(false,true)); yield return new WaitUntil(()=>SplashManager.isFinished); break;
            case CameraType.FlashIn : SettingUI(false); SplashManager.isFinished = false; StartCoroutine(theSplashManager.FadeIn(true,true)); yield return new WaitUntil(()=>SplashManager.isFinished); break;
            case CameraType.FlashOut : SettingUI(false); SplashManager.isFinished = false; StartCoroutine(theSplashManager.FadeOut(true,true)); yield return new WaitUntil(()=>SplashManager.isFinished); break;
            
            case CameraType.ObjectFront : theCam.CameraTargetting(dialogues[lineCount].tf_target); break;
            case CameraType.Reset : theCam.CameraTargetting(null, 0.05f, true, false); break; //타겟값은 필요없음 null, 조금 리셋은 트루, 이즈피니쉬는 폴스(엔드에서 줘야댐)
        
            case CameraType.ShowCutScene :  SettingUI(false); CutSceneManager.isFinished =false; StartCoroutine(theCutSceneManager.CutSceneCoroutine(dialogues[lineCount].spriteName[contextCount], true)); //보여주는 것이므로 트루이고 화자의 스프라이트 네임 위치에 컷씬도 넣었으므로 중복이다.
                                            yield return new WaitUntil(()=>CutSceneManager.isFinished);
                                            break;
            case CameraType.HideCutScene :  SettingUI(false); CutSceneManager.isFinished =false; StartCoroutine(theCutSceneManager.CutSceneCoroutine(null, false));//컷씬 안보여주는 것이므로
                                            yield return new WaitUntil(()=>CutSceneManager.isFinished);
                                            theCam.CameraTargetting(dialogues[lineCount].tf_target); //다시 타게팅하게끔
                                            break;

            case CameraType.AppearSlideCG : SlideManager.isFinished = false; StartCoroutine(theSlideManager.AppearSlide(SplitSlideCGName())); yield return new WaitUntil(()=>SlideManager.isFinished); theCam.CameraTargetting(dialogues[lineCount].tf_target); break;
            case CameraType.DisappearSlideCG : SlideManager.isFinished = false; StartCoroutine(theSlideManager.DisappearSlide()); yield return new WaitUntil(()=>SlideManager.isFinished); theCam.CameraTargetting(dialogues[lineCount].tf_target); break;
            case CameraType.ChangeSlideCG : SlideManager.isChanged = false; StartCoroutine(theSlideManager.ChangeSlide(SplitSlideCGName())); yield return new WaitUntil(()=>SlideManager.isChanged); theCam.CameraTargetting(dialogues[lineCount].tf_target); break;
        }
        StartCoroutine(TypeWriter());//라셋하든 타게팅하든 출력해야되므로
    }

    string SplitSlideCGName()
    {
        string t_Text = dialogues[lineCount].spriteName[contextCount];
        string[] t_Array = t_Text.Split(new char[]{'/'});
        if (t_Array.Length <= 1)
        {
            return t_Array[0];
        }
        else
        {
            return t_Array[1];
        }
    }

    IEnumerator EndDialogue()
    {
        SettingUI(false);
        if (theCutSceneManager.CheckCutScene())
        {
            CutSceneManager.isFinished =false; StartCoroutine(theCutSceneManager.CutSceneCoroutine(null, false));//컷씬 안보여주는 것이므로
            yield return new WaitUntil(()=>CutSceneManager.isFinished);
        }

        AppearOrDisappearObjects();

        yield return new WaitUntil(()=>Spin_Character.isFinished); //완전히 사라지고 나서 뜨도록, 하지만 이게 트루로 바뀔 때까지 무한 대기여서 아래 코드가 동작을 안한다. (스핀캐릭터, 스플래쉬매니저, 슬라이드 매니저를 바꿔준다)

        isDialogue =false;
        contextCount = 0;
        lineCount = 0;
        dialogues =null; //대화를 간직하고 있을 필요가 없음
        isNext = false;
        theCam.CameraTargetting(null, 0.05f, true, true);
    }

    void AppearOrDisappearObjects()
    {
        if (go_Objects != null)
        {
            Spin_Character.isFinished = false;
            for (int i = 0; i < go_Objects.Length; i++)
            {
                if (appearTypeNumber == APPEAR)
                {
                    go_Objects[i].SetActive(true);
                    StartCoroutine(go_Objects[i].GetComponent<Spin_Character>().SetAppearOrDisappear(true));
                
                }
                else if (appearTypeNumber == DISAPPEAR) 
                {
                    //go_Objects[i].SetActive(false); 비활성화 자체는 아래 코루틴에도 들어가 있으므로 제거해도 상관없음
                    StartCoroutine(go_Objects[i].GetComponent<Spin_Character>().SetAppearOrDisappear(false));
                }
            }
        }
        go_Objects = null;
        appearTypeNumber = NONE;
    }

    void ChangeSprite()
    {
        if (dialogues[lineCount].tf_target != null) //중복해서 넣었으므로 조건문을 걸어야 구분해서 바꿈. 원래는 빈칸만 아니면 호출이라서
        {
            if (dialogues[lineCount].spriteName[contextCount] != "") //공백일 땐 제외하고
            {
                StartCoroutine(theSpriteManager.SpriteChangeCoroutine(dialogues[lineCount].tf_target,
                                                                      dialogues[lineCount].spriteName[contextCount].Split(new char[]{'/'})[0]));
            }//tf_타켓에 꼭 넣어줘야 그 인물의 스프라이트가 바뀌는 것이므로 none으로 냅두면 안된다.
    
        }
    }

    void PlaySound()
    {
        if (dialogues[lineCount].VocieName[contextCount] != "")
        {
            SoundManager.instance.PlaySound(dialogues[lineCount].VocieName[contextCount], 2);
        }
    }

    IEnumerator TypeWriter() //텍스트 출력 코루틴
    {
        SettingUI(true); //텍스트가 출력될 때 떠야되므로 여기에 넣어줌
        ChangeSprite();
        PlaySound();

        string t_ReplaceText = dialogues[lineCount].contexts[contextCount]; //한줄이 여기에 들어가게된다.
        t_ReplaceText = t_ReplaceText.Replace("`",","); //컴마를 인식 못하는걸 여기서 치환해준다.
        t_ReplaceText = t_ReplaceText.Replace("\\n","\n"); //텍스트로 인식하기 위해선 슬래쉬를 한번더 해준다.


        bool t_white = false, t_yellow = false, t_cyan = false;
        bool t_ignore = false; //특수문자를 만나면 생략시키기 위한 변수


        for (int i = 0; i < t_ReplaceText.Length; i++)
        {
            switch(t_ReplaceText[i]) //특수문자인지 아닌지 체크하는 함수
            {
                case 'ⓦ' : t_white = true; t_yellow = false; t_cyan =false; t_ignore =true; break; //특수문자는 이그노어 하도록함.
                case 'ⓨ' : t_white = false; t_yellow = true; t_cyan =false; t_ignore =true; break;
                case 'ⓒ' : t_white = false; t_yellow = false; t_cyan =true; t_ignore =true; break;
                case '①' : StartCoroutine(theSplashManager.Splash()); SoundManager.instance.PlaySound("emotion0", 1); t_ignore = true; break;
                case '②' : StartCoroutine(theSplashManager.Splash()); SoundManager.instance.PlaySound("emotion1", 1); t_ignore = true; break;
            }

            string t_letter = t_ReplaceText[i].ToString(); //이걸 이용해서 색변화를 한다.

            if (!t_ignore)
            {
                if (t_white)
                {
                    t_letter = "<color=#ffffff>" + t_letter + "</color>";//폰트색 반영 검은색은 000000
                }
                else if (t_yellow)
                {
                    t_letter = "<color=#ffff00>" + t_letter + "</color>";
                }
                else if (t_cyan)
                {
                    t_letter = "<color=#42dee3>" + t_letter + "</color>";
                }
                txt_Dialogue.text += t_letter;//한글자씩 추가시켜준다. 그리고 대기시킴
            }
            t_ignore =false;
            yield return new WaitForSeconds(textDelay);
        }

        isNext = true;
        
    }

    void SettingUI(bool p_flag)
    {
        go_DialogueBar.SetActive(p_flag);

        if (p_flag)
        {
            if (dialogues[lineCount].name == "")
            {
                go_DialogueNameBar.SetActive(false);
            }
            else
            {
                go_DialogueNameBar.SetActive(true);
                txt_Name.text = dialogues[lineCount].name; //이름을 넣어줌
            }
        }
        else
        {
            go_DialogueNameBar.SetActive(false);
        }

    }
}
