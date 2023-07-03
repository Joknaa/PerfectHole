using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Box : PPBase
    {
        [Header("Basic parameters")]
        public float width = 1.0f;
        public float length = 1.0f;
        public float height = 1.0f;
        [Header("Segments")]
        public int widthSegs = 2;
        public int lengthSegs = 2;
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
            m_mesh.name = "Box";

            length = Mathf.Clamp(length, 0.00001f, 10000.0f);
            width = Mathf.Clamp(width, 0.00001f, 10000.0f);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            lengthSegs = Mathf.Clamp(lengthSegs, 1, 100);
            widthSegs = Mathf.Clamp(widthSegs, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);

            float lengthHalf = length * 0.5f;
            float widthHalf = width * 0.5f;
            float heightHalf = height * 0.5f;
            
            CreatePlane(pivotOffset + m_rotation * new Vector3(0.0f, 0.0f, -lengthHalf), m_rotation * Vector3.up,      m_rotation * Vector3.right,   width,  height, widthSegs,  heightSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreatePlane(pivotOffset + m_rotation * new Vector3(0.0f, 0.0f, lengthHalf),  m_rotation * Vector3.up,      m_rotation * Vector3.left,    width,  height, widthSegs,  heightSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreatePlane(pivotOffset + m_rotation * new Vector3(-widthHalf, 0.0f, 0.0f),  m_rotation * Vector3.up,      m_rotation * Vector3.back,    length, height, lengthSegs, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreatePlane(pivotOffset + m_rotation * new Vector3(widthHalf, 0.0f, 0.0f),   m_rotation * Vector3.up,      m_rotation * Vector3.forward, length, height, lengthSegs, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreatePlane(pivotOffset + m_rotation * new Vector3(0.0f, heightHalf, 0.0f),  m_rotation * Vector3.forward, m_rotation * Vector3.right,   width,  length, widthSegs,  lengthSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreatePlane(pivotOffset + m_rotation * new Vector3(0.0f, -heightHalf, 0.0f), m_rotation * Vector3.forward, m_rotation * Vector3.left,    width,  length, widthSegs,  lengthSegs, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
        }
    }
}