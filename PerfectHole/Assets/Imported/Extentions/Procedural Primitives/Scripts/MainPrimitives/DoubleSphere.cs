using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class DoubleSphere : PPBase
    {
        [Header("Basic parameters")]
        public float radius1 = 0.5f;
        public float radius2 = 0.3f;
        [Header("Segments")]
        public int segments = 24;
        [Header("Slice")]
        public bool sliceOn = true;
        public float sliceFrom = 90.0f;
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
            m_mesh.name = "DoubleSphere";

            radius1 = Mathf.Clamp(radius1, 0.00001f, 10000.0f);
            radius2 = Mathf.Clamp(radius2, 0.00001f, radius1);
            segments = Mathf.Clamp(segments, 4, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);
            cutFrom = Mathf.Clamp01(cutFrom);
            cutTo = Mathf.Clamp(cutTo, cutFrom, 1.0f);
            
            float min = (radius1 - radius2) / (radius1 * 2);
            float max = (radius1 + radius2) / (radius1 * 2);
            float cutFrom2 = Mathf.Clamp01((cutFrom - min) / (max - min));
            float cutTo2 = Mathf.Clamp01((cutTo - min) / (max - min));
            float cf = Mathf.PI - Mathf.Acos((cutFrom - 0.5f) * 2.0f);
            float ct = Mathf.PI - Mathf.Acos((cutTo - 0.5f) * 2.0f);
            float cf2 = Mathf.PI - Mathf.Acos((cutFrom2 - 0.5f) * 2.0f);
            float ct2 = Mathf.PI - Mathf.Acos((cutTo2 - 0.5f) * 2.0f);
            bool hemiSphere = cutTpye == SphereCutOption.HemiSphere;
            bool sphereSector = cutTpye == SphereCutOption.SphericalSector;

            CreateSphere(pivotOffset + Vector3.zero, m_rotation * Vector3.forward, m_rotation * Vector3.right, radius1, segments, segments / 2, sliceOn, sliceFrom, sliceTo, hemiSphere || sphereSector, cf, ct, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals, smooth);
            if (sphereSector) CreateSphere(pivotOffset + Vector3.zero, m_rotation * Vector3.forward, m_rotation * Vector3.right, radius2, segments, segments / 2, sliceOn, sliceFrom, sliceTo, sphereSector, cf,  ct,  generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals, smooth);
            else              CreateSphere(pivotOffset + Vector3.zero, m_rotation * Vector3.forward, m_rotation * Vector3.right, radius2, segments, segments / 2, sliceOn, sliceFrom, sliceTo, hemiSphere,   cf2, ct2, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals, smooth);

            if (sliceOn)
            {
                Vector3 centerFrom = new Vector3(Mathf.Sin(sliceFrom * Mathf.Deg2Rad), 0.0f, Mathf.Cos(sliceFrom * Mathf.Deg2Rad)) * radius1 * 0.5f;
                Vector3 centerTo = new Vector3(Mathf.Sin(sliceTo * Mathf.Deg2Rad), 0.0f, Mathf.Cos(sliceTo * Mathf.Deg2Rad)) * radius1 * 0.5f;
                CreateHemiRing(pivotOffset + Vector3.zero, m_rotation * Vector3.up, m_rotation * centerFrom.normalized, radius1, radius2, segments / 2, 1, hemiSphere || sphereSector, cf, ct, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling,                              flipNormals,  sphereSector);
                CreateHemiRing(pivotOffset + Vector3.zero, m_rotation * Vector3.up, m_rotation * centerTo.normalized,   radius1, radius2, segments / 2, 1, hemiSphere || sphereSector, cf, ct, generateMappingCoords, realWorldMapSize, UVOffset, new Vector2(-UVTiling .x, UVTiling.y), !flipNormals, sphereSector);
            }

            Vector2 sincosFrom = new Vector2(Mathf.Sin(cf), -Mathf.Cos(cf));
            Vector2 sincosTo = new Vector2(Mathf.Sin(ct), -Mathf.Cos(ct));

            if (hemiSphere)
            {
                Vector2 sincosFrom2 = new Vector2(Mathf.Sin(cf2), -Mathf.Cos(cf2));
                Vector2 sincosTo2 = new Vector2(Mathf.Sin(ct2), -Mathf.Cos(ct2));
                CreateRing(pivotOffset + m_rotation * new Vector3(0.0f, sincosFrom.y * radius1, 0.0f), m_rotation * Vector3.forward, m_rotation * Vector3.right, radius1 * sincosFrom.x, radius2 * sincosFrom2.x, segments, 1, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals);
                CreateRing(pivotOffset + m_rotation * new Vector3(0.0f, sincosTo.y * radius1, 0.0f),   m_rotation * Vector3.forward, m_rotation * Vector3.right, radius1 * sincosTo.x,   radius2 * sincosTo2.x,   segments, 1, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            }

            if (sphereSector)
            {
                float hFrom = sincosFrom.y * (radius1 - radius2);
                float hTo = sincosTo.y * (radius1 - radius2);
                if (cutFrom > 0.5f) CreateCone(pivotOffset + m_rotation * new Vector3(0.0f, sincosFrom.y * radius2 + hFrom * 0.5f, 0.0f), m_rotation * Vector3.forward, m_rotation * Vector3.right, hFrom,  sincosFrom.x * radius2, radius1 * sincosFrom.x, segments, 1, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals,  smooth);
                else                CreateCone(pivotOffset + m_rotation * new Vector3(0.0f, sincosFrom.y * radius2 + hFrom * 0.5f, 0.0f), m_rotation * Vector3.forward, m_rotation * Vector3.right, -hFrom, radius1 * sincosFrom.x, sincosFrom.x * radius2, segments, 1, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals, smooth);
                if (cutTo > 0.5f)   CreateCone(pivotOffset + m_rotation * new Vector3(0.0f, sincosTo.y * radius2 + hTo * 0.5f, 0.0f),     m_rotation * Vector3.forward, m_rotation * Vector3.right, hTo,    sincosTo.x * radius2,   radius1 * sincosTo.x,   segments, 1, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals, smooth);
                else                CreateCone(pivotOffset + m_rotation * new Vector3(0.0f, sincosTo.y * radius2 + hTo * 0.5f, 0.0f),     m_rotation * Vector3.forward, m_rotation * Vector3.right, -hTo,   radius1 * sincosTo.x,   sincosTo.x * radius2,   segments, 1, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals,  smooth);
            }
        }
    }
}