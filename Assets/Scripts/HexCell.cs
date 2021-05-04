using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Hex
{
    public class HexCell : MonoBehaviour
    {
        [SerializeField]
        private HexCoordinates _hex;
        
            
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.Label(transform.position, _hex.ToString());
        }

        [ContextMenu("TestCreateMesh")]
        private void TestCreateMesh()
        {
            var mesh = HexMesh.GetMesh(10);
            var go = new GameObject("Mesh");
            go.AddComponent<MeshFilter>().sharedMesh = mesh;
            go.AddComponent<MeshRenderer>();
            go.AddComponent<MeshCollider>();
        }
#endif
    }
}