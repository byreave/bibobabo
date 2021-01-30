using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassMgrComp : MonoBehaviour
{
    [SerializeField, Tooltip("相机跟随目标")]
    private float _oriMass = 1f;

    private float _curMass = 1f;
    private float _oriVolume;
    private float _curVolume;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        EventManager EM = EventManager.Get(gameObject);
        if (EM != null)
        {
            EM.Evt_InitVolume += InitVolume;
            EM.Evt_OnSwitchSliceMesh += OnSliceMesh;
        }
    }

    void Start()
    {
        
    }
    public void InitVolume(GameObject BodyObj)
    {
        _oriVolume = CalcMeshVolume(BodyObj);
        Debug.Log("Init Volume = " + Mathf.Abs(_oriVolume));
        SycnMassByVolume();
    }

    void Update()
    {
        
    }

    private void OnSliceMesh(GameObject newMesh)
    {
        CalcMeshVolume(newMesh);
        SycnMassByVolume();
    }

    private float CalcMeshVolume(GameObject newMesh)
    {
        MeshFilter meshFilter = newMesh.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Vector3[] arrVertices = meshFilter.mesh.vertices;
            int[] arrTriangles = meshFilter.mesh.triangles;
            float sum = 0.0f;

            Vector3 MeshScale = newMesh.transform.localScale;

            for (int i = 0; i < meshFilter.mesh.subMeshCount; i++)
            {
                int[] arrIndices = meshFilter.mesh.GetTriangles(i);
                for (int j = 0; j < arrIndices.Length; j += 3)
                    sum += CalcVolume(arrVertices[arrIndices[j]], arrVertices[arrIndices[j + 1]], arrVertices[arrIndices[j + 2]], MeshScale);
            }


            Debug.Log("Volume = " + Mathf.Abs(sum));
            return sum;
        }
        return -1f;
    }
    private float CalcVolume(Vector3 pt0, Vector3 pt1, Vector3 pt2, Vector3 scale)
    {
        pt0 = new Vector3(pt0.x * scale.x, pt0.y * scale.y, pt0.z * scale.z);
        pt1 = new Vector3(pt1.x * scale.x, pt1.y * scale.y, pt1.z * scale.z);
        pt2 = new Vector3(pt2.x * scale.x, pt2.y * scale.y, pt2.z * scale.z);
        float v321 = pt2.x * pt1.y * pt0.z;
        float v231 = pt1.x * pt2.y * pt0.z;
        float v312 = pt2.x * pt0.y * pt1.z;
        float v132 = pt0.x * pt2.y * pt1.z;
        float v213 = pt1.x * pt0.y * pt2.z;
        float v123 = pt0.x * pt1.y * pt2.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    private void SycnMassByVolume()
    {
        _curMass = (_curVolume / _oriVolume) * _oriMass;
    }
}
