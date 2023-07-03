using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Cylinder : PPBase
    {
        [Header("Basic parameters")]
        public float radius = 0.5f;
        public float height = 1.0f;
        [Header("Segments")]
        public int sides = 20;
        public int capSegs = 2;
        public int heightSegs = 2;
        [Header("Slice")]
        public bool sliceOn = false;
        public float sliceFrom = 0.0f;
        public float sliceTo = 360.0f;
        [Header("Mapping Coordinates")]
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public Vector2 UVOffset = new Vector2(0.0f, 0.0f);
        public Vector2 UVTiling = new Vector2(1.0f, 1.0f);
        [Header("Others")]
        public bool flipNormals = false;
        public bool smooth = true;

        protected override void CreateMesh()
        {
            m_mesh.name = "Cylinder";

            radius = Mathf.Clamp(radius, 0.00001f, 10000.0f);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            sides = Mathf.Clamp(sides, 3, 100);
            capSegs = Mathf.Clamp(capSegs, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);

            float heightHalf = height * 0.5f;

            CreateCylinder(pivotOffset + Vector3.zero, m_rotation * Vector3.forward, m_rotation * Vector3.right, height, radius, sides, heightSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals, smooth);
            CreateCircle(pivotOffset + m_rotation * new Vector3(0.0f, heightHalf, 0.0f),  m_rotation * Vector3.forward, m_rotation * Vector3.right, radius, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreateCircle(pivotOffset + m_rotation * new Vector3(0.0f, -heightHalf, 0.0f), m_rotation * Vector3.forward, m_rotation * Vector3.right, radius, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals);

            if (sliceOn)
            {
                Vector3 centerFrom = new Vector3(Mathf.Sin(sliceFrom * Mathf.Deg2Rad), 0.0f, Mathf.Cos(sliceFrom * Mathf.Deg2Rad)) * radius * 0.5f;
                Vector3 centerTo = new Vector3(Mathf.Sin(sliceTo * Mathf.Deg2Rad), 0.0f, Mathf.Cos(sliceTo * Mathf.Deg2Rad)) * radius * 0.5f;

                Vector2 tilingCenter = realWorldMapSize ? new Vector2(1.0f, 1.0f) : new Vector2(0.5f, 1.0f);
                tilingCenter = new Vector2(UVTiling.x * tilingCenter.x, UVTiling.y * tilingCenter.y);

                CreatePlane(pivotOffset + m_rotation * centerFrom, m_rotation * Vector3.up, m_rotation * -centerFrom.normalized, radius, height, capSegs, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset,                           tilingCenter, flipNormals);
                CreatePlane(pivotOffset + m_rotation * centerTo,   m_rotation * Vector3.up, m_rotation * centerTo.normalized,    radius, height, capSegs, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(0.5f, 0.0f), tilingCenter, flipNormals);
            }
        }
    }
}