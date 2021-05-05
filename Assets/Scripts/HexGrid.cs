using System;
using System.Collections;
using System.Collections.Generic;
using Hex.Brush;
using Hex.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Hex
{
    [ExecuteInEditMode, SelectionBase]
    public class HexGrid : MonoBehaviour
    {
        public bool ShowScope = true;
        public Material HexMat;
        public HexRenderer Renderer = new HexRenderer();
        
        public HexGridData Data;
        public BrushData BrushData;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var pos = transform.position;
            if (Data != null)
            {
                if (ShowScope)
                {
                    var poses = new Vector3[4];
                    poses[0] = new Vector3(Data.width * 0.5f, 0, - Data.height * 0.5f) + pos;
                    poses[1] = new Vector3(-Data.width * 0.5f, 0, - Data.height * 0.5f) + pos;
                    poses[2] = new Vector3(-Data.width * 0.5f, 0,  Data.height * 0.5f) + pos;
                    poses[3] = new Vector3(Data.width * 0.5f, 0, Data.height * 0.5f) + pos;

                    Handles.DrawSolidRectangleWithOutline(poses, Color.clear, Color.green);
                }

                if (HexMat != null && Data.hexDatas != null)
                {
                    foreach (var data in Data.hexDatas)
                    {
                        Renderer.ShowHex(data.hex, pos, Data.size, HexMat);
                        var hexPos = (Vector3)data.hex.HexToPixel(Data.size) + pos;
                        Handles.Label(hexPos, data.hex.ToOffsetString());
                    }
                }
            }
        }
#endif
    }
}