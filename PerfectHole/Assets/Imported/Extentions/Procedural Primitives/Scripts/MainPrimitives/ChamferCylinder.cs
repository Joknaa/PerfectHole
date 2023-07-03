using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class ChamferCylinder : PPBase
    {
        [Header("Basic parameters")]
        public float radius = 0.5f;
        public float height = 1.0f;
        public float fillet = 0.1f;
        [Header("Segments")]
        public int sides = 20;
        public int capSegs = 2;
        public int heightSegs = 2;
        public int filletSegs = 3;
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
            m_mesh.name = "ChamferCylinder";

            radius = Mathf.Clamp(radius, 0.00001f, 10000.0f);
            height = Mathf.Clamp(height, 0.00001f, 10000.0f);
            float min = radius < height / 2.0f ? radius : height / 2.0f;
            fillet = Mathf.Clamp(fillet, 0.00001f, min);
            sides = Mathf.Clamp(sides, 3, 256);
            capSegs = Mathf.Clamp(capSegs, 1, 100);
            heightSegs = Mathf.Clamp(heightSegs, 1, 100);
            filletSegs = Mathf.Clamp(filletSegs, 1, 100);
            sliceFrom = Mathf.Clamp(sliceFrom, 0.0f, 360.0f);
            sliceTo = Mathf.Clamp(sliceTo, sliceFrom, 360.0f);

            float heightHalf = height * 0.5f;
            float heightHalfFillet = heightHalf - fillet;
            float radiusFillet = radius - fillet;

            Vector2 tilingTorus = realWorldMapSize ? new Vector2(1.0f, 1.0f) : new Vector2(1.0f, 2.0f);
            tilingTorus = new Vector2(UVTiling.x * tilingTorus.x, UVTiling.y * tilingTorus.y);

            CreateCylinder(pivotOffset + Vector3.zero, m_rotation * Vector3.forward, m_rotation * Vector3.right, heightHalfFillet * 2, radius, sides, heightSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals, smooth);
            CreateCircle(pivotOffset + m_rotation * new Vector3(0.0f, heightHalf, 0.0f),  m_rotation * Vector3.forward, m_rotation * Vector3.right, radiusFillet, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, flipNormals);
            CreateCircle(pivotOffset + m_rotation * new Vector3(0.0f, -heightHalf, 0.0f), m_rotation * Vector3.forward, m_rotation * Vector3.right, radiusFillet, sides, capSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset, UVTiling, !flipNormals);
            CreateTorus(pivotOffset + m_rotation * new Vector3(0.0f, -heightHalfFillet, 0.0f), m_rotation * Vector3.forward, m_rotation * Vector3.right, radiusFillet, fillet, sides, filletSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(0.0f, 0.0f), tilingTorus, flipNormals, smooth, 90.0f,  180.0f);
            CreateTorus(pivotOffset + m_rotation * new Vector3(0.0f, heightHalfFillet, 0.0f),  m_rotation * Vector3.forward, m_rotation * Vector3.right, radiusFillet, fillet, sides, filletSegs, sliceOn, sliceFrom, sliceTo, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(0.0f, 0.5f), tilingTorus, flipNormals, smooth, 180.0f, 270.0f);

            if (sliceOn)
            {
                float filletHalf = fillet * 0.5f;
                float heightHalfFilletHalf = heightHalf - filletHalf;
                float radiusFilletHalf = radius - filletHalf;
                float rfh = radiusFillet * 0.5f;

                Vector3 centerFrom = new Vector3(Mathf.Sin(sliceFrom * Mathf.Deg2Rad), 0.0f, Mathf.Cos(sliceFrom * Mathf.Deg2Rad));
                Vector3 centerTo = new Vector3(Mathf.Sin(sliceTo * Mathf.Deg2Rad), 0.0f, Mathf.Cos(sliceTo * Mathf.Deg2Rad));

                Vector2 tilingCircle = realWorldMapSize ? new Vector2(1.0f, 1.0f) : new Vector2(fillet / radius, fillet / heightHalf);
                tilingCircle = new Vector2(UVTiling.x * tilingCircle.x, UVTiling.y * tilingCircle.y);
                Vector2 tilingcenter = realWorldMapSize ? new Vector2(1.0f, 1.0f) : new Vector2(rfh / radius, heightHalfFillet / heightHalf);
                tilingcenter = new Vector2(UVTiling.x * tilingcenter.x, UVTiling.y * tilingcenter.y);
                Vector2 tilinglr = realWorldMapSize ? new Vector2(1.0f, 1.0f) : new Vector2(filletHalf / radius, heightHalfFillet / heightHalf);
                tilinglr = new Vector2(UVTiling.x * tilinglr.x, UVTiling.y * tilinglr.y);
                Vector2 tilingtb = realWorldMapSize ? new Vector2(1.0f, 1.0f) : new Vector2(rfh / radius, fillet / height);
                tilingtb = new Vector2(UVTiling.x * tilingtb.x, UVTiling.y * tilingtb.y);

                //center
                CreatePlane(pivotOffset + m_rotation * centerFrom * rfh, m_rotation * Vector3.up, m_rotation * -centerFrom.normalized, radiusFillet, heightHalfFillet * 2, capSegs, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(filletHalf / radius, filletHalf / heightHalf), tilingcenter, flipNormals);
                CreatePlane(pivotOffset + m_rotation * centerTo * rfh,   m_rotation * Vector3.up, m_rotation * centerTo.normalized, radiusFillet,    heightHalfFillet * 2, capSegs, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(0.5f, filletHalf / heightHalf),                tilingcenter, flipNormals);

                //left right
                CreatePlane(pivotOffset + m_rotation * centerFrom * radiusFilletHalf, m_rotation * Vector3.up, m_rotation * -centerFrom.normalized, fillet, heightHalfFillet * 2, filletSegs, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(0.0f, filletHalf / heightHalf),                      tilinglr, flipNormals);
                CreatePlane(pivotOffset + m_rotation * centerTo * radiusFilletHalf,   m_rotation * Vector3.up, m_rotation * centerTo.normalized, fillet,    heightHalfFillet * 2, filletSegs, heightSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(radiusFilletHalf / radius, filletHalf / heightHalf), tilinglr, flipNormals);

                //top bottom
                CreatePlane(pivotOffset + m_rotation * new Vector3(centerFrom.x * rfh, heightHalfFilletHalf, centerFrom.z * rfh),  m_rotation * Vector3.up, m_rotation * -centerFrom.normalized, radiusFillet, fillet, capSegs, filletSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(filletHalf / radius, (height - fillet) / height), tilingtb, flipNormals);
                CreatePlane(pivotOffset + m_rotation * new Vector3(centerFrom.x * rfh, -heightHalfFilletHalf, centerFrom.z * rfh), m_rotation * Vector3.up, m_rotation * -centerFrom.normalized, radiusFillet, fillet, capSegs, filletSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(filletHalf / radius, 0.0f),                       tilingtb, flipNormals);
                CreatePlane(pivotOffset + m_rotation * new Vector3(centerTo.x * rfh, heightHalfFilletHalf, centerTo.z * rfh),      m_rotation * Vector3.up, m_rotation * centerTo.normalized,    radiusFillet, fillet, capSegs, filletSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(0.5f, (height - fillet) / height),                tilingtb, flipNormals);
                CreatePlane(pivotOffset + m_rotation * new Vector3(centerTo.x * rfh, -heightHalfFilletHalf, centerTo.z * rfh),     m_rotation * Vector3.up, m_rotation * centerTo.normalized,    radiusFillet, fillet, capSegs, filletSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(0.5f, 0.0f),                                      tilingtb, flipNormals);

                //corner
                CreateCircle(pivotOffset + m_rotation * new Vector3(centerFrom.x * radiusFillet, heightHalfFillet, centerFrom.z * radiusFillet),  m_rotation * Vector3.up, m_rotation * -centerFrom.normalized, fillet, filletSegs, filletSegs, true, 270.0f, 360.0f, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(-radiusFillet / (radius * 2), heightHalfFillet / height),  tilingCircle, flipNormals);
                CreateCircle(pivotOffset + m_rotation * new Vector3(centerFrom.x * radiusFillet, -heightHalfFillet, centerFrom.z * radiusFillet), m_rotation * Vector3.up, m_rotation * -centerFrom.normalized, fillet, filletSegs, filletSegs, true, 180.0f, 270.0f, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(-radiusFillet / (radius * 2), -heightHalfFillet / height), tilingCircle, flipNormals);
                CreateCircle(pivotOffset + m_rotation * new Vector3(centerTo.x * radiusFillet, heightHalfFillet, centerTo.z * radiusFillet),      m_rotation * Vector3.up, m_rotation * centerTo.normalized,    fillet, filletSegs, filletSegs, true, 0.0f,   90.0f,  generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(radiusFillet / (radius * 2),  heightHalfFillet / height),  tilingCircle, flipNormals);
                CreateCircle(pivotOffset + m_rotation * new Vector3(centerTo.x * radiusFillet, -heightHalfFillet, centerTo.z * radiusFillet),     m_rotation * Vector3.up, m_rotation * centerTo.normalized,    fillet, filletSegs, filletSegs, true, 90.0f,  180.0f, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(radiusFillet / (radius * 2),  -heightHalfFillet / height), tilingCircle, flipNormals);
            }
        }
    }
}