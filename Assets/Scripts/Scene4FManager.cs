using Unity.VisualScripting;
using UnityEngine;

public class Scene4FManager : MonoBehaviour
{
    public static Scene4FManager instance;

    public int tejidosHechos = 0;

    private void Awake()
    {
        instance = this;
    }

    public void AddTejido()
    {
        tejidosHechos = tejidosHechos + 1;
        if(tejidosHechos > 5)
        {
            StartCoroutine(SceneTransition.instance.FadeToBlack("Escena5F"));
        }
    }
}
