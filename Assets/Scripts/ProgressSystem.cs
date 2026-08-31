using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressSystem : MonoBehaviour
{
    public static ProgressSystem instance;
    public enum CurrentScene{Scene1, Scene2, Scene3, Scene4, Scene5, Scene6, Scene7, Scene8, Scene9};

    public static CurrentScene currentScene;

    public bool isFemale = false;

    public Dictionary<string, int[,]> sceneProgressCheckpoints = new Dictionary<string, int[,]>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    public void SceneCheckpointUpdate(string sceneName)
    {
        if (sceneProgressCheckpoints.ContainsKey(sceneName))
        {
            int[,] checkpointData = sceneProgressCheckpoints[sceneName];

            checkpointData[1, 0] += 1;

            sceneProgressCheckpoints[sceneName] = checkpointData;

            if (checkpointData[1, 0] >= checkpointData[0, 0])
            {
                NextStage();
            }
        }
        else
        {
            Console.WriteLine("Scene not found in progress dictionary");
        }
    }

    void Update()
    {
        
    }

    public void UpdateProgress(CurrentScene cur)
    {
        currentScene = cur;
    }

    public void NextStage()
    {
        switch (currentScene)
        {
            case CurrentScene.Scene1:
                SceneManager.LoadScene("Escena2");
                break;
            case CurrentScene.Scene2:
                SceneManager.LoadScene("Escena3");
                break;
            case CurrentScene.Scene3:
                if (isFemale)
                {
                    SceneManager.LoadScene("Escena4F");
                    break;
                }
                else
                {
                    SceneManager.LoadScene("Escena4M");
                    break;
                }
            case CurrentScene.Scene4:
                if (isFemale)
                {
                    SceneManager.LoadScene("Escena5F");
                    break;
                }
                else
                {
                    SceneManager.LoadScene("Escena5M");
                    break;
                }
            case CurrentScene.Scene5:
                if (isFemale)
                {
                    SceneManager.LoadScene("Escena6F");
                    break;
                }
                else
                {
                    SceneManager.LoadScene("Escena6M");
                    break;
                }
            case CurrentScene.Scene6:
                SceneManager.LoadScene("Escena7");
                break;
            case CurrentScene.Scene7:
                SceneManager.LoadScene("Escena8");
                break;
            case CurrentScene.Scene8:
                SceneManager.LoadScene("Escena9");
                break;

        }
    }

}
