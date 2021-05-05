using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HexPoint
{
    public float x;
    public float z;

    public float y;

    public HexPoint(float x, float z, float y)
    {
        this.x = x;
        this.z = z;
        this.y = y;
    }

    public static implicit operator Vector3(HexPoint point)
    {
        return new Vector3(point.x, point.y, point.z);
    }
    
    public static implicit operator HexPoint(Vector3 point)
    {
        return new HexPoint(point.x, point.z, point.y);
    }
    
    public static HexPoint operator+(HexPoint pointA, HexPoint pointB)
    {
        return new HexPoint(pointA.x + pointB.x, pointA.z + pointB.z, pointA.y + pointB.y);
    }
        
    public static HexPoint operator-(HexPoint pointA, HexPoint pointB)
    {
        return new HexPoint(pointA.x - pointB.x, pointA.z - pointB.z, pointA.y - pointB.y);
    }

    public override string ToString()
    {
        return ((Vector3) this).ToString();
    }
}
