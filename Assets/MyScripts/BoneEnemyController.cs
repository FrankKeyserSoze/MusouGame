using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneEnemyController : MonoBehaviour
{
    float boneHp = 100f;
    Animator animator;
    PlayerController playerController;
    int damage;

    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(boneHp);
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
}
