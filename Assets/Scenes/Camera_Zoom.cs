using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Zoom : MonoBehaviour
{
    void public GameObject Camera;
    void public float ZoomOutDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
