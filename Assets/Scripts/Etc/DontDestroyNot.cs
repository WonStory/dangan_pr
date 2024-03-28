using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyNot : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
