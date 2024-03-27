using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //씬이동을 위해 필요한 매니저

public class TransferManager : MonoBehaviour
{
    
    public IEnumerator Transfer(string p_SceneName, string p_LocationName)
    {
        yield return null;

        SceneManager.LoadScene(p_SceneName);
    }
}
