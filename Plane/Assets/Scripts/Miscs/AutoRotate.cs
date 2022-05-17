using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] float speed = 360f;//��ת�ٶ�
    [SerializeField] Vector3 angle;//�Ƕ�

    void Update()
    {
        //��ת
        transform.Rotate(angle * speed * Time.deltaTime);
    }
}
