using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Torus : PPBase
    {
        [Header("Basic parameters")]
        public float radius1 = 0.5f;
        public float radius2 = 0.1f;
        [Header("Segments")]
        public int sides = 24;
        public int segments = 12;
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
            m_mesh.name = "Torus";

            radius1 = Mathf.Clamp(radius1, 0.0f, 10000.0f);
            radius2 = Mathf.Clamp(radius2, 0.0f, 10000.0f);
            sides = Mathf.Clamp(sides, 3, 100);
            segments = Mathf.Clamp(segments, 3, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);

            CreateTorus(pivotOffset + Vector3.zero, m_rotation * Vector3.forward, m_rotation * Vector3.right, radius1, radius2, sides, segments, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals, smooth, 0.0f, 360.0f);

            if (sliceOn)
            {
                Vector3 centerFrom = new Vector3(Mathf.Sin(sliceFrom * Mathf.Deg2Rad), 0.0f, Mathf.Cos(sliceFrom * Mathf.Deg2Rad)) * radius1;
                Vector3 centerTo = new Vector3(Mathf.Sin(sliceTo * Mathf.Deg2Rad), 0.0f, Mathf.Cos(sliceTo * Mathf.Deg2Rad)) * radius1;
                CreateCircle(pivotOffset + m_rotation * centerFrom, m_rotation * -centerFrom.normalized, m_rotation * Vector3.down, radius2, segments, 2, false, 0.0f, 360.0f, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
                CreateCircle(pivotOffset + m_rotation * centerTo,   m_rotation * -centerTo.normalized,   m_rotation * Vector3.up,   radius2, segments, 2, false, 0.0f, 360.0f, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            }
        }
    }
}