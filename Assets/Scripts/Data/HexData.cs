using System;
using UnityEngine;

namespace Hex.Data
{
    [Serializable]
    public class HexData
    {
        public HexCoordinates hex;
        [SerializeReference]
        public PathFeature path;

        public static HexData CreateFromHex(HexCoordinates hex)
        {
            var data = new HexData()
            {
                hex = hex,
            };

            return data;
        }
    }
}