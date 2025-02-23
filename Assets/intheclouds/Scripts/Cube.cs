using Oculus.Interaction;
using UnityEngine;

public class Cube : MonoBehaviour
{

    private Grabbable grabbable;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grabbable = GetComponentInChildren<Grabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
