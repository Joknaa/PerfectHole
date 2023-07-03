using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Prism : PPBase
    {
        [Header("Basic parameters")]
        public float width = 1;
        public float length = 1;
        public float height = 1.0f;
        public float offset = 0;
        [Header("Segments")]
        public int sideSegs = 2;
        public int heightSegs = 2;
        [Header("Mapping Coordinates")]
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public Vector2 UVOffset = new Vector2(0.0f, 0.0f);
        public Vector2 UVTiling = new Vector2(1.0f, 1.0f);
        [Header("Others")]
        public bool flipNormals = false;

        protected override void CreateMesh()
        {
            m_mesh.name = "Prism";

            width = Mathf.Clamp(width, 0.00001f, 10000.0f);
            length = Mathf.Clamp(length, 0.00001f, 10000.0f);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            offset = Mathf.Clamp(offset, -10000.0f, 10000.0f);
            sideSegs = Mathf.Clamp(sideSegs, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);

            float lengthHalf = length * 0.5f;
            float widthHalf = width * 0.5f;
            float heightHalf = height * 0.5f;

            CreateTriangle(pivotOffset + m_rotation * new Vector3(0.0f, heightHalf, 0.0f),  m_rotation * Vector3.forward, m_rotation * Vector3.right, width, length, offset, sideSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreateTriangle(pivotOffset + m_rotation * new Vector3(0.0f, -heightHalf, 0.0f), m_rotation * Vector3.forward, m_rotation * Vector3.right, width, length, offset, sideSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals);

            Vector3 p0 = new Vector3(-widthHalf, 0.0f, -lengthHalf);
            Vector3 p1 = new Vector3(offset, 0.0f, lengthHalf);
            Vector3 p2 = new Vector3(widthHalf, 0.0f, -lengthHalf);
            Vector3 vLeft = p0 - p1;
            Vector3 vRight = p1 - p2;
            CreatePlane(pivotOffset + m_rotation * new Vector3(0.0f, 0.0f, -lengthHalf), m_rotation * Vector3.up, m_rotation * Vector3.right, width, height, sideSegs, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreatePlane(pivotOffset + m_rotation * (p0 + p1) * 0.5f, m_rotation * Vector3.up, m_rotation * vLeft.normalized,  vLeft.magnitude,  height, sideSegs, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreatePlane(pivotOffset + m_rotation * (p1 + p2) * 0.5f, m_rotation * Vector3.up, m_rotation * vRight.normalized, vRight.magnitude, height, sideSegs, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
        }
    }
}