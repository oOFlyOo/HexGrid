using System;
using UnityEngine;

namespace Hex
{
    [Serializable]
    public class HexRenderer
    {
        public Color color = Color.white;
        public float minSize = 1;
        public float maxSize = 1;
        public float pointInternal = 1;

        private Material _mat;
        
        public void ShowHex(HexCoordinates hex, Vector3 offset, float size, Material mat)
        {
            mat = GetMaterial(mat);
            mat.SetColor("_Color", color);
            mat.SetFloat("_MinSize", minSize);
            mat.SetFloat("_MaxSize", maxSize);
            mat.SetFloat("_Internal", pointInternal);
            
            mat.SetPass(0);
            Graphics.DrawMeshNow(HexMesh.GetMesh(size), (Vector3)hex.HexToPixel(size) + offset, Quaternion.identity);
        }

        private Material GetMaterial(Material origin)
        {
            if (_mat == null || _mat.shader != origin.shader)
            {
                _mat = new Material(origin);
            }

            return _mat;
        }
    }
}