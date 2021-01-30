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

    [SerializeField]
    [Tooltip("切割提示面")]
    private GameObject _sliceTip = null;

    private bool InSliceMode = false;
    private List<Vector3> PointList = new List<Vector3>();

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
        ////Test
        //if (over) return;
        //if (count < 600)
        //{
        //    Debug.Log(count);
        //    count++;
        //}
        //else
        //{
        //    over = true;
        //    Slice();
        //}
        SliceTick();
        OnMouseClickInSliceMode();

    }

    void SliceTick()
    {
        //if(!InSliceMode)
        //{
        //    // todo : hidden UI

        //    return;
        //}

        //DrawLineUpdate
        switch(PointList.Count)
        {
            case 0:
                {
                    return;
                }
            case 1:
                {
                    // draw slice ui
                    Vector3 LineStart = PointList[0];
                    Vector3 LineEnd = GetMouseWorldPos();

                    Debug.DrawLine(LineStart, LineEnd, Color.red);

                    if(!_sliceTip.active)
                    {
                        _sliceTip.SetActive(true);
                    }
                    //_sliceTip.transform.position = new Vector3(LineStart.x, LineStart.y, 0);
                    //_sliceTip.transform.LookAt(new Vector3(LineEnd.x, LineEnd.y, 0));

                    Vector3 Dir = LineEnd - LineStart;
                    float Angle = Vector3.Angle(Dir, new Vector3(1f, 0f, 0f));
                    Angle = (Dir.y < 0) ? 360 - Angle : Angle;
                    Debug.Log(Angle);
                    _sliceTip.transform.position = new Vector3(LineStart.x, LineStart.y, 0);
                    _sliceTip.transform.rotation = new Quaternion();

                    _sliceTip.transform.RotateAround(_sliceTip.transform.position, new Vector3(0f, 0f, 1f), Angle);
                    break;
                }
            case 2:
                {
                    // do slice here
                    SliceByMouseClick(PointList[0], PointList[1]);
                    PointList.Clear();
                    _sliceTip.SetActive(false);
                    break;
                }
            default:return;
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(ray, out hitInfo, 30f, 1 << LayerMask.NameToLayer("MouseClick"));// layerMask = 8 : MouseClick
        //Debug.DrawLine(ray.origin, hitInfo.point, Color.green, 10);//DrawRay(ray.origin, ray.direction, Color.green, 10);
        if (hit)
        {
            Debug.Log("Hit: " + hitInfo.collider.gameObject.name);
            return hitInfo.point;
        }
        else return Vector3.zero;
    }

    void OnSwitchSliceMode()
    {
        InSliceMode = !InSliceMode;

        PointList.Clear();
    }

    void OnMouseClickInSliceMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed left click.");
            Vector3 MousePos = GetMouseWorldPos();
            if (MousePos.z != 0)
            {
                PointList.Add(MousePos);
            }
        }
    }

    void SliceByMouseClick(Vector3 LineStart, Vector3 LineEnd)
    {
        Vector3 DrawLineDir = LineEnd - LineStart;
        Vector3 PlaneNormal = Quaternion.AngleAxis(90f, new Vector3(0, 0, 1)) * DrawLineDir;

        Slice_V2(LineStart, PlaneNormal, _sliceTip.transform.up.z < 0);
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

    public void Slice_V2(Vector3 pointInPlane, Vector3 pointNormal, bool useTop)
    {
        SlicedHull hull = SliceObject(_sliceMesh, pointInPlane, pointNormal, null);
        if (hull != null)
        {
            GameObject bottom = hull.CreateLowerHull(_sliceMesh, null);
            GameObject top = hull.CreateUpperHull(_sliceMesh, null);

            Destroy(_sliceMesh.gameObject);

            if (useTop)
            {
                _sliceMesh = top;
                AddHullComponents(bottom, true);
                AddHullComponents(top);
            }
            else
            {
                _sliceMesh = bottom;
                AddHullComponents(bottom);
                AddHullComponents(top, true);
            }


            EventManager EM = EventManager.Get(gameObject);
            if (EM != null)
            {
                EM.Evt_OnSliceMesh.Invoke(top);
            }
        }
    }

    public SlicedHull SliceObject(GameObject obj, Vector3 pointInPlane, Vector3 pointNormal, Material crossSectionMaterial = null)
    {
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        return obj.Slice(pointInPlane, pointNormal, crossSectionMaterial);
    }
    public void AddHullComponents(GameObject obj, bool AddDelayDestroy = false)
    {
        obj.layer = 9;
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ| RigidbodyConstraints.FreezeRotationY| RigidbodyConstraints.FreezeRotationX;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = obj.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(100, obj.transform.position, 20);

        if(AddDelayDestroy)
        {
            obj.AddComponent<DelayDestroy>();
        }
    }
}
