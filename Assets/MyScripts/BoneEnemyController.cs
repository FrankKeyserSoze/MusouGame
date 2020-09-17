using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneEnemyController : MonoBehaviour
{
    float boneHp = 100f;
    Animator animator;
    PlayerController playerController;
    int damage;
    float time;
    Rigidbody rigidbody;
    Vector3 direction;
    public float moveSpeed;
    public ENEMY_STATE enemyState;

    public PlayerDetection playerDetection;

    public AttackRange attackRange;

    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        enemyState = ENEMY_STATE.WAIT;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(boneHp);
        // time += Time.deltaTime;
        // if(enemyState == ENEMY_STATE.WAIT)
        // {
        //     if (time > 10f)
        //     {
        //         time = 0f;

        //         transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), new Vector3(0, 1, 0));

        //         direction = transform.position.normalized;

        //         enemyState = ENEMY_STATE.MOVE;
        //     }
        // }

        // if (enemyState == ENEMY_STATE.MOVE)
        // {
        //     Move();
        //     if(time > 5f)
        //     {
        //         time = 0f;
        //         enemyState = ENEMY_STATE.WAIT;
        //     }
        // }
        // Debug.Log(time);
        if(playerDetection.isSearching == true && attackRange.isAttack == false)
        {
            transform.LookAt(playerDetection.player.transform);
            rigidbody.AddForce(transform.forward * moveSpeed);

            
        }
        if(attackRange.isAttack == true)
        {
            playerDetection.isSearching = false;
            rigidbody.velocity = Vector3.zero;

        }

    }

    void OnTriggerEnter(Collider other)
    {
        //ぶつかった相手が両手剣なら
        if(other.gameObject.CompareTag("2HWATag"))
        {
            //playerControllerが空なら
            if(playerController == null)
            {
                //WeaponControllerを介してplayerControllerを取得
                playerController = other.gameObject.GetComponent<WeaponController>().playerController;
            }
            
            damage = playerController.SetTwoHWADamage();

            boneHp -= damage;

            animator.SetTrigger("HitTrigger");
        }
        
        //ぶつかった相手が右足なら
        if(other.gameObject.CompareTag("RLegTag"))
        {
            //playerControllerが空なら
            if(playerController == null)
            {
                //WeaponControllerを介してplayerControllerを取得
               playerController = other.gameObject.GetComponent<WeaponController>().playerController;
            }

            damage = playerController.SetTwoHWADamage();

            boneHp -= damage;

        }
    }

    void Move()
    {
        
        
        rigidbody.AddForce(direction * moveSpeed);
    }

    void Attack()
    {
        Debug.Log("attack");
        enemyState = ENEMY_STATE.ATTACK;
    }

    public enum ENEMY_STATE
    {
        WAIT, MOVE, ATTACK
    }
}
