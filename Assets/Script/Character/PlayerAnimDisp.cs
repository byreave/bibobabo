using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimDisp : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerBody;

    [SerializeField]
    private string BodyPrefabName;

    [SerializeField]
    private float _zOffset;

    private Vector3 LastSavePos;
    private Vector3 LastSaveScale;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        EventManager EM = EventManager.Get(gameObject);
        if (EM != null)
        {
            EM.Evt_OnSwitchPlayerBody += OnSwitchSliceMesh;
            EM.Evt_UpdateSavePos += UpdateSavePos;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_playerBody != null)
        {
            LastSavePos = _playerBody.transform.position;
            LastSaveScale = _playerBody.transform.localScale;

            
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            RollBackToLastSavePoint();
        }
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

    private void OnResetToLastSavePoint()
    {

    }

    private void UpdateSavePos(Vector3 SavePos)
    {
        LastSavePos = SavePos;
    }

    private void RollBackToLastSavePoint()
    {
        Vector3 pos = _playerBody.transform.position;
        Quaternion rot = _playerBody.transform.rotation;

        Destroy(_playerBody);

        _playerBody = Instantiate(Resources.Load("Prefabs/"+ BodyPrefabName), pos, rot) as GameObject;
        EventManager EM = EventManager.Get(gameObject);
        if(EM != null)
        {
            EM.Evt_OnSwitchPlayerBody.Invoke(_playerBody);
        }

        _playerBody.transform.position = LastSavePos;
        _playerBody.transform.localScale = LastSaveScale;
    }

}
