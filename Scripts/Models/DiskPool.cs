using System.Collections.Generic;
using UnityEngine;

public class DiskPool : MonoBehaviour
{
    // 飞碟资源字典，根据 level 存储不同 prefab
    public Dictionary<int, GameObject> diskPrefabs = new Dictionary<int, GameObject>();

    // 对象池
    private List<GameObject> used;   // 正在使用的飞碟
    private List<GameObject> free;   // 空闲的飞碟
    private HashSet<Vector3> activePositions; // 用于记录已生成飞碟的位置，避免重叠

    private float minDistance = 2.0f; // 飞碟之间的最小距离

    void Start()
    {
        used = new List<GameObject>();
        free = new List<GameObject>();
        activePositions = new HashSet<Vector3>();

        // 加载不同的 prefab
        diskPrefabs[1] = Resources.Load<GameObject>("Prefabs/pumpkin");
        diskPrefabs[2] = Resources.Load<GameObject>("Prefabs/bird1");
        diskPrefabs[3] = Resources.Load<GameObject>("Prefabs/bird2");
    }

    // 获取飞碟
    public GameObject GetDisk()
    {
        GameObject disk;

        // 如果有空闲飞碟，从池中取出，否则创建新飞碟
        if (free.Count > 0)
        {
            disk = free[0];
            free.RemoveAt(0);
        }
        else
        {
            // 从 1、2、3 中随机选择一个 model
            int model = Random.Range(1, 4); 

            if (!diskPrefabs.ContainsKey(model))
            {
                Debug.LogError($"No prefab available for level {model}");
                return null;
            }

            // 实例化模型
            Vector3 spawnPosition = GetNonOverlappingPosition();
            disk = Instantiate(diskPrefabs[model], spawnPosition, Quaternion.identity);
            disk.AddComponent<DiskModel>();
        }

        // 设置飞碟属性
        SetDiskProperties(disk);

        // 将飞碟加入使用列表
        used.Add(disk);
        return disk;
    }

    // 释放飞碟的方法
    public void FreeDisk(GameObject disk)
    {
        activePositions.Remove(disk.transform.position); // 移除该位置
        disk.SetActive(false);
        used.Remove(disk);
        free.Add(disk);
    }

    // 设置飞碟属性
    private void SetDiskProperties(GameObject disk)
    {
        DiskModel data = disk.GetComponent<DiskModel>();

        // 统一得分为 1 分
        data.points = 1;

        // 随机速度设置（可以根据需要调整范围）
        data.speed = Random.Range(4.0f, 8.0f);

        // 随机方向，主要是水平方向偏移
        data.direction = new Vector3(Random.Range(-1f, 1f) > 0 ? 2 : -2, 1, 0);

        disk.SetActive(true);
    }

    // 获取一个不与现有位置重叠的生成位置
    private Vector3 GetNonOverlappingPosition()
    {
        Vector3 position;
        int maxAttempts = 100; // 最大尝试次数，避免死循环
        int attempts = 0;

        do
        {
            // 随机生成位置，调整范围以适应场景大小
            position = new Vector3(
                Random.Range(-10f, 10f), // X 坐标范围
                Random.Range(5f, 10f),  // Y 坐标范围
                Random.Range(-10f, 10f) // Z 坐标范围
            );

            attempts++;
        } while (IsOverlapping(position) && attempts < maxAttempts);

        activePositions.Add(position); // 记录新位置
        return position;
    }

    // 检查生成位置是否与现有飞碟的位置重叠
    private bool IsOverlapping(Vector3 position)
    {
        foreach (Vector3 activePosition in activePositions)
        {
            if (Vector3.Distance(activePosition, position) < minDistance)
            {
                return true;
            }
        }
        return false;
    }
}