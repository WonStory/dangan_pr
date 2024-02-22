using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //UI크로스헤어를 움직여줘야하는데 그래서 위치를 기억해둬야댐.
    [SerializeField] Transform tf_Crosshair; //serializedfield를 쓰면 원래 private한 걸 inspector에도 띄워줘서 편하게 할 수 있다.(퍼블릭 효과를 줄 수 있다. 보호는 유지되면서)

    [SerializeField] Transform tf_Cam;//카메라 정보도 받아와야된다.

    [SerializeField] float sightSensivitity;//고개의 움직임 속도.
    [SerializeField] float lookLimitX;
    [SerializeField] float lookLimitY;
    float currentAngleX;
    float currentAngleY;


    // Update is called once per frame
    void Update()
    {
        //마우스 움직임에 따라 업데이트 해줘야되므로 크로스헤어를 여기에 해준다.
        CrosshairMovig();
        ViewMoving();
    }

    void ViewMoving()
    {
        //고개를 돌리는 거 뿐만 아니라 마우스포인터에 따라서도 움직여야된다.
        if (tf_Crosshair.localPosition.x > (Screen.width / 2 - 100) || tf_Crosshair.localPosition.x < (Screen.width / 2 + 100) )//여유롭게(나가지 못하게 막기도 했으니까)
        {
            //카메라 로테이션 y를 이용해서 x를 움직임
            currentAngleY += (tf_Crosshair.localPosition.x > 0 ) ? sightSensivitity : -sightSensivitity; //삼항연산자로 하여금 조건문을 통해 어떤값을 더할지 하는건데, 마우스가 왼쪽이냐 오른쪽이냐를 판별해준다.
            tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);


        }
    }

    void CrosshairMovig()
    {
        tf_Crosshair.localPosition = new Vector2(Input.mousePosition.x - (Screen.width / 2), 
                                                 Input.mousePosition.y - (Screen.height / 2) - 30);
        //좌표값을 움직여줄라면 포지션을 활용해야되는데 절대적인 포지션말고 상대적인 로컬 포지션을 써야된다. 부모객체와의 거리
        //그리고 마우스 좌표를 그대로 넣어서 실제값과 차이가 나므로 보정해줘야한다.
        float t_cursorPosX = tf_Crosshair.localPosition.x;
        float t_cursorPosY = tf_Crosshair.localPosition.y;

        //범위를 제한을 해줘야한다.
        t_cursorPosX = Mathf.Clamp(t_cursorPosX, (-Screen.width / 2 + 50) , (Screen.width / 2 -50) );
        t_cursorPosY = Mathf.Clamp(t_cursorPosY, (-Screen.height / 2 + 50) , (Screen.height / 2 -50) );

        //변환된 변수를 다시 좌표로 치환해줘야 된다.
        tf_Crosshair.localPosition = new Vector2(t_cursorPosX,t_cursorPosY);
    }
}
