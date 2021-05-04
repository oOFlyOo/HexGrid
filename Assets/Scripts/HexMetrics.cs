using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hex
{
    public static class HexMetrics
    {
        public static readonly float Sqrt3 = Mathf.Sqrt(3);

        public static readonly int EdgeNums = 6;
        public static readonly int CornerNums = 6;
        
        public enum EdgeType
        {
            Edge1,
            Edge2,
            Edge3,
            Edge4,
            Edge5,
            Edge6,
        }

        /// <summary>
        /// 右边开始
        /// 顺时针计算
        /// </summary>
        public static EdgeType[] EdgeArray = new[]
        {
            EdgeType.Edge1, 
            EdgeType.Edge2, 
            EdgeType.Edge3, 
            EdgeType.Edge4, 
            EdgeType.Edge5, 
            EdgeType.Edge6, 
        };
        
        public enum CornerType
        {
            Corner1,
            Corner2,
            Corner3,
            Corner4,
            Corner5,
            Corner6,
        }
        
        /// <summary>
        /// 右下角开始计算
        /// 顺时针计算
        /// </summary>
        public static CornerType[] CornerArray = new[]
        {
            CornerType.Corner1,
            CornerType.Corner2,
            CornerType.Corner3,
            CornerType.Corner4,
            CornerType.Corner5,
            CornerType.Corner6,
        };
        
        public static float OuterToInner(float size)
        {
            return Sqrt3 * 0.5f * size;
        }

        public static float OuterToDoubleInner(float size)
        {
            return Sqrt3 * size;
        }

        public static HexPoint HexToPixel(this HexCoordinates hex, float size)
        {
            var x = hex.X * OuterToDoubleInner(size) + hex.Z * OuterToInner(size);
            var z = hex.Z * size * 3 / 2;
            
            return new HexPoint(x, z, 0);
        }
        
        public static HexCoordinates PixelToHex(this HexPoint point, float size)
        {
            var x = Sqrt3 / 3 * point.x / size - 1f / 3 * point.z / size;
            var z = 2f / 3 * point.z / size;
            var y = - x - z;
            var fHex = new FractionalHex(x, z, y);

            return fHex.HexRound();
        }

        public static HexCoordinates HexRound(this FractionalHex fhex)
        {
            var x = Mathf.RoundToInt(fhex.x);
            var y = Mathf.RoundToInt(fhex.y);
            var z = Mathf.RoundToInt(fhex.z);

            var difX = Mathf.Abs(fhex.x - x);
            var difY = Mathf.Abs(fhex.y - y);
            var difZ = Mathf.Abs(fhex.z - z);

            if (difX > difY && difX > difZ)
            {
                x = -y - z;
            }
            else if (difZ > difY)
            {
                z = -x - y;
            }
            
            return new HexCoordinates(x, z);
        }

        
        public static HexCoordinates HexNeighbour(EdgeType edgeType)
        {
            switch (edgeType)
            {
                case EdgeType.Edge1: 
                    return new HexCoordinates(1, 0);
                case EdgeType.Edge2: 
                    return new HexCoordinates(1, - 1);
                case EdgeType.Edge3: 
                    return new HexCoordinates(0, - 1);
                case EdgeType.Edge4: 
                    return new HexCoordinates(- 1, 0);
                case EdgeType.Edge5: 
                    return new HexCoordinates(- 1, 1);
                case EdgeType.Edge6: 
                    return new HexCoordinates(0, 1);
            }

            return new HexCoordinates(0, 0);
        }

        public static IEnumerable<HexCoordinates> HexNeighbours()
        {
            foreach (var edgeType in EdgeArray)
            {
                yield return HexNeighbour(edgeType);
            }
        }

        public static HexCoordinates Neighbour(this HexCoordinates hex, EdgeType edgeType)
        {
            return hex + HexNeighbour(edgeType);
        }
        
        public static IEnumerable<HexCoordinates> Neighbours(this HexCoordinates hex)
        {
            foreach (var edgeType in EdgeArray)
            {
                yield return hex.Neighbour(edgeType);
            }
        }

        public static HexPoint HexCorner(float size, CornerType cornerType)
        {
            var innerSize = OuterToInner(size);
            var halfSize = size * 0.5f;
            var offset = new HexPoint(0, 0, 0);
            
            switch (cornerType)
            {
                case CornerType.Corner1: 
                    offset = new HexPoint(innerSize, -halfSize, 0);
                    break;
                case CornerType.Corner2: 
                    offset = new HexPoint(0, -size, 0);
                    break;
                case CornerType.Corner3: 
                    offset = new HexPoint(-innerSize, -halfSize, 0);
                    break;
                case CornerType.Corner4: 
                    offset = new HexPoint(-innerSize, halfSize, 0);
                    break;
                case CornerType.Corner5: 
                    offset = new HexPoint(0, size, 0);
                    break;
                case CornerType.Corner6: 
                    offset = new HexPoint(innerSize, halfSize, 0);
                    break;
            }

            return offset;
        }

        public static IEnumerable<HexPoint> HexCorners(float size)
        {
            foreach (var corner in CornerArray)
            {
                yield return HexCorner(size, corner);
            }
        }

        public static HexPoint Corner(this HexCoordinates hex, float size, CornerType cornerType)
        {
            return hex.HexToPixel(size) + HexCorner(size, cornerType);
        }
        
        public static IEnumerable<HexPoint> Corners(this HexCoordinates hex, float size)
        {
            foreach (var corner in CornerArray)
            {
                yield return hex.Corner(size, corner);
            }
        }
    }
}