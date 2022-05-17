using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPort : SingLeton<ViewPort>
{
    float minX;
    float maxX;
    float minY;
    float maxY;
    float middleX;//左边中点

    public float MaxX => maxX;
    private void Start()
    {
        //将视口坐标转化为世界坐标
        Camera mainCamera = Camera.main;
        Vector2 buttomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));//左下角世界位置
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));//右上
        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;

        minX = buttomLeft.x;
        minY = buttomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }
    public Vector3 PlayerMoveablePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        //限制玩家位置
        Vector3 position = Vector3.zero;//初始化

        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);//将玩家x设定在minx和max区间内
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);//将玩家y设定在miny和may区间内

        return position;// |————————————|maxY
                        // |            |
    }                   // |            |   视口
                        // |————————————|
                        //minx          maxX/MinY
    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {//随机敌人生成位置 |镜头外

        Vector3 position = Vector3.zero;//初始化

        position.x = maxX + paddingX;//向右偏移paddingX
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }
    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY)
    {//随机 右半边 位置移动
        Vector3 position = Vector3.zero;//初始化

        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        position.x = Random.Range(middleX, maxX - paddingX);

        return position;
    }
    public Vector3 RandomEnemyPosition(float paddingX, float paddingY)
    {//随机 全场 位置移动
        Vector3 position = Vector3.zero;//初始化

        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        position.x = Random.Range(minX + paddingX, maxX - paddingX);

        return position;
    }
}
