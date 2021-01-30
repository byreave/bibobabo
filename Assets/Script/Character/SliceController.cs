using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine;

public class SliceController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("切割平面")]
    private GameObject _slicePlane = null;

    [SerializeField]
    [Tooltip("切割目标")]
    private GameObject _sliceMesh = null;

    void Start()
    {
        EventManager EM = EventManager.Get(gameObject);
        if (EM != null)
        {
            EM.Evt_InitVolume.Invoke(_sliceMesh);
        }
    }

    int count = 0;
    bool over = false;

    void Update()
    {
        //Test
        if (over) return;
        if (count < 600)
        {
            Debug.Log(count);
            count++;
        }
        else
        {
            over = true;
            Slice();
        }
    }

    public void Slice()
    {
        SlicedHull hull = SliceObject(_sliceMesh, _slicePlane.transform, null);
        if (hull != null)
        {
            GameObject bottom = hull.CreateLowerHull(_sliceMesh, null);
            GameObject top = hull.CreateUpperHull(_sliceMesh, null);
            //AddHullComponents(bottom);
            //AddHullComponents(top);
            Destroy(_sliceMesh.gameObject);


            EventManager EM = EventManager.Get(gameObject);
            if (EM != null)
            {
                EM.Evt_OnSliceMesh.Invoke(top);
            }
        }
    }

    public SlicedHull SliceObject(GameObject obj, Transform _plane = null, Material crossSectionMaterial = null)
    {
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        return obj.Slice(_plane.position, _plane.up, crossSectionMaterial);
    }
}
