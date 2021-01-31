using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Zoom : MonoBehaviour
{
    //[SerializeField]
    private GameObject CameraObj;

    [SerializeField, Tooltip("正交相机目标Size")]
    private float _cameraorthographicSize;

    [SerializeField, Tooltip("插值时间")]
    private float _lerpTime;

    private float LerpSchedule = -1f;
    private float StartLerpValue = 0f;


    void Start()
    {
        var RootObjs = gameObject.scene.GetRootGameObjects();

        foreach(var obj in RootObjs)
        {
            if(obj.tag == "MainCamera")
            {
                CameraObj = obj;
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CameraObj == null) return;

        if (other.tag == "Player")
        {
            var CameraComp = CameraObj.GetComponent<Camera>();
            if (CameraComp != null)
            {
                StartLerpValue = CameraComp.orthographicSize;
                LerpSchedule = 0f;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LerpSchedule == -1) return;

        if (CameraObj == null) return;

        var CameraComp = CameraObj.GetComponent<Camera>();
        if (CameraComp != null)
        {
            if(_lerpTime > 0)
            {
                LerpSchedule += Time.deltaTime;

                CameraComp.orthographicSize = Mathf.Lerp(StartLerpValue, _cameraorthographicSize,Mathf.Clamp01( LerpSchedule / _lerpTime));
                if (LerpSchedule >= _lerpTime) LerpSchedule = -1;
            }
            else
            {
                CameraComp.orthographicSize = _cameraorthographicSize;
            }
            
        }
    }
}
