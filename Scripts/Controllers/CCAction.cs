using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCFlyAction : SSAction
{
    float gravity;          
    float speed;            
    Vector3 direction;     
    float time;            

    /**
     * 工厂方法：创建一个新的 CCFlyAction 实例
     * @param direction 飞行方向
     * @param speed 飞行速度
     * @returns 新创建的 CCFlyAction
     */
    public static CCFlyAction GetSSAction(Vector3 direction, float speed)
    {
        CCFlyAction action = ScriptableObject.CreateInstance<CCFlyAction>();
        action.gravity = 9.8f;       // 默认重力加速度
        action.time = 0;            // 初始化时间
        action.speed = speed;       // 设置飞行速度
        action.direction = direction; // 设置飞行方向
        return action;
    }

    public override void Start()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true; // 禁用刚体的物理行为
    }

    public override void Update()
    {
        time += Time.deltaTime; 

        transform.Translate(Vector3.down * gravity * time * Time.deltaTime);
        transform.Translate(direction * speed * Time.deltaTime);

        // 如果飞碟到达底部，则标记动作完成并触发回调
        if (this.transform.position.y < -6)
        {
            this.destroy = true;
            this.enable = false;

            // 通知动作完成
            this.callback.SSActionEvent(this);
        }
    }
}