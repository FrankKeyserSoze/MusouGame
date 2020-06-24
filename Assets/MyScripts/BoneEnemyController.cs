using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneEnemyController : MonoBehaviour
{
    public GameObject player;
    float boneHp = 100f;
    public float twoHWADam;
    public float rLegDam;
    int twoHWACriticalRate;
    public float twoHWACriticalDam;
    int rLegCriticalRate;
    public float rLegCriticalDam;
    Animator animator;

    
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
        if(other.gameObject.CompareTag("2HWATag"))
        {
            twoHWACriticalRate = Random.Range(1,11);
            if(twoHWACriticalRate <= 3)
            {
                twoHWADam *= twoHWACriticalDam;
            }
            else
            {
                twoHWADam = 30f;
            }
            boneHp -= twoHWADam;
        }
        if(other.gameObject.CompareTag("RLegTag"))
        {
            rLegCriticalRate = Random.Range(1,11);
            if(rLegCriticalRate <= 5)
            {
                rLegDam *= rLegCriticalDam;
            }
            else
            {
                rLegDam = 15f;
            }
            boneHp -= rLegDam;
            
            animator.SetTrigger("HitTrigger");
        }
    }
}
