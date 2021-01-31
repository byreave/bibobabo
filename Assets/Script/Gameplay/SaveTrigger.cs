using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject TeleportPoint = null;

    bool bAlreadyTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (bAlreadyTrigger) return;

        if (other.tag == "Player")
        {
            var RootObjs = gameObject.scene.GetRootGameObjects();

            foreach (var obj in RootObjs)
            {
                if (obj.tag == "PlayerDisp")
                {
                    var EM = EventManager.Get(obj);
                    if (EM != null)
                    {
                        EM.Evt_UpdateSavePos.Invoke(TeleportPoint.transform.position);
                    }

                    bAlreadyTrigger = true;
                    break;
                }
            }
        }
    }

}
