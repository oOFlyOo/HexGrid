using UnityEngine;

namespace Hex.Data
{
    public class HexGridData: ScriptableObject
    {
        public float width;
        public float height;
        public float size;

        public int halfRowGridNum;
        public int halfColGridNum;

        public int rowGridNum;
        public int colGridNum;

        public HexData[] hexDatas;

        public HexData this[int rowIndex, int colIndex]
        {
            get
            {
                if (!ContainsIndex(rowIndex, colIndex))
                {
                    return null;
                }
                
                return hexDatas[IndexToArrayIndex(rowIndex, colIndex)];
            }
            set
            {
                if (!ContainsIndex(rowIndex, colIndex))
                {
                    return;
                }
                
                hexDatas[IndexToArrayIndex(rowIndex, colIndex)] = value;
            }
        }

        public void CalculateGridNum()
        {
            if (width <= 0 || height <= 0 || size <= 0)
            {
                return;
            }

            var edgeWidth = HexMetrics.OuterToDoubleInner(size);
            var edgeHeight = size * 3 / 2;
            // 保证原点扩展，偏移之后也在范围内
            halfRowGridNum = (int) ((height - 2 * size) / edgeHeight * 0.5);
            halfColGridNum = (int) (width / edgeWidth * 0.5) - 1;

            rowGridNum = halfRowGridNum * 2 + 1;
            colGridNum = halfColGridNum * 2 + 1;
        }

        
        /// <summary>
        /// index取左下角为原点
        /// row和col取中心为原点
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        public void RowColToIndex(int row, int col, out int rowIndex, out int colIndex)
        {
            rowIndex = row + halfRowGridNum;
            colIndex = col + halfColGridNum;
        }

        public void IndexToRowCol(int rowIndex, int colIndex, out int row, out int col)
        {
            row = rowIndex - halfRowGridNum;
            col = colIndex - halfColGridNum;
        }

        public int IndexToArrayIndex(int rowIndex, int colIndex)
        {
            var index = rowIndex * colGridNum + colIndex;

            return index;
        }

        public bool ContainsIndex(int rowIndex, int colIndex)
        {
            if (rowIndex < 0 || rowIndex >= rowGridNum)
            {
                return false;
            }
            
            if (colIndex < 0 || colIndex >= colGridNum)
            {
                return false;
            }

            return true;
        }

        public bool HasHexDatas()
        {
            return hexDatas != null && hexDatas.Length > 0;
        }
        
        public void CreateHexDatas()
        {
            if (rowGridNum <= 0 || colGridNum <= 0)
            {
                return;
            }

            hexDatas = new HexData[rowGridNum * colGridNum];
            for (int rowIndex = 0; rowIndex < rowGridNum; rowIndex++)
            {
                for (int colIndex = 0; colIndex < colGridNum; colIndex++)
                {
                    IndexToRowCol(rowIndex, colIndex, out int row, out int col);
                    HexMetrics.OffsetToAxial(row, col, out int x, out int z);
                    this[rowIndex, colIndex] = HexData.CreateFromHex(new HexCoordinates(x, z));
                }
            }
        }
    }
}