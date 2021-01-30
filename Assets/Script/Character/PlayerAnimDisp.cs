using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimDisp : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerBody;

    [SerializeField]
    private float _zOffset;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        EventManager EM = EventManager.Get(gameObject);
        if (EM != null)
        {
            EM.Evt_OnSwitchSliceMesh += OnSwitchSliceMesh;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(_playerBody != null)
        {
            gameObject.transform.position = new Vector3(_playerBody.transform.position.x,
                _playerBody.transform.position.y,
                _playerBody.transform.position.z + _zOffset);
        }
    }

    private void OnSwitchSliceMesh(GameObject newObj)
    {
        _playerBody = newObj;
    }
}
