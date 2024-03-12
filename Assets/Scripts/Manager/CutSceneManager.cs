using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{

    public static bool isFinished = false; //컷씬을 불러오는 도중 텍스트가 출력되면 안되므로 불 변수로 척도를 나눔.

    SplashManager theSplashManager;
    CameraController theCam;

    [SerializeField] Image img_CutScene;



    // Start is called before the first frame update
    void Start()
    {
        theSplashManager = FindObjectOfType<SplashManager>();
        theCam = FindObjectOfType<CameraController>();
    }

    public bool CheckCutScene()
    {
        return img_CutScene.gameObject.activeSelf;
    }

    public IEnumerator CutSceneCoroutine(string p_CutSceneName, bool p_isShow)//다시 보여줄지 사라질지 불로 설정
    {
        SplashManager.isFinished = false;
        StartCoroutine(theSplashManager.FadeOut(true, false));
        yield return new WaitUntil(()=>SplashManager.isFinished);

        if (p_isShow) //이거로 판단
        {
            Sprite t_Sprite = Resources.Load<Sprite>("CutScenes/" + p_CutSceneName);
            if (t_Sprite != null) //실수로 잘못넣으면 안되므로 if문 써줌
            {
                img_CutScene.gameObject.SetActive(true);
                img_CutScene.sprite = t_Sprite;
                theCam.CameraTargetting(null, 0.1f, true, false); //카메라를 한번 초기화해준다음 다시 오브젝트프론트로 가게한다.
            }
            else
            {
                Debug.LogError("잘못된 컷신 CG파일 이름입니다.");
            }
        }
        else
        {
            img_CutScene.gameObject.SetActive(false);
        }
    
        SplashManager.isFinished = false;
        StartCoroutine(theSplashManager.FadeIn(true, false));
        yield return new WaitUntil(()=>SplashManager.isFinished);

        yield return new WaitForSeconds(0.5f); //살짝 멈칫했다 나오도록 자연스러움을 위해

        isFinished = true;
    }
}
