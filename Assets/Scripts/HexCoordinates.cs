using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hex
{
    [Serializable]
    public struct HexCoordinates
    {
        [SerializeField]
        private int x;
        [SerializeField]
        private int z;
        
        public int X => x;
        public int Z => z;
        /// <summary>
        /// 左手坐标系
        /// </summary>
        public int Y => -x-z;

        public HexCoordinates(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public override string ToString()
        {
            return $"({x}, {z})";
        }
        
        public string ToOffsetString()
        {
            HexMetrics.AxialToOffset(x, z, out int row, out int col);
            
            return $"({col}, {row})";
        }
        
        public static HexCoordinates operator+(HexCoordinates hexA, HexCoordinates hexB)
        {
            return new HexCoordinates(hexA.x + hexB.x, hexA.z + hexB.z);
        }
        
        public static HexCoordinates operator-(HexCoordinates hexA, HexCoordinates hexB)
        {
            return new HexCoordinates(hexA.x - hexB.x, hexA.z - hexB.z);
        }
    }
}