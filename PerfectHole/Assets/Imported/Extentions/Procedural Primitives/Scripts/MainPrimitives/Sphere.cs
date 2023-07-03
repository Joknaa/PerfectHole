using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class Sphere : PPBase
    {
        [Header("Basic parameters")]
        public float radius = 0.5f;
        [Header("Segments")]
        public int segments = 24;
        [Header("Slice")]
        public bool sliceOn = false;
        public float sliceFrom = 0.0f;
        public float sliceTo = 360.0f;
        public SphereCutOption cutTpye = SphereCutOption.None;
        public float cutFrom = 0.0f;
        public float cutTo = 1.0f;
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
            m_mesh.name = "Sphere";

            radius = Mathf.Clamp(radius, 0.00001f, 10000.0f);
            segments = Mathf.Clamp(segments, 4, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);
            cutFrom = Mathf.Clamp01(cutFrom);
            cutTo = Mathf.Clamp(cutTo, cutFrom, 1.0f);

            float cf = Mathf.PI - Mathf.Acos((cutFrom - 0.5f) * 2.0f);
            float ct = Mathf.PI - Mathf.Acos((cutTo - 0.5f) * 2.0f);
            bool hemiSphere = cutTpye == SphereCutOption.HemiSphere;
            bool sphereSector = cutTpye == SphereCutOption.SphericalSector;

            CreateSphere(pivotOffset + Vector3.zero, m_rotation * Vector3.forward, m_rotation * Vector3.right, radius, segments, segments / 2, sliceOn, sliceFrom, sliceTo, hemiSphere || sphereSector, cf, ct, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals, smooth);

            if (sliceOn)
            {
                Vector3 centerFrom = new Vector3(Mathf.Sin(sliceFrom * Mathf.Deg2Rad), 0.0f, Mathf.Cos(sliceFrom * Mathf.Deg2Rad)) * radius * 0.5f;
                Vector3 centerTo = new Vector3(Mathf.Sin(sliceTo * Mathf.Deg2Rad), 0.0f, Mathf.Cos(sliceTo * Mathf.Deg2Rad)) * radius * 0.5f;
                CreateHemiCircle(pivotOffset + Vector3.zero, m_rotation * Vector3.up, m_rotation * centerFrom.normalized, radius, segments / 2, 2, hemiSphere || sphereSector, cf, ct, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling,                             flipNormals,  sphereSector);
                CreateHemiCircle(pivotOffset + Vector3.zero, m_rotation * Vector3.up, m_rotation * centerTo.normalized,   radius, segments / 2, 2, hemiSphere || sphereSector, cf, ct, generateMappingCoords, realWorldMapSize, UVOffset, new Vector2(-UVTiling.x, UVTiling.y), !flipNormals, sphereSector);
            }

            Vector2 sincosFrom = new Vector2(Mathf.Sin(cf), -Mathf.Cos(cf));
            Vector2 sincosTo = new Vector2(Mathf.Sin(ct), -Mathf.Cos(ct));
            if (hemiSphere)
            {
                CreateCircle(pivotOffset + m_rotation * new Vector3(0.0f, sincosFrom.y * radius, 0.0f), m_rotation * Vector3.forward, m_rotation * Vector3.right, radius * sincosFrom.x, segments, 2, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals);
                CreateCircle(pivotOffset + m_rotation * new Vector3(0.0f, sincosTo.y * radius, 0.0f),   m_rotation * Vector3.forward, m_rotation * Vector3.right, radius * sincosTo.x,   segments, 2, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            }
            if (sphereSector)
            {
                float hFrom = sincosFrom.y * radius;
                float hTo = sincosTo.y * radius;
                if (cutFrom > 0.5f) CreateCone(pivotOffset + m_rotation * new Vector3(0.0f, hFrom * 0.5f, 0.0f), m_rotation * Vector3.forward, m_rotation * Vector3.right, hFrom,  0.0f,                  radius * sincosFrom.x, segments,     segments / 4, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals,  smooth);
                else                CreateCone(pivotOffset + m_rotation * new Vector3(0.0f, hFrom * 0.5f, 0.0f), m_rotation * Vector3.forward, m_rotation * Vector3.right, -hFrom, radius * sincosFrom.x, 0.0f, segments,        segments / 4, sliceOn,               sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals, smooth);
                if (cutTo > 0.5f)   CreateCone(pivotOffset + m_rotation * new Vector3(0.0f, hTo * 0.5f, 0.0f),   m_rotation * Vector3.forward, m_rotation * Vector3.right, hTo,    0.0f,                  radius * sincosTo.x,   segments,     segments / 4, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals, smooth);
                else                CreateCone(pivotOffset + m_rotation * new Vector3(0.0f, hTo * 0.5f, 0.0f),   m_rotation * Vector3.forward, m_rotation * Vector3.right, -hTo,   radius * sincosTo.x,   0.0f, segments,        segments / 4, sliceOn,               sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals,  smooth);
            }
        }
    }
}