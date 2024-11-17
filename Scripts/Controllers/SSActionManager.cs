using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour
{
    // 动作管理
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>(); // 当前动作
    private List<SSAction> waitingAdd = new List<SSAction>();                    // 等待添加的动作
    private List<int> waitingDelete = new List<int>();                           // 等待删除的动作

    // 每帧更新动作管理器
    protected void Update()
    {
        // 添加等待执行的动作
        AddPendingActions();

        // 更新当前正在执行的动作
        UpdateRunningActions();

        // 移除已完成的动作
        RemoveCompletedActions();
    }

    // 运行一个动作：初始化动作并加入等待队列
    public void RunAction(GameObject gameObject, SSAction action, ISSActionCallback manager)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = manager;

        waitingAdd.Add(action);
        action.Start();
    }

    // 添加等待中的动作到管理器
    private void AddPendingActions()
    {
        foreach (SSAction action in waitingAdd)
        {
            actions[action.GetInstanceID()] = action;
        }
        waitingAdd.Clear();
    }

    // 更新当前正在运行的动作
    private void UpdateRunningActions()
    {
        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction action = kv.Value;

            if (action.destroy) // 动作标记为销毁
            {
                waitingDelete.Add(action.GetInstanceID());
            }
            else if (action.enable) // 动作激活时运行
            {
                action.Update();
            }
        }
    }

    // 移除已完成的动作
    private void RemoveCompletedActions()
    {
        foreach (int key in waitingDelete)
        {
            if (actions.TryGetValue(key, out SSAction action))
            {
                actions.Remove(key);
                Destroy(action);
            }
        }
        waitingDelete.Clear();
    }

    // 动作管理器初始化逻辑（如有需要可扩展）
    protected void Start()
    {
    }
}