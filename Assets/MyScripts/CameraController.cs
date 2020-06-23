using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        //MainCameraの初期位置とPlayerの位置とのベクトルの差を求める
        offset = transform.position - player.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        //offsetの間隔を空けながらPlayerを追従する
        gameObject.transform.position = player.transform.position + offset;
    }
}

