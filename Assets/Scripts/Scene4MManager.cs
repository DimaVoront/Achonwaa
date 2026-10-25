using UnityEngine;

public class Scene4MManager : MonoBehaviour
{
    int sainosHunted = 0;
    public static Scene4MManager instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    public void AddSaino()
    {
        sainosHunted++;
        if(sainosHunted > 2)
        {
            StartCoroutine(SceneTransition.instance.FadeToBlack("Escena5M"));
        }
    }
}
