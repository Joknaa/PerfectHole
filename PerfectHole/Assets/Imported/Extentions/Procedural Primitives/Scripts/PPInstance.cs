using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    [ExecuteInEditMode]
    public class PPInstance : MonoBehaviour
    {
        public PPBase m_source = null;

        private void Start()
        {
            ApplySource();
        }

        private void OnValidate()
        {
            ApplySource();
        }

        public void ApplySource()
        {
            if (m_source == null || GetComponent<MeshFilter>() == null) return;
            GetComponent<MeshFilter>().sharedMesh = m_source.mesh;
        }

        public void ApplySource(PPBase source)
        {
            if (source ==null || GetComponent<MeshFilter>() == null) return;
            m_source = source;
            GetComponent<MeshFilter>().sharedMesh = m_source.mesh;
        }
    }
}