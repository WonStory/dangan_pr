using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rigidbody; //퍼블릭으로 설정하고 직접넣어줘도 되지만 private로 설정하고 직접 넣어줘도 된다.
    public float speed = 10f; //스피드 점프높이 대쉬기능
    public float jumpHeight = 3f;
    public float dash = 5f;
    public float rotSpeed = 9F;

    private Vector3 dir = Vector3.zero; //dir을 벡터3의 zero로 설정

    private bool gorund = false;
    //땅을 구분하기 위해 레이어를 씀
    public LayerMask layer;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>(); //프라이빗으로 직접 넣어줌
    }

    // Update is called once per frame
    void Update()
    {
        //움직임을 계산해줘야된다. x는 a,d키의 방향, z는 w,s키의 방향
        dir.x = Input.GetAxis("Horizontal"); 
        dir.z = Input.GetAxis("Vertical");
        dir.Normalize();//대각선 속도 해결
        //dir이 zero가 아니라면 키입력이 된 상태
        CheckGround();

        if (Input.GetButtonDown("Jump") && gorund) //참일 때
        {
            Vector3 jumpPower = Vector3.up * jumpHeight;
            rigidbody.AddForce(jumpPower, ForceMode.VelocityChange);
            //but 점프를 계속 누르면 계속 하늘로 감 => 땅에서만 가동하게끔 bool을 쓴다.
        }
    }
    //회전할 때 확확도는 것을 막아줌
    private void FixedUpdate()
    {
        if (dir != Vector3.zero)
        {   
            //지금 바라보는 방향의 부호 != 나아갈 방향 부호
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z) )
            {
                //이럴 때 조금만 회전시켜주면된다.
                transform.Rotate(0,1,0);
            }
            //러프를 써야댐, 서서히 변하긴 하는데 타임.델타타임만 넣으면 너무 느려서 rot스피드를 지정해서 곱해줌
            transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
        }
        rigidbody.MovePosition(this.gameObject.transform.position + dir * speed * Time.deltaTime);
        //대각선으로 갈때 더 빨라짐 => 정규화를 안해서 그럼, 방향을 구한다음에 전부다 1로 처리를 해야된다.
        //그리고 정반대로 얼굴을 돌릴 때 애매하게 움직임(부호가 반대인 키를 누를 때 이런 버그가 일어남)
    }

    void CheckGround()
    {
        RaycastHit hit;
        //조건문으로 레이저를 쏜다. 어느위치에서 발사해서 어느방향으로 얼마만큼 쏠건지
        if (Physics.Raycast(transform.position + (Vector3.up * 0.2f), Vector3.down, out hit, 0.4f, layer ))
        {   //발끝에서 레이저를 쏜다(현재 위치), 다만 발끝이 땅에 파묻히면 검출이 안되므로 살짝 띄워줌, 검출이 되면 hit에 담아라
            gorund = true;
        }
        else
        {
            gorund = false;
        }
    }
}
