using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Tube : PPBase
    {
        [Header("Basic parameters")]
        public float radius1 = 0.5f;
        public float radius2 = 0.3f;
        public float height = 1.0f;
        public bool cap1 = false;
        public float capThickness1 = 0.2f;
        public bool cap2 = false;
        public float capThickness2 = 0.2f;
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
            m_mesh.name = "Tube";

            radius1 = Mathf.Clamp(radius1, 0.00001f, 10000.0f);
            radius2 = Mathf.Clamp(radius2, 0.00001f, radius1);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            if (cap1)
            {
                capThickness1 = Mathf.Clamp(capThickness1, 0.00001f, height);
                capThickness2 = Mathf.Clamp(capThickness2, 0.00001f, height - capThickness1);
            }
            else
            {
                capThickness1 = Mathf.Clamp(capThickness1, 0.00001f, height);
                capThickness2 = Mathf.Clamp(capThickness2, 0.00001f, height);
            }
            sides = Mathf.Clamp(sides, 3, 100);
            capSegs = Mathf.Clamp(capSegs, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);

            float heightHalf = height * 0.5f;
            float hDown = cap1 ? capThickness1 : 0.0f;
            float hUp = cap2 ? capThickness2 : 0.0f;
            float dif = radius1 - radius2;

            Vector3 centerDown = new Vector3(0.0f, -heightHalf, 0.0f);
            Vector3 centerUp = new Vector3(0.0f, heightHalf, 0.0f);
            Vector3 centerDown2 = new Vector3(0.0f, -heightHalf + hDown, 0.0f);
            Vector3 centerUp2 = new Vector3(0.0f, heightHalf - hUp, 0.0f);

            CreateCylinder(pivotOffset + Vector3.zero, m_rotation * Vector3.forward, m_rotation * Vector3.right, height, radius1, sides, heightSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals, smooth);
            CreateCylinder(pivotOffset + m_rotation * new Vector3(0.0f, (hDown - hUp) * 0.5f, 0.0f), m_rotation * Vector3.forward, m_rotation * Vector3.right, height - hUp - hDown, radius2, sides, heightSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals, smooth);
            if (cap1)
            {
                CreateCircle(pivotOffset + m_rotation * centerDown,  m_rotation * Vector3.forward, m_rotation * Vector3.right, radius1, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals);
                CreateCircle(pivotOffset + m_rotation * centerDown2, m_rotation * Vector3.forward, m_rotation * Vector3.right, radius2, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            }
            else
            {
                CreateRing(pivotOffset + m_rotation * centerDown, m_rotation * Vector3.forward, m_rotation * Vector3.right, radius1, radius2, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals);
            }
            if (cap2)
            {
                CreateCircle(pivotOffset + m_rotation * centerUp,  m_rotation * Vector3.forward, m_rotation * Vector3.right, radius1, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
                CreateCircle(pivotOffset + m_rotation * centerUp2, m_rotation * Vector3.forward, m_rotation * Vector3.right, radius2, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals);
            }
            else
            {
                CreateRing(pivotOffset + m_rotation * centerUp, m_rotation * Vector3.forward, m_rotation * Vector3.right, radius1, radius2, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            }
            if (sliceOn)
            {

                Vector3 centerFrom = new Vector3(Mathf.Sin(sliceFrom * Mathf.Deg2Rad), 0.0f, Mathf.Cos(sliceFrom * Mathf.Deg2Rad));
                Vector3 centerTo = new Vector3(Mathf.Sin(sliceTo * Mathf.Deg2Rad), 0.0f, Mathf.Cos(sliceTo * Mathf.Deg2Rad));

                Vector2 tiling = realWorldMapSize ? new Vector2(1.0f, 1.0f) : new Vector2(0.5f, 1.0f);
                tiling = new Vector2(UVTiling.x * tiling.x, UVTiling.y * tiling.y);

                CreatePlane(pivotOffset + m_rotation * centerFrom * (radius2 + dif * 0.5f), m_rotation * Vector3.up, m_rotation * -centerFrom.normalized, dif, height, capSegs, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset,                           tiling, flipNormals);
                CreatePlane(pivotOffset + m_rotation * centerTo * (radius2 + dif * 0.5f),   m_rotation * Vector3.up, m_rotation * centerTo.normalized,    dif, height, capSegs, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(0.5f, 0.0f), tiling, flipNormals);
                if (cap1)
                {
                    CreatePlane(pivotOffset + m_rotation * new Vector3(centerFrom.x * radius2 * 0.5f, -heightHalf + capThickness1 * 0.5f, centerFrom.z * radius2 * 0.5f), m_rotation * Vector3.up, m_rotation * -centerFrom.normalized, radius2, capThickness1, capSegs, 2, generateMappingCoords, realWorldMapSize, UVOffset,                           tiling, flipNormals);
                    CreatePlane(pivotOffset + m_rotation * new Vector3(centerTo.x * radius2 * 0.5f, -heightHalf + capThickness1 * 0.5f, centerTo.z * radius2 * 0.5f),     m_rotation * Vector3.up, m_rotation * centerTo.normalized,    radius2, capThickness1, capSegs, 2, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(0.5f, 0.0f), tiling, flipNormals);
                }
                if (cap2)
                {
                    CreatePlane(pivotOffset + m_rotation * new Vector3(centerFrom.x * radius2 * 0.5f, heightHalf - capThickness2 * 0.5f, centerFrom.z * radius2 * 0.5f), m_rotation * Vector3.up, m_rotation * -centerFrom.normalized, radius2, capThickness2, capSegs, 2, generateMappingCoords, realWorldMapSize, UVOffset,                           tiling, flipNormals);
                    CreatePlane(pivotOffset + m_rotation * new Vector3(centerTo.x * radius2 * 0.5f, heightHalf - capThickness2 * 0.5f, centerTo.z * radius2 * 0.5f),     m_rotation * Vector3.up, m_rotation * centerTo.normalized,    radius2, capThickness2, capSegs, 2, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(0.5f, 0.0f), tiling, flipNormals);
                }
            }
        }
    }
}