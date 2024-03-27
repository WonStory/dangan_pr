using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //씬이동을 위해 필요한 매니저

public class TransferManager : MonoBehaviour
{
    string locationName;


    public IEnumerator Transfer(string p_SceneName, string p_LocationName)
    {
        yield return null;
        locationName = p_LocationName;
        TransferSpawnManager.spawnTiming = true; //씬전환이 이루어졌으므로 트루로 바꿔준다.
        SceneManager.LoadScene(p_SceneName);
    }

    public string GetLocationName()
    {
        return locationName;
    }
}
