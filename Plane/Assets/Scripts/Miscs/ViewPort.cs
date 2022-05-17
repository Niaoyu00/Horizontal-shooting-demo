using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPort : SingLeton<ViewPort>
{
    float minX;
    float maxX;
    float minY;
    float maxY;
    float middleX;//����е�

    public float MaxX => maxX;
    private void Start()
    {
        //���ӿ�����ת��Ϊ��������
        Camera mainCamera = Camera.main;
        Vector2 buttomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));//���½�����λ��
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));//����
        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;

        minX = buttomLeft.x;
        minY = buttomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }
    public Vector3 PlayerMoveablePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        //�������λ��
        Vector3 position = Vector3.zero;//��ʼ��

        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);//�����x�趨��minx��max������
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);//�����y�趨��miny��may������

        return position;// |������������������������|maxY
                        // |            |
    }                   // |            |   �ӿ�
                        // |������������������������|
                        //minx          maxX/MinY
    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {//�����������λ�� |��ͷ��

        Vector3 position = Vector3.zero;//��ʼ��

        position.x = maxX + paddingX;//����ƫ��paddingX
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }
    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY)
    {//��� �Ұ�� λ���ƶ�
        Vector3 position = Vector3.zero;//��ʼ��

        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        position.x = Random.Range(middleX, maxX - paddingX);

        return position;
    }
    public Vector3 RandomEnemyPosition(float paddingX, float paddingY)
    {//��� ȫ�� λ���ƶ�
        Vector3 position = Vector3.zero;//��ʼ��

        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        position.x = Random.Range(minX + paddingX, maxX - paddingX);

        return position;
    }
}
