using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysisFlyAction : SSAction
{
    private float speed;            // 水平速度
    private Vector3 direction;      // 飞行方向

    /**
     * 工厂方法：创建一个新的 PhysisFlyAction 实例
     * @param direction 飞行方向
     * @param speed 飞行速度
     * @returns 新创建的 PhysisFlyAction
     */
    public static PhysisFlyAction GetSSAction(Vector3 direction, float speed)
    {
        PhysisFlyAction action = ScriptableObject.CreateInstance<PhysisFlyAction>();
        action.speed = speed;
        action.direction = direction.normalized; // 确保方向向量被归一化
        return action;
    }

    /**
     * 动作初始化：设置刚体属性并赋予初速度
     */
    public override void Start()
    {
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

        if (rigidbody == null)
        {
            Debug.LogError("PhysisFlyAction: Missing Rigidbody component.");
            return;
        }

        rigidbody.isKinematic = false;               // 启用物理模拟
        rigidbody.velocity = speed * direction;      // 设置初始速度
        // 添加额外的水平推动力以提高发射速度
        Vector3 horizontalForce = direction * speed * 1.0f; 
        rigidbody.AddForce(horizontalForce, ForceMode.Impulse); // 使用冲量模式
    }

    /**
     * 每帧更新：检测飞碟是否超出边界
     */
    public override void Update()
    {
        if (transform.position.y < -6)               // 检测飞碟是否到达底部
        {
            DestroyAction();
        }
    }

    /**
     * 销毁动作并触发回调
     */
    private void DestroyAction()
    {
        this.destroy = true;
        this.enable = false;

        // 通知回调动作已完成
        callback?.SSActionEvent(this);
    }
}