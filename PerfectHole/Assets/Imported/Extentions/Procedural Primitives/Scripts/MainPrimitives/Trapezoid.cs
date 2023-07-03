using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Trapezoid : PPBase
    {
        [Header("Basic parameters")]
        public float width1 = 2;
        public float width2 = 1;
        public float length = 2;
        public float offset = 0;
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
            m_mesh.name = "Trapezoid";

            length = Mathf.Clamp(length, 0.00001f, 10000.0f);
            width1 = Mathf.Clamp(width1, 0.00001f, 10000.0f);
            width2 = Mathf.Clamp(width2, 0.00001f, 10000.0f);
            offset = Mathf.Clamp(offset, -10000.0f, 10000.0f);
            lengthSegs = Mathf.Clamp(lengthSegs, 1, 100);
            widthSegs = Mathf.Clamp(widthSegs, 1, 100);

            CreateTrapezoid(pivotOffset + Vector3.zero, m_rotation * Vector3.forward, m_rotation * Vector3.right, width1, width2, length, offset, widthSegs, lengthSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
        }
    }
}