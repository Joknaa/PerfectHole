using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Arrow : PPBase
    {
        [Header("Basic parameters")]
        public float width1 = 0.5f;
        public float width2 = 0.3f;
        public float width3 = 1.0f;
        public float length1 = 1.0f;
        public float length2 = 0.5f;
        public float height = 0.5f;
        [Header("Segments")]
        public int widthSegs1 = 2;
        public int lengthSegs1 = 2;
        public int widthSegs2 = 4;
        public int lengthSegs2 = 2;
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
            m_mesh.name = "Arrow";

            width1 = Mathf.Clamp(width1, 0.00001f, 10000.0f);
            width2 = Mathf.Clamp(width2, 0.00001f, 10000.0f);
            width3 = Mathf.Clamp(width3, width2, 10000.0f);
            length1 = Mathf.Clamp(length1, 0.00001f, 10000.0f);
            length2 = Mathf.Clamp(length2, 0.00001f, 10000.0f);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            lengthSegs1 = Mathf.Clamp(lengthSegs1, 1, 100);
            widthSegs1 = Mathf.Clamp(widthSegs1, 1, 100);
            lengthSegs2 = Mathf.Clamp(lengthSegs2, 1, 100);
            widthSegs2 = Mathf.Clamp(widthSegs2, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);

            float widthHalf1 = width1 * 0.5f;
            float widthHalf2 = width2 * 0.5f;
            float widthHalf3 = width3 * 0.5f;
            float heightHalf = height * 0.5f;
            float lengthHalf1 = length1 * 0.5f;
            float lengthHalf2 = length2 * 0.5f;

            //head
            Vector3 p0 = new Vector3(-widthHalf3, 0.0f, lengthHalf1 - lengthHalf2);
            Vector3 p1 = new Vector3(0.0f, 0.0f, lengthHalf1 + lengthHalf2);
            Vector3 p2 = new Vector3(widthHalf3, 0.0f, lengthHalf1 - lengthHalf2);
            Vector3 vLeft = p0 - p1;
            Vector3 vRight = p1 - p2;
            CreateTriangle(pivotOffset + m_rotation * new Vector3(0.0f, heightHalf, lengthHalf1),  m_rotation * Vector3.forward, m_rotation * Vector3.right, width3, length2, 0.0f, widthSegs2, lengthSegs2, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreateTriangle(pivotOffset + m_rotation * new Vector3(0.0f, -heightHalf, lengthHalf1), m_rotation * Vector3.forward, m_rotation * Vector3.right, width3, length2, 0.0f, widthSegs2, lengthSegs2, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals);
            CreatePlane(pivotOffset + m_rotation * new Vector3(0.0f, 0.0f, lengthHalf1 - lengthHalf2), m_rotation * Vector3.up, m_rotation * Vector3.right, width3, height, widthSegs2, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreatePlane(pivotOffset + m_rotation * (p0 + p1) * 0.5f, m_rotation * Vector3.up, m_rotation * vLeft.normalized,  vLeft.magnitude,  height, lengthSegs2, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreatePlane(pivotOffset + m_rotation * (p1 + p2) * 0.5f, m_rotation * Vector3.up, m_rotation * vRight.normalized, vRight.magnitude, height, lengthSegs2, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);

            //body
            Vector3 pp0 = new Vector3(-widthHalf1, 0.0f, -lengthHalf2 - lengthHalf1);
            Vector3 pp1 = new Vector3(-widthHalf2, 0.0f, -lengthHalf2 + lengthHalf1);
            Vector3 pp2 = new Vector3(widthHalf2, 0.0f, -lengthHalf2 + lengthHalf1);
            Vector3 pp3 = new Vector3(widthHalf1, 0.0f, -lengthHalf2 - lengthHalf1);
            Vector3 vvLeft = pp0 - pp1;
            Vector3 vvRight = pp2 - pp3;
            CreateTrapezoid(pivotOffset + m_rotation * new Vector3(0.0f, heightHalf, -lengthHalf2),  m_rotation * Vector3.forward, m_rotation * Vector3.right, width1, width2, length1, 0.0f, widthSegs1, lengthSegs1, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreateTrapezoid(pivotOffset + m_rotation * new Vector3(0.0f, -heightHalf, -lengthHalf2), m_rotation * Vector3.forward, m_rotation * Vector3.right, width1, width2, length1, 0.0f, widthSegs1, lengthSegs1, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals);
            CreatePlane(pivotOffset + m_rotation * new Vector3(0.0f, 0.0f, -lengthHalf2 - lengthHalf1), m_rotation * Vector3.up, m_rotation * Vector3.right, width1, height, widthSegs1, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreatePlane(pivotOffset + m_rotation * (pp0 + pp1) * 0.5f, m_rotation * Vector3.up, m_rotation * vvLeft.normalized,  vvLeft.magnitude,  height, lengthSegs1, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreatePlane(pivotOffset + m_rotation * (pp2 + pp3) * 0.5f, m_rotation * Vector3.up, m_rotation * vvRight.normalized, vvRight.magnitude, height, lengthSegs1, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
        }
    }
}