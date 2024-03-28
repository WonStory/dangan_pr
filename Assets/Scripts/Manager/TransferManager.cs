using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //씬이동을 위해 필요한 매니저

public class TransferManager : MonoBehaviour
{
    string locationName;

    SplashManager theSplashManager;
    InteractionController theIC;

    void Start()
    {
        theSplashManager = FindObjectOfType<SplashManager>();
        theIC = FindObjectOfType<InteractionController>();
    }

    public IEnumerator Transfer(string p_SceneName, string p_LocationName)
    {
        theIC.SettingUI(false);
        SplashManager.isFinished = false;
        StartCoroutine(theSplashManager.FadeOut(false, true));
        yield return new WaitUntil(()=>SplashManager.isFinished); //트루가 될 때 까지 화면전환 대기시킨다


        locationName = p_LocationName;
        TransferSpawnManager.spawnTiming = true; //씬전환이 이루어졌으므로 트루로 바꿔준다.
        SceneManager.LoadScene(p_SceneName);
        
    }

    public IEnumerator Done()//스폰장소로 이동된 다음에 호출되어야된다.
    {
        SplashManager.isFinished = false;
        StartCoroutine(theSplashManager.FadeIn(false, true));
        yield return new WaitUntil(()=>SplashManager.isFinished); //트루가 될 때 까지 화면전환 대기시킨다

        theIC.SettingUI(true); //ui를 다시 등장시킨다.
    }

    public string GetLocationName()
    {
        return locationName;
    }
}