using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //UI크로스헤어를 움직여줘야하는데 그래서 위치를 기억해둬야댐.
    [SerializeField] Transform tf_Crosshair; //serializedfield를 쓰면 원래 private한 걸 inspector에도 띄워줘서 편하게 할 수 있다.(퍼블릭 효과를 줄 수 있다. 보호는 유지되면서)




    // Update is called once per frame
    void Update()
    {
        //마우스 움직임에 따라 업데이트 해줘야되므로 크로스헤어를 여기에 해준다.
        CrosshairMovig();
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
