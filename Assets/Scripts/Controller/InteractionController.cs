using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera cam;
    
    RaycastHit hitInfo;

    [SerializeField] GameObject go_NomalCrosshair;
    [SerializeField] GameObject go_interactiveCrosshair;
    bool isContact = false; //Contact함수의 길이가 길어지면 자꾸 검사하는게 쓸데없어짐. 따로 변수를 만들어서 관리


    // Update is called once per frame
    void Update()
    {
        CheckObject();
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


}
