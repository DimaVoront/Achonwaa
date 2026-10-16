using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XRBowInteractable : MonoBehaviour
{
    
    private LineRenderer _lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
