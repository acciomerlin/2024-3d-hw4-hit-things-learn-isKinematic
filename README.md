# 【3d-game-hw4】打飞碟unity小游戏

## 1 游戏简介

1. 点击在飞的南瓜或者鸟来得分，点一个得一分，一视同仁；
2. 有两种模式：运动学/物理学飞行，物理学飞行即isKinematic ==false，飞行物体有碰撞效果；
3. 每一局限时30s，有最高得分记录。

## 2 Assets架构与各文件说明

<img src="assets/image-20241117233414635.png" alt="image-20241117233414635" style="zoom:50%;" />

- #### Resources: Prefabs里是3种飞的东西，Skybox里是不同模式的天空，便于区分。

  <img src="assets/image-20241117233729596.png" alt="image-20241117233729596" style="zoom:50%;" />

  <img src="assets/image-20241117233745780.png" alt="image-20241117233745780" style="zoom:50%;" />

  - 用到的免费素材来自于与Resources文件夹同级的另外3个文件夹内，都是官网free assets。

- #### Scenes: samplescene里的firstcontroller挂载脚本作为入口。

  <img src="assets/image-20241117233616856.png" alt="image-20241117233616856" style="zoom:50%;" />

- #### Scripts: 控制游戏逻辑的脚本，尽量使用MVC结构：


​	<img src="assets/image-20241117234623127.png" alt="image-20241117234623127" style="zoom:50%;" />

- **SingletonAutoMonoBase: 单例模式基类**

  ```c#
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  
  public class SingletonAutoMonoBase<T> : MonoBehaviour where T : MonoBehaviour
  {
      private static T instance;
      public static T Instance
      {
          get
          {
              if (instance == null)
              {
                  GameObject obj = new GameObject();
                  obj.name = typeof(T).ToString();
                  DontDestroyOnLoad(obj); // 支持跨scene
                  instance = obj.AddComponent<T>();
              }
              return instance;
          }
      }
  }
  ```

- **Controller：**

  <img src="assets/image-20241117235059857.png" alt="image-20241117235059857" style="zoom:50%;" />

  <img src="assets/image-20241117235111773.png" alt="image-20241117235111773" style="zoom:50%;" />

- **Model：使用了对象池管理飞行物**

  <img src="assets/image-20241117235138719.png" alt="image-20241117235138719" style="zoom:50%;" />

- **View：用户交互逻辑实现**

  - UserGUI：用户界面类，负责渲染游戏的 UI 元素，包括剩余时间、提示信息和游戏规则等。

    ![image-20241117235330936](assets/image-20241117235330936.png)

    ![image-20241117235341778](assets/image-20241117235341778.png)

    ![image-20241117235353283](assets/image-20241117235353283.png)


## 3 游戏脚本UML图

![1](assets/1.png)

## 4 游戏演示视频与创新点

https://www.bilibili.com/video/BV1HCU7Y7EPP

对鼠标做了粒子系统追踪，提升玩家在点击时的体感。