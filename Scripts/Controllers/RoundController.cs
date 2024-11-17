using UnityEngine;

public class RoundController : MonoBehaviour
{
    private IActionManager actionManager;          // 动作管理者
    private DiskPool diskPool;                     // 飞碟对象池
    private ScoreRecorder scoreRecorder;           // 分数记录器
    private UserGUI userGUI;                       // 用户界面
    private float sendTime = 0;                    // 时间计数器

    void Start()
    {
        diskPool = SingletonAutoMonoBase<DiskPool>.Instance;   // 获取飞碟对象池
        actionManager = SingletonAutoMonoBase<CCActionManager>.Instance; // 默认使用运动学模式
        scoreRecorder = new ScoreRecorder();
        userGUI = SingletonAutoMonoBase<UserGUI>.Instance;
    }

    public void Reset()
    {
        sendTime = 0;
        scoreRecorder.Reset();
    }

    public void Record(DiskModel disk)
    {
        scoreRecorder.Record(disk);
    }

    public int GetPoints()
    {
        return scoreRecorder.GetPoints();
    }

    public void SetFlyMode(bool isPhysis)
    {
        actionManager = isPhysis ? SingletonAutoMonoBase<PhysisActionManager>.Instance : SingletonAutoMonoBase<CCActionManager>.Instance;
    }

    private void SendDisk()
    {
        GameObject disk = diskPool.GetDisk();
        disk.transform.position = new Vector3(-disk.GetComponent<DiskModel>().direction.x * 7, Random.Range(0f, 8f), 0);
        actionManager.Fly(disk, disk.GetComponent<DiskModel>().speed, disk.GetComponent<DiskModel>().direction);
    }

    void Update()
    {
        sendTime += Time.deltaTime;
        if (sendTime >= 1f) // 每秒发送一次飞碟
        {
            sendTime = 0f;

            for (int i = 0; i < 5; i++) // 每次生成 5 个飞碟
            {
                SendDisk();
            }
        }
    }
}