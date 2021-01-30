using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementComp : MonoBehaviour
{
    [SerializeField, Tooltip("�������Ŀ��")]
    private Transform _target;

    [SerializeField, Tooltip("���ƽ���ٶ�")]
    private float _smooth;

    [SerializeField, Tooltip("������ɫ�ľ���")]
    private float _zdist;

    void Start()
    {
        if (_target == null) return;

        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = new Vector3(_target.position.x, _target.position.y, _target.position.z - _zdist);
    }

    void LateUpdate()
    {
        if (_target == null) return;

        Vector3 move_position = new Vector3(_target.position.x, _target.position.y, _target.position.z - _zdist);
        transform.position = Vector3.Lerp(transform.position, move_position, _smooth);
    }
}
