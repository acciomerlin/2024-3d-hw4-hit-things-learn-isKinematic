using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    IUserAction userAction;

    int points;                   // 当前得分
    int highestScore;             // 最高分
    float leftTime = 30f;         // 剩余时间
    bool isGameOver = false;      // 游戏是否结束
    string selectedMode = "";     // 当前选择的模式
    public bool showStartScreen = true;

    public Material kinematicsSkybox;  // 运动学模式的 Skybox
    public Material physisSkybox;      // 物理学模式的 Skybox

    private GameObject mouseTrail;     // 鼠标拖尾粒子系统

    public void SetMessage(string gameMessage)
    {
        isGameOver = true;
    }

    public void SetPoints(int points)
    {
        this.points = points;
        if (points > highestScore)
        {
            highestScore = points; // 更新最高分
        }
    }

    public void SetTime(float time)
    {
        leftTime = time;
    }

    void Start()
    {
        points = 0;
        highestScore = PlayerPrefs.GetInt("HighestScore", 0); // 读取最高分
        userAction = SSDirector.GetInstance().CurrentScenceController as IUserAction;

        // 动态加载 Skybox
        kinematicsSkybox = Resources.Load<Material>("Skyboxes/KSkybox");
        physisSkybox = Resources.Load<Material>("Skyboxes/PSkybox");

        if (kinematicsSkybox == null || physisSkybox == null)
        {
            Debug.LogError("Skybox materials not found! Check the file paths.");
        }

        // 初始化鼠标拖尾粒子效果
        InitializeMouseTrail();
    }

    void InitializeMouseTrail()
    {
        mouseTrail = new GameObject("MouseTrail");
        ParticleSystem ps = mouseTrail.AddComponent<ParticleSystem>();

        var main = ps.main;
        main.startColor = Color.yellow;
        main.startSize = 0.1f;
        main.startLifetime = 0.4f;

        var emission = ps.emission;
        emission.rateOverTime = 50;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    void OnGUI()
    {
        if (showStartScreen)
        {
            DrawStartOrEndScreen();
            return;
        }

        DrawInGameUI();
    }

    private void DrawStartOrEndScreen()
    {
        GUIStyle titleStyle = new GUIStyle
        {
            normal = { textColor = Color.white },
            fontSize = 50,
            alignment = TextAnchor.MiddleCenter
        };

        GUIStyle style = new GUIStyle
        {
            normal = { textColor = Color.white },
            fontSize = 30,
            alignment = TextAnchor.MiddleCenter
        };

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 30, // 设置字体大小
            alignment = TextAnchor.MiddleCenter // 居中对齐
        };

        // 绘制背景
        GUI.color = new Color(0, 0, 0, 0.5f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = Color.white;

        GUI.Label(new Rect(Screen.width / 2 - 200, 100, 400, 50), "Hit Things in 30s", titleStyle);
        GUI.Label(new Rect(Screen.width / 2 - 200, 200, 400, 50), $"Highest Score: {highestScore}", style);

        // 显示当前模式
        string modeText = "Current Mode: NONE";
        if (selectedMode != "")
        {
            modeText = selectedMode == "Kinematics" ? "Current Mode: Kinematics" : "Current Mode: Physis";
        }
        GUI.Label(new Rect(Screen.width / 2 - 200, 300, 400, 50), modeText, style);

        // 模式切换按钮
        if (GUI.Button(new Rect(Screen.width / 2 - 100, 400, 200, 50), "Switch Mode", buttonStyle))
        {
            if (selectedMode == "Kinematics")
            {
                selectedMode = "Physis";
                userAction.SetFlyMode(true);
                RenderSettings.skybox = physisSkybox; // 切换到物理学模式 Skybox
            }
            else
            {
                selectedMode = "Kinematics";
                userAction.SetFlyMode(false);
                RenderSettings.skybox = kinematicsSkybox; // 切换到运动学模式 Skybox
            }
        }

        // 开始/重新开始按钮
        string buttonText = isGameOver ? "Restart" : "Start";
        if (!string.IsNullOrEmpty(selectedMode))
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, 500, 200, 50), buttonText, new GUIStyle(GUI.skin.button) { fontSize = 30 }))
            {
                if (isGameOver)
                {
                    ResetGame();
                }
                showStartScreen = false;
                userAction.Restart();
            }
        }
        else
        {
            GUI.Label(new Rect(Screen.width / 2 - 200, 520, 400, 50), "Please select a mode first!", style);
        }

        if (isGameOver)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height - 150, 300, 50), $"Your Score: {points}", style);
        }
    }

    private void DrawInGameUI()
    {
        GUIStyle style = new GUIStyle
        {
            normal = { textColor = Color.white },
            fontSize = 30
        };

        GUI.Label(new Rect(20, 20, 200, 50), $"Score: {points}", style);
        GUI.Label(new Rect(20, 60, 200, 50), $"Highest: {highestScore}", style);

        GUI.Label(new Rect(Screen.width - 200, 20, 200, 50), $"Left Time: {Mathf.CeilToInt(leftTime)}s", style);
    }

    void Update()
    {
        if (!showStartScreen)
        {
            // 更新鼠标拖尾位置
            if (mouseTrail != null)
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                mouseTrail.transform.position = mouseWorldPosition;
            }

            leftTime -= Time.deltaTime;
            if (leftTime <= 0)
            {
                showStartScreen = true;
                isGameOver = true;

                SaveHighestScore();
            }
        }
    }

    private void ResetGame()
    {
        points = 0;
        leftTime = 30f; // 重置为 30 秒
        showStartScreen = false;
        isGameOver = false;
    }

    private void SaveHighestScore()
    {
        PlayerPrefs.SetInt("HighestScore", highestScore);
        PlayerPrefs.Save();
    }
}