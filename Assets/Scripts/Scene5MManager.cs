using UnityEngine;

public class Scene5MManager : MonoBehaviour
{
    int conquerorsKilled = 0;
    public static Scene5MManager instance;
    public GameObject interaction2Dialogue;
    public GameObject interaction1Dialogue;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    public void AddSaino()
    {
        conquerorsKilled++;
        if (conquerorsKilled > 2)
        {
            interaction1Dialogue.SetActive(false);
            interaction2Dialogue.SetActive(true);
        }
    }
}
