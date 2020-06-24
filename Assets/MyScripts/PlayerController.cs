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
    AnimatorStateInfo info;
    Vector3 nlzForce;
    bool isAttack;
    public GameObject twoHWU;
    public GameObject twoHWA;
    public GameObject rLeg;
    bool isAttackDone;
    bool isArmed;
  
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
        
            if(isAttackDone == false)
            {
                twoHWA.GetComponent<CapsuleCollider>().enabled = true;
            }
            else
            {
                twoHWA.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
        else
        {
            twoHWA.GetComponent<CapsuleCollider>().enabled = false;
            isAttackDone = false;
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
            //攻撃
            if(Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("AttackTrigger");
            }

            //動いていない時
                if(isArmed == false)
                {
                    //武器交換
                    if(Input.GetKeyDown("1"))
                    {
                        animator.SetTrigger("Wepon1Trigger");
                        StartCoroutine(SwitchWeapon());
                        isArmed = true;
                    }
                }
                else
                {
                    //武器交換
                    if(Input.GetKeyDown("1"))
                    {
                        animator.SetTrigger("Wepon1Trigger");
                        StartCoroutine(SwitchWeapon());
                        isArmed = false;
                    }
                }

            //空中にいない時に入力されたら
            if(Input.GetKeyDown(KeyCode.Space) && isAir == false)
            {
                this.rigidbody.AddForce(transform.up * this.jumpForce);

                //空中にいるとき旗揚げ
                isAir = true;

                this.animator.SetTrigger("JumpTrigger");
            }
        }
        else
        {
            return;
        }

        Debug.Log(isArmed);

    }
    
    
    void FixedUpdate()
    {
        
        if(isAttack == true)
        {
            return;
        }

        if(isArmed == true)
        {
            maxWalkSpeed = 1.5f;
        }
        else
        {
            maxWalkSpeed = 2.0f;
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
            nlzForce *= walkForce;
            // 物体に力を加える
            rigidbody.AddForce(nlzForce);
        }

        //1フレーム前のPlayerの位置との差を計り、どの方向に進んでいるかを計算する。
        diff = transform.position - lastPos;
        // キャラクターが上を向かないようにするためdiffのy軸方向のベクトルを0にする
        diffY0 = diff;
        diffY0.y = 0;
        //transform.positionの値を代入する動作を含むことによって、ここの値は、1つ前のフレームのものになる。
        lastPos = transform.position;

        // 少し動いたら
        if(diffY0.magnitude > 0.01f)
        {
            //差の方向を向き向くためのRotationの値を代入
            Quaternion playerRotation = Quaternion.LookRotation(diffY0);
            //最初に向いていた角度から差の方向へフレームごとに25%づつ回転する
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, 0.25f);
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

    //武器を手に持つモーションが始まってから背中の武器と手の武器が重なるまで待つ
    IEnumerator SwitchWeapon()
    {
        yield return new WaitForSeconds(0.38f);
        twoHWA.SetActive(!twoHWA.activeSelf);
        twoHWU.SetActive(!twoHWU.activeSelf);
    }
   
    //両手剣攻撃モーションのEventで呼ばれる
    public void AttackDone()
    {
        isAttackDone = true;
    }

   
    
}
