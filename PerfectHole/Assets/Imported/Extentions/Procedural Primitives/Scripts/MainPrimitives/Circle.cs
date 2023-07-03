using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Circle : PPBase
    {
        [Header("Basic parameters")]
        public float radius = 1;
        [Header("Segments")]
        public int sides = 20;
        public int segments = 5;
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

        protected override void CreateMesh()
        {
            m_mesh.name = "Circle";

            radius = Mathf.Clamp(radius, 0.00001f, 10000.0f);
            segments = Mathf.Clamp(segments, 1, 100);
            sides = Mathf.Clamp(sides, 3, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);

            CreateCircle(pivotOffset + Vector3.zero, m_rotation * Vector3.forward, m_rotation * Vector3.right, radius, sides, segments, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
        }
    }
}