using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class PPCombiner : PPBase
    {
        public List<GameObject> elements = new List<GameObject>();
        public bool includeChildren = false;
        public bool applyTargetsTransform = true;

        List<Mesh> mList;

        protected override void CreateMesh()
        {
            m_mesh.name = "Combined Mesh";

            mList = new List<Mesh>();
            foreach (GameObject go in elements)
            {
                if (go == gameObject) continue;
                if (includeChildren)
                {
                    var mfs = go.GetComponentsInChildren<MeshFilter>();
                    foreach (var mf in mfs)
                    {
                        if (mf.sharedMesh == null) continue;
                        if (applyTargetsTransform) AddMesh(mf.sharedMesh, mf.transform);
                        else AddMesh(mf.sharedMesh);
                    }
                }
                else
                {
                    MeshFilter mf = go.GetComponent<MeshFilter>();
                    if (mf == null || mf.sharedMesh == null) continue;
                    AddMesh(mf.sharedMesh);
                }
            }
            mList = null;
        }

        void AddMesh(Mesh m, Transform trans = null)
        {
            if (mList.Contains(m)) return;
            mList.Add(m);

            Vector3[] vertices = m.vertices;
            Vector3[] normals = m.normals;
            int[] triangles = m.triangles;

            if (m_rotation != Quaternion.identity)
            {
                for (int i = 0; i < vertices.Length; ++i)
                {
                    vertices[i] = m_rotation * vertices[i];
                }
                for (int i = 0; i < normals.Length; ++i)
                {
                    normals[i] = m_rotation * normals[i];
                }
            }

            if (pivotOffset != Vector3.zero)
            {
                for (int i = 0; i < vertices.Length; ++i)
                {
                    vertices[i] += pivotOffset;
                }
            }

            if (trans != null)
            {
                for (int i = 0; i < vertices.Length; ++i)
                {
                    vertices[i] = trans.TransformVector(vertices[i]) + trans.localPosition;
                }
                for (int i = 0; i < normals.Length; ++i)
                {
                    normals[i] = trans.worldToLocalMatrix.transpose * normals[i];
                }
            }

            for (int i = 0; i < triangles.Length; ++i)
            {
                triangles[i] += m_vertices.Count;
            }

            m_vertices.AddRange(vertices);
            m_normals.AddRange(normals);
            m_triangles.AddRange(triangles);
            m_uv.AddRange(m.uv);
        }
    }
}