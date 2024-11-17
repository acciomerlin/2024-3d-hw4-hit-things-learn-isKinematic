using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    DiskPool diskPool;                       
    RoundController roundController;
    UserGUI userGUI;

    /**
     * implement ISceneController
    */
    public void LoadResources()
    {
        diskPool = SingletonAutoMonoBase<DiskPool>.Instance;
        roundController = SingletonAutoMonoBase<RoundController>.Instance;
        userGUI = SingletonAutoMonoBase<UserGUI>.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        SSDirector.GetInstance().CurrentScenceController = this;
        LoadResources();
    }

    // Update is called once per frame
    void Update()
    {
        if (!userGUI.showStartScreen)
        {
           if (Input.GetMouseButtonDown(0)) // 检测左键点击
            {
                Hit(Input.mousePosition); // 传递鼠标点击位置
            }
        }
 

    }

    public void FreeDisk(GameObject disk)
    {
        diskPool.FreeDisk(disk);
    }

    /**
     * implement IUserAction
    */
    public void Hit(Vector3 position)
    {
        Camera ca = Camera.main;
        Ray ray = ca.ScreenPointToRay(position);

        RaycastHit[] hits = Physics.RaycastAll(ray);

        // 遍历所有检测到的碰撞点
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            DiskModel disk = hit.collider.gameObject.GetComponent<DiskModel>();
            if (disk != null)
            {
                hit.collider.gameObject.transform.position = new Vector3(0, -7, 0);

                roundController.Record(disk);
                userGUI.SetPoints(roundController.GetPoints());
                break; // 退出循环，防止重复积分
            }
        }
    }


    public void Restart()
    {
        userGUI.SetMessage("");
        userGUI.SetPoints(0);
        roundController.Reset();
    }

    public void SetFlyMode(bool isPhy)
    {
        roundController.SetFlyMode(isPhy);
    }
}
