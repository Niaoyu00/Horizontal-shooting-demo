using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] float speed = 360f;//旋转速度
    [SerializeField] Vector3 angle;//角度

    void Update()
    {
        //自转
        transform.Rotate(angle * speed * Time.deltaTime);
    }
}
