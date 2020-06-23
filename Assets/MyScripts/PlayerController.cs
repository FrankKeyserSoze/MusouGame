using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    Rigidbody rigidbody;
    float walkForce = 100.0f;
    float maxWalkSpeed = 2.0f;
    float jumpForce = 250.0f;
    bool isAir;
    Vector3 lastPos;
    Animator animator;
    Vector3 diff;
    Vector3 diffY0;
    float diffVero;
    Vector3 antiWalkForce;
    AnimatorStateInfo info;
    Vector3 nlzForce;
    bool isAttack;
    public GameObject twoHWU;
    public GameObject twoHWA;
    public GameObject rLeg;
    bool isMove;
    public float twoHWADam;
    public float rLegDam;
    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody = gameObject.GetComponent<Rigidbody>();
        this.animator = GetComponent<Animator>();
        twoHWA.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        // //アニメーション遷移条件のSpeedに代入
        float lastVero = rigidbody.velocity.magnitude;
        diffVero = rigidbody.velocity.magnitude - lastVero;
        animator.SetFloat("Speed", lastVero);
        info = animator.GetCurrentAnimatorStateInfo(0);
        //AttackTagが付いているStateのとき攻撃中の旗を揚げる
        if(info.IsTag("Attack"))
        {
            isAttack = true;
        }
        else
        {
            isAttack = false;
        }
        //両手剣での攻撃モーション中
        if(info.IsName("2Hand-Sword-Attack2"))
        {
            StartCoroutine(ColliderEnabled());
             Debug.Log("Coroutine 開始");
            twoHWA.GetComponent<CapsuleCollider>().enabled = true;
            Debug.Log(twoHWA.GetComponent<CapsuleCollider>().enabled);
 
        }
        else
        {
            twoHWA.GetComponent<CapsuleCollider>().enabled = false;
        }
        //キックでの攻撃モーション中
        if(info.IsName("Hikick"))
        {
            rLeg.GetComponent<CapsuleCollider>().enabled = true;
        }
        else
        {
            rLeg.GetComponent<CapsuleCollider>().enabled = false;
        }
        //AttackTagがついているStateが進行してる時は無視
        if(isAttack == false)
        {
            if(Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("AttackTrigger");
            }
            if(isMove == false)
            {
                if(Input.GetKeyDown("1"))
                {
                    animator.SetTrigger("Wepon1Trigger");
                    StartCoroutine(SwitchWeapon());
                }
            }
            if(Input.GetKeyDown(KeyCode.Space) && isAir == false)
            {
                this.rigidbody.AddForce(transform.up * this.jumpForce);
                //空中にいるとき旗揚げ
                isAir = true;
                this.animator.SetTrigger("JumpTrigger");
                this.rigidbody.AddForce(antiWalkForce);
            }
        }
        else
        {
            return;
        }
    }
    
    
    void FixedUpdate()
    {
        // 速度制限
        // if(this.rigidbody.velocity.magnitude < this.maxWalkSpeed)
        // {
            //移動
            // if(Input.GetKey(KeyCode.W))
            // {
            //     this.rigidbody.AddForce(transform.forward * this.walkForce);
            // }
            // if(Input.GetKey(KeyCode.S))
            // {
            //     this.rigidbody.AddForce(transform.forward * this.walkForce * -1f);
            // }
            // if(Input.GetKey(KeyCode.D))
            // {
            //     this.rigidbody.AddForce(transform.right * this.walkForce);
            // }
            // if(Input.GetKey(KeyCode.A))
            // {
            //     this.rigidbody.AddForce(transform.right * this.walkForce * -1f);
            // }
        // }
        //VerticalとHorizontalAxisをそれぞれ前進、回転動作につなげる
        // float translation = Input.GetAxisRaw("Vertical") * speed;
        // float rotation = Input.GetAxisRaw("Horizontal") * rotationSpeed;
        // //Z軸へ前進
        // transform.Translate(0, 0, translation);
        // // Y軸を中心に回転
        // // transform.Rotate(0,rotation, 0);
        // //速度、回転を10f/s,100f/sにする
        // translation *= Time.deltaTime;
        // rotation *= Time.deltaTime;
        
        if(isAttack == true)
        {
            return;
        }
         
        // 速度制限と攻撃中でなかったら実行
        if(this.rigidbody.velocity.magnitude < this.maxWalkSpeed)
        {            
            // 縦横のAxisの値を取る
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            //Axisの値をベクトルに変換
            Vector3 force = new Vector3(x, 0, z);
            //ベクトルを正規化
            nlzForce = force.normalized;
            //正規化されたベクトルと歩く力を計算する
            nlzForce *= this.walkForce;
            // 物体に力を加える
            this.rigidbody.AddForce(nlzForce);
        }
        //1フレーム前のPlayerの位置との差を計り、どの方向に進んでいるかを計算する。
        this.diff = transform.position - lastPos;
        // キャラクターが上を向かないようにするためdiffのy軸方向のベクトルを0にする
        this.diffY0 = this.diff;
        this.diffY0.y = 0;
        //transform.positionの値を代入する動作を含むことによって、ここの値は、1つ前のフレームのものになる。
        lastPos = transform.position;
        // 少し動いたら
        if(diffY0.magnitude > 0.01f)
        {
            //差の方向を向き向くためのRotationの値を代入
            Quaternion playerRotation = Quaternion.LookRotation(diffY0);
            //最初に向いていた角度から差の方向へフレームごとに25%づつ回転する
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, 0.25f);
            isMove = true;
        }
        else
        {
            isMove = false;
        }       
        
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Floor"))
        {
            //地面に触れたら旗下げ
            isAir = false;
        }
    }
    IEnumerator SwitchWeapon()
    {
        yield return new WaitForSeconds(0.38f);
        twoHWA.SetActive(!twoHWA.activeSelf);
        twoHWU.SetActive(!twoHWU.activeSelf);
    }
    IEnumerator ColliderEnabled()
    {
        yield return new WaitForSeconds(0.4f);
        twoHWA.GetComponent<CapsuleCollider>().enabled = false;
        // Debug.Log(twoHWA.GetComponent<CapsuleCollider>().enabled);  
    }
    public void Hit()        // ヒット時のアニメーションイベント（今のところからっぽ。ないとエラーが出る）
    {
    }
    // void Sample()
    // {
    //     info = animator.GetCurrentAnimatorClipInfo(0);
    //     for(int i = 0; i < info.Length; i++)
    //     {
    //         Debug.Log(info[i].clip.name);
    //     }
    // }
}
