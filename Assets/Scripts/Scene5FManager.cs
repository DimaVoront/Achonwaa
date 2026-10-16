using UnityEngine;

public class Scene5FManager : MonoBehaviour
{
    public static Scene5FManager instance;
    int fruitsCollected = 0;
    public GameObject interaction2Dialogue;
    public GameObject interaction1Dialogue;
    bool hasWater = false;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FruitColected()
    {
        fruitsCollected++;
        if(fruitsCollected > 3)
        {
            interaction2Dialogue.SetActive(true);
            interaction1Dialogue.SetActive(false);
        }
    }

    public void FruitRemoved()
    {
        fruitsCollected = fruitsCollected - 1;
    }

    public void GrabWater()
    {
        if (fruitsCollected > 3)
        {
            hasWater = true;
            Debug.Log("Se acabo xd");
        }
        
    }
}
