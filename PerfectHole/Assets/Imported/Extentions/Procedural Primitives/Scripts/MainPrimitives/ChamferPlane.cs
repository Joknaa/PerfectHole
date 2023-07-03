using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public class ChamferPlane : PPBase
    {
        [Header("Basic parameters")]
        public float width = 2;
        public float length = 2;
        public float fillet = 0.4f;
        [Header("Segments")]
        public int widthSegs = 10;
        public int lengthSegs = 10;
        public int filletSegs = 5;
        [Header("Mapping Coordinates")]
        public bool generateMappingCoords = true;
        public bool realWorldMapSize = false;
        public Vector2 UVOffset = new Vector2(0.0f, 0.0f);
        public Vector2 UVTiling = new Vector2(1.0f, 1.0f);
        [Header("Others")]
        public bool flipNormals = false;

        protected override void CreateMesh()
        {
            m_mesh.name = "ChamferPlane";

            length = Mathf.Clamp(length, 0.00001f, 10000.0f);
            width = Mathf.Clamp(width, 0.00001f, 10000.0f);
            float min = length < width ? length : width;
            fillet = Mathf.Clamp(fillet, 0.00001f, min / 2.0f);
            lengthSegs = Mathf.Clamp(lengthSegs, 1, 100);
            widthSegs = Mathf.Clamp(widthSegs, 1, 100);
            filletSegs = Mathf.Clamp(filletSegs, 1, 100);

            float lengthHalf = length * 0.5f;
            float widthHalf = width * 0.5f;
            float filletHalf = fillet * 0.5f;
            float lengthHalfFillet = lengthHalf - fillet;
            float widthHalfFillet = widthHalf - fillet;
            float lengthHalfFilletHalf = lengthHalf - filletHalf;
            float widthHalfFilletHalf = widthHalf - filletHalf;

            Vector2 tilingCircle = realWorldMapSize ? new Vector2(1.0f, 1.0f) : new Vector2(fillet / widthHalf, fillet / lengthHalf);
            tilingCircle = new Vector2(UVTiling.x * tilingCircle.x, UVTiling.y * tilingCircle.y);
            Vector2 tilingcenter = realWorldMapSize ? new Vector2(1.0f, 1.0f) : new Vector2(widthHalfFillet / widthHalf, lengthHalfFillet / lengthHalf);
            tilingcenter = new Vector2(UVTiling.x * tilingcenter.x, UVTiling.y * tilingcenter.y);
            Vector2 tilinglr = realWorldMapSize ? new Vector2(1.0f, 1.0f) : new Vector2(fillet / width, lengthHalfFillet / lengthHalf);
            tilinglr = new Vector2(UVTiling.x * tilinglr.x, UVTiling.y * tilinglr.y);
            Vector2 tilingtb = realWorldMapSize ? new Vector2(1.0f, 1.0f) : new Vector2(widthHalfFillet / widthHalf, fillet / length);
            tilingtb = new Vector2(UVTiling.x * tilingtb.x, UVTiling.y * tilingtb.y);

            //center
            CreatePlane(pivotOffset + Vector3.zero, m_rotation * Vector3.forward, m_rotation * Vector3.right, widthHalfFillet * 2, lengthHalfFillet * 2, widthSegs, lengthSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(filletHalf / widthHalf, filletHalf / lengthHalf), tilingcenter, flipNormals);

            //top bottom
            CreatePlane(pivotOffset + m_rotation * new Vector3(0.0f, 0.0f, lengthHalfFilletHalf),  m_rotation * Vector3.forward, m_rotation * Vector3.right, widthHalfFillet * 2, fillet, widthSegs, filletSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(filletHalf / widthHalf, (length - fillet) / length), tilingtb, flipNormals);
            CreatePlane(pivotOffset + m_rotation * new Vector3(0.0f, 0.0f, -lengthHalfFilletHalf), m_rotation * Vector3.forward, m_rotation * Vector3.right, widthHalfFillet * 2, fillet, widthSegs, filletSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(filletHalf / widthHalf, 0.0f),                       tilingtb, flipNormals);

            //left right
            CreatePlane(pivotOffset + m_rotation * new Vector3(-widthHalfFilletHalf, 0.0f, 0.0f), m_rotation * Vector3.forward, m_rotation * Vector3.right, fillet, lengthHalfFillet * 2, filletSegs, lengthSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(0.0f, filletHalf / lengthHalf),                     tilinglr, flipNormals);
            CreatePlane(pivotOffset + m_rotation * new Vector3(widthHalfFilletHalf, 0.0f, 0.0f),  m_rotation * Vector3.forward, m_rotation * Vector3.right, fillet, lengthHalfFillet * 2, filletSegs, lengthSegs, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2((width - fillet) / width, filletHalf / lengthHalf), tilinglr, flipNormals);

            //corner
            CreateCircle(pivotOffset + m_rotation * new Vector3(widthHalf - fillet, 0.0f, lengthHalf - fillet), m_rotation * Vector3.forward, m_rotation * Vector3.right, fillet, filletSegs, filletSegs, true, 0.0f,   90.0f,  generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(widthHalfFillet / width, lengthHalfFillet / length),   tilingCircle, flipNormals);
            CreateCircle(pivotOffset + m_rotation * new Vector3(widthHalf - fillet, 0.0f, -lengthHalfFillet),   m_rotation * Vector3.forward, m_rotation * Vector3.right, fillet, filletSegs, filletSegs, true, 90.0f,  180.0f, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(widthHalfFillet / width, -lengthHalfFillet / length),  tilingCircle, flipNormals);
            CreateCircle(pivotOffset + m_rotation * new Vector3(-widthHalfFillet, 0.0f,   -lengthHalfFillet),   m_rotation * Vector3.forward, m_rotation * Vector3.right, fillet, filletSegs, filletSegs, true, 180.0f, 270.0f, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(-widthHalfFillet / width, -lengthHalfFillet / length), tilingCircle, flipNormals);
            CreateCircle(pivotOffset + m_rotation * new Vector3(-widthHalfFillet, 0.0f,   lengthHalf - fillet), m_rotation * Vector3.forward, m_rotation * Vector3.right, fillet, filletSegs, filletSegs, true, 270.0f, 360.0f, generateMappingCoords, realWorldMapSize, UVOffset + new Vector2(-widthHalfFillet / width, lengthHalfFillet / length),  tilingCircle, flipNormals);
        }
    }
}