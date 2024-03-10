using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spin_Character : MonoBehaviour
{
    [SerializeField] Transform tf_Target;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tf_Target != null)
        {
            Quaternion t_Rotation = Quaternion.LookRotation(tf_Target.position); //대상을 바라보도록
            Vector3 t_Euler = new Vector3(0, t_Rotation.eulerAngles.y, 0); //y축으로 회전해야 자연스럽다. x,z축은 고정
            transform.eulerAngles = t_Euler;
        }
    }
}
