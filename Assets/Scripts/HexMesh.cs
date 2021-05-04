using System.Collections;
using System.Collections.Generic;
using Hex;
using UnityEngine;

public static class HexMesh
{
    private static Dictionary<float, Mesh> _instanceMeshes = new Dictionary<float, Mesh>();

    public static Mesh GetMesh(float size)
    {
        if (!_instanceMeshes.TryGetValue(size, out Mesh mesh))
        {
            mesh = CreateMesh(size);
            _instanceMeshes[size] = mesh;
        }

        return mesh;
    }

    public static Mesh CreateMesh(float size)
    {
        var vertices = new List<Vector3>();
        var uv = new List<Vector2>();
        var triangles = new List<int>();
        
        vertices.Add(Vector3.zero);
        uv.Add(Vector2.zero);

        foreach (var corner in HexMetrics.HexCorners(size))
        {
            vertices.Add(corner);
            uv.Add(Vector2.one);
        }

        for (int i = 1; i < HexMetrics.CornerNums; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }
        
        // 循环一周
        triangles.Add(0);
        triangles.Add(HexMetrics.CornerNums);
        triangles.Add(1);

        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
