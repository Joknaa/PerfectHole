using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Plane : PPBase
    {
        [Header("Basic parameters")]
        public float width = 2;
        public float length = 2;
        [Header("Segments")]
        public int widthSegs = 10;
        public int lengthSegs = 10;
        [Header("Mapping Coordinates")]
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public Vector2 UVOffset = new Vector2(0.0f, 0.0f);
        public Vector2 UVTiling = new Vector2(1.0f, 1.0f);
        [Header("Others")]
        public bool flipNormals = false;

        protected override void CreateMesh()
        {
            m_mesh.name = "Plane";

            length = Mathf.Clamp(length, 0.00001f, 10000.0f);
            width = Mathf.Clamp(width, 0.00001f, 10000.0f);
            lengthSegs = Mathf.Clamp(lengthSegs, 1, 100);
            widthSegs = Mathf.Clamp(widthSegs, 1, 100);
            
            CreatePlane(pivotOffset + Vector3.zero, m_rotation * Vector3.forward, m_rotation * Vector3.right, width, length, widthSegs, lengthSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
        }
    }
}