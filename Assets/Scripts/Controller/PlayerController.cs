using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    void Awake() //시작과 동시에 , 싱글톤
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] float walkSpped;
    [SerializeField] float runSpeed;
    float applySpeed;

    [SerializeField] float fieldSensitivity; //고개의 움직임(민감도)를 결정하기 위해
    [SerializeField] float fieldLookLimitX;

    //UI크로스헤어를 움직여줘야하는데 그래서 위치를 기억해둬야댐.
    [SerializeField] Transform tf_Crosshair; //serializedfield를 쓰면 원래 private한 걸 inspector에도 띄워줘서 편하게 할 수 있다.(퍼블릭 효과를 줄 수 있다. 보호는 유지되면서)

    [SerializeField] Transform tf_Cam;//카메라 정보도 받아와야된다.
    [SerializeField] Vector2 camBoundary; //캠의 가두기 영역
    [SerializeField] float sightMoveSpeed;//고개를 돌릴 때 같이 조금 움직여주는 정도
    [SerializeField] float sightSensivitity;//고개의 움직임 속도.
    [SerializeField] float lookLimitX; //필드용이 아님
    [SerializeField] float lookLimitY;
    float currentAngleX;
    float currentAngleY;


    [SerializeField]GameObject go_NotCamDown;
    [SerializeField]GameObject go_NotCamUp;
    [SerializeField]GameObject go_NotCamLeft;
    [SerializeField]GameObject go_NotCamRight;

    float originPosY ;

    public void Reset()
    {
        tf_Crosshair.localPosition = new Vector3(0, -30, 0);
        currentAngleX = 0;
        currentAngleY = 0; //리셋을 시켜줘야 원래 보던 각도로 안보고있음
    }

    void Start()
    {
        originPosY = tf_Cam.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {   
        if (!InteractionController.isInteract)//대화가 아닐 때만 이 기능들이 작동하도록, 가장 최우선적 조건문
        {
            if (CameraController.onlyView)
            {
                //마우스 움직임에 따라 업데이트 해줘야되므로 크로스헤어를 여기에 해준다.
                CrosshairMovig();
                ViewMoving();
                KeyViewMoving();
                CameraLimit();
                NotCamUI();
                
            }
            else
            {
                FieldMoving();
                FieldLooking();
            }
        }
    }

    void FieldMoving()
    {
        if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            Vector3 t_Dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized; //y는 공중, 크기까지 정해지지 않도록 노말라이즈드 해준다.
        
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applySpeed = runSpeed;
            }
            else
            {
                applySpeed = walkSpped;
            }

            transform.Translate(t_Dir * applySpeed * Time.deltaTime, Space.Self); //1초에 원하는 applySpeed만큼 움직이도록. 그리고 어느방향이든 바라보고있는 방향으로 나아가도록 <-> world는 어느 방향이든 정해진 방향으로
        }
    }

    void FieldLooking()
    {
        if (Input.GetAxisRaw("Mouse X") != 0) //마우스 좌우 오른쪽은 1 왼쪽은 -1을 리턴
        {
            float t_AngleY = Input.GetAxisRaw("Mouse X"); //플레이어 기준y가 마우스 기준 좌우임
            Vector3 t_Rot = new Vector3(0, t_AngleY * fieldSensitivity, 0);
            transform.rotation = Quaternion.Euler(transform.localEulerAngles + t_Rot);
        }
        if (Input.GetAxisRaw("Mouse Y") != 0)
        {
            float t_AngleX = Input.GetAxisRaw("Mouse Y");
            currentAngleX -= t_AngleX;//빼주는 이유는 상하반전때문이다.
            currentAngleX = Mathf.Clamp(currentAngleX, -fieldLookLimitX, fieldLookLimitX); //자기자신을 클램프
            tf_Cam.localEulerAngles = new Vector3(currentAngleX, 0, 0); //이렇게 나온걸 x축만 건드릴 수 있도록
        }
    }

    void NotCamUI()
    {
        go_NotCamDown.SetActive(false); //일단 다 비활성화한다.
        go_NotCamUp.SetActive(false);
        go_NotCamLeft.SetActive(false);
        go_NotCamRight.SetActive(false);

        //이제 활성화되는 조건을 건다. => 더이상 움직이지 못할 때(리밋이랑 같거나 클 때)
        if (currentAngleY >= lookLimitX)
            go_NotCamRight.SetActive(true);
        else if (currentAngleY <= -lookLimitX)
            go_NotCamLeft.SetActive(true);

        if (currentAngleX <= -lookLimitY)
            go_NotCamUp.SetActive(true);
        else if (currentAngleX >= lookLimitY)
            go_NotCamDown.SetActive(true);
    }

    void CameraLimit() //y축의 1을 변수로 바꿔주는것이 좋다. =>1대신 쓸 변수 선언
    {
        if (tf_Cam.localPosition.x >= camBoundary.x)
        {
            tf_Cam.localPosition = new Vector3(camBoundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z); //넘을라고 그러면 지금 자기 위치에 고정시켜버림
        }
        else if (tf_Cam.localPosition.x <= -camBoundary.x)
        {
            tf_Cam.localPosition = new Vector3(-camBoundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z); //이건 -로 고정시켜버림
        }

        if (tf_Cam.localPosition.y >= originPosY + camBoundary.y) //여기도 y위치 생각해야댐
        {
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, originPosY + camBoundary.y, tf_Cam.localPosition.z); //지금 y좌표가 1이 더해져있어서 그걸 꼭 생각해야댐
        }
        else if (tf_Cam.localPosition.y <= originPosY - camBoundary.y)
        {
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, originPosY - camBoundary.y, tf_Cam.localPosition.z); 
        }
    }

    void KeyViewMoving()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 )  //axisraw는 왼쪽 오른쪽을 -1,1로 반환하고 안누르면 0을 반환한다.
        {
            currentAngleY += sightSensivitity * Input.GetAxisRaw("Horizontal"); //음수 양수를 구분하여 방향을 지정해줄 수 있음
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + sightMoveSpeed * Input.GetAxisRaw("Horizontal"), tf_Cam.localPosition.y , tf_Cam.localPosition.z);
        }
        if (Input.GetAxisRaw("Vertical") != 0 )  //axisraw는 왼쪽 오른쪽을 -1,1로 반환하고 안누르면 0을 반환한다.
        {
            currentAngleX += sightSensivitity * -Input.GetAxisRaw("Vertical"); //음수 양수를 구분하여 방향을 지정해줄 수 있음
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x , tf_Cam.localPosition.y + sightMoveSpeed * Input.GetAxisRaw("Vertical"), tf_Cam.localPosition.z);
        }
        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
    }

    void ViewMoving()
    {
        //고개를 돌리는 거 뿐만 아니라 마우스포인터에 따라서도 움직여야된다.
        if (tf_Crosshair.localPosition.x > (Screen.width / 2 - 100) || tf_Crosshair.localPosition.x < (-Screen.width / 2 + 100) )//여유롭게(나가지 못하게 막기도 했으니까)
        {
            //카메라 로테이션 y를 이용해서 x를 움직임
            
            currentAngleY += (tf_Crosshair.localPosition.x > 0 ) ? sightSensivitity : -sightSensivitity; //삼항연산자로 하여금 조건문을 통해 어떤값을 더할지 하는건데, 마우스가 왼쪽이냐 오른쪽이냐를 판별해준다.
            //고개를 무한히 돌리기보다 제한해주는게 좋음
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX); //inspector창에서 채워준다.
            
            float t_applySpeed = (tf_Crosshair.localPosition.x > 0) ? sightMoveSpeed : -sightMoveSpeed; // applyspeed를 삼항연산자로 받아서 사이트무브스피드에 따라 x축 이동하는 값을 만든다.
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + t_applySpeed, tf_Cam.localPosition.y, tf_Cam.localPosition.z );
        }
         if (tf_Crosshair.localPosition.y > (Screen.height / 2 - 100) || tf_Crosshair.localPosition.y < (-Screen.height / 2 + 100) )
        {
            currentAngleX += (tf_Crosshair.localPosition.y > 0 ) ? -sightSensivitity : sightSensivitity; //y축은 반전이 일어나서 -부호를 반대로 해줘야댐(fps를 생각)
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY); 

            float t_applySpeed = (tf_Crosshair.localPosition.y > 0) ? sightMoveSpeed : -sightMoveSpeed; // applyspeed를 삼항연산자로 받아서 사이트무브스피드에 따라 x축 이동하는 값을 만든다.
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, tf_Cam.localPosition.y + t_applySpeed, tf_Cam.localPosition.z );
        }
        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
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
