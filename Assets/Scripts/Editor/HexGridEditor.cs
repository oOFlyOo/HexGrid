using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Hex.Brush;
using Hex.Data;
using UnityEditor;
using UnityEditor.Presets;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Hex.Editor
{
    [CustomEditor(typeof(HexGrid))]
    public class HexGridEditor : UnityEditor.Editor
    {
        private static BrushData.BrushType _brushType = BrushData.BrushType.None;
        private static BrushFeature _brushFeature = new BrushFeature();

        private HexGrid _grid;
        private int _curBrush = 0;

        private void OnEnable()
        {
            _grid = (HexGrid) target;
        }

        public override void OnInspectorGUI()
        {
            if (_grid.HexMat == null)
            {
                LoadMaterial();
            }
            _grid.HexMat = EditorUIHelper.ObjectField<Material>(_grid.HexMat, "材质球");
            
            _grid.Data = EditorUIHelper.ObjectField<HexGridData>(_grid.Data, "数据");
            var gridData = _grid.Data;
            if (gridData == null)
            {
                EditorUIHelper.Space();
                EditorUIHelper.Button("创建数据", CreateHexGridData);
            }
            else
            {
                gridData.width = EditorUIHelper.Slider("宽度", gridData.width, 0, 1000);
                gridData.height = EditorUIHelper.Slider("高度", gridData.height, 0, 1000);
                gridData.size = EditorUIHelper.Slider("六边形边长", gridData.size, 0, 10);
                EditorUIHelper.Button("更新HexGrid", UpdateHexGrid);

                _grid.BorderColor = EditorUIHelper.ColorField(_grid.BorderColor, "边界颜色");
                EditorUIHelper.PropertyField(serializedObject, "Renderer", "网格颜色");
                _grid.ShowScope = EditorUIHelper.Toggle("显示边界", _grid.ShowScope);
                _grid.ShowGrid = EditorUIHelper.Toggle("显示网格", _grid.ShowGrid);
            }

            EditorUIHelper.Space();
            _grid.BrushData = EditorUIHelper.ObjectField<BrushData>(_grid.BrushData, "笔刷");
            var brushData = _grid.BrushData;
            if (brushData == null)
            {
                EditorUIHelper.Space();
                EditorUIHelper.Button("创建笔刷", CreateBrushData);
            }
            else
            {
                EditorUIHelper.Button("编辑笔刷", () => BrushDataEditorWindow.Open(brushData));
                
                EditorUIHelper.Space();
                _brushType = EditorUIHelper.EnumPopup<BrushData.BrushType>(_brushType, "笔刷类型");

                if (brushData.pathBrushes.Count > 0)
                {
                    _curBrush = EditorUIHelper.Popup("刷路", _curBrush,
                        brushData.pathBrushes.Select(brush => brush.name).ToArray());
                }

                _brushFeature.brushOptionType = EditorUIHelper.EnumPopup<BrushFeature.BrushOptionType>(_brushFeature.brushOptionType);
            }

            if (EditorUIHelper.Changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                if (gridData != null)
                {
                    EditorUtility.SetDirty(gridData);
                }
            }
        }

        private void LoadMaterial()
        {
            _grid.HexMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/HexRenderer.mat");
        }

        private void CreateHexGridData()
        {
            var scenePath = EditorSceneManager.GetActiveScene().path;
            var path = EditorUtility.SaveFilePanel("创建数据", Path.GetDirectoryName(scenePath),
                $"{Path.GetFileNameWithoutExtension(scenePath)}_data", "asset");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var folderPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            path = path.Substring(path.IndexOf("Assets"));
            var data = AssetDatabase.LoadAssetAtPath<HexGridData>(path);
            if (data == null)
            {
                data = CreateInstance<HexGridData>();
                AssetDatabase.CreateAsset(data, path);
            }
            
            _grid.Data = data;
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
        
        private void CreateBrushData()
        {
            var scenePath = EditorSceneManager.GetActiveScene().path;
            var path = EditorUtility.SaveFilePanel("创建笔刷", Path.GetDirectoryName(scenePath),
                $"{Path.GetFileNameWithoutExtension(scenePath)}_brush", "asset");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var folderPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            path = path.Substring(path.IndexOf("Assets"));
            var data = AssetDatabase.LoadAssetAtPath<BrushData>(path);
            if (data == null)
            {
                data = CreateInstance<BrushData>();

                var preset = AssetDatabase.LoadAssetAtPath<Preset>("Assets/Presets/BrushData.preset");
                if (preset != null)
                {
                    preset.ApplyTo(data);
                }
                AssetDatabase.CreateAsset(data, path);
            }
            
            _grid.BrushData = data;
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        private void UpdateHexGrid()
        {
            _grid.Data.CalculateGridNum();
            _grid.Data.CreateHexDatas();
        }

        private void OnPreSceneGUI()
        {
            CheckBrush();
        }

        private void OnSceneGUI()
        {
            ShowFeature();
            // CheckBrush();
        }
        
        private void CheckBrush()
        {
            if (_brushType == BrushData.BrushType.None)
            {
                return;
            }
            
            if (_grid.Data == null)
            {
                return;
            }
            
            var brushData = _grid.BrushData;
            if (brushData == null || !brushData.HasPathBrushes())
            {
                return;
            }

            if (_curBrush < 0 || _curBrush > brushData.pathBrushes.Count)
            {
                return;
            }

            var brush = brushData.pathBrushes[_curBrush];
            var gridPos = _grid.transform.position;

            var curEvent = Event.current;
            HexPoint worldPos = GetMouseWorldPosition(curEvent.mousePosition, _grid.transform);

            var gridData = _grid.Data;
            brush.renderer.ShowHex(worldPos.PixelToHex(gridData.size), gridPos, gridData.size, _grid.HexMat);
            UpdateSceneView();
            
            // HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (curEvent.type == EventType.MouseUp)
            {
                var hex = worldPos.PixelToHex(gridData.size);
                HexMetrics.AxialToOffset(hex.X, hex.Z, out int row, out int col);
                gridData.RowColToIndex(row, col, out int rowIndex, out int colIndex);
                var data = gridData[rowIndex, colIndex];
                if (data != null)
                {
                    if (data.path != brush.path)
                    {
                        data.path = brush.path;
                    }
                    // curEvent.Use();
                }
            }

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            // GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
        }

        private void UpdateSceneView()
        {
            // new Mesh也会触发
            SceneView.lastActiveSceneView.Repaint();
        }

        private void ShowFeature()
        {
            if (_grid.Data == null || !_grid.Data.HasHexDatas())
            {
                return;
            }
            
            var brushData = _grid.BrushData;
            if (brushData == null || !brushData.HasPathBrushes())
            {
                return;
            }
            
            var gridData = _grid.Data;
            
            var gridPos = _grid.transform.position;
            foreach (var data in gridData.hexDatas)
            {
                if (data.path != null)
                {
                    var brush = brushData.GetPathBrushByPathType(data.path.pathType);
                    brush.renderer.ShowHex(data.hex, gridPos, gridData.size, _grid.HexMat);
                }
            }
        }

        private Vector3 GetMouseWorldPosition(Vector2 mousePos, Transform parent = null)
        {
            var worldRay = HandleUtility.GUIPointToWorldRay(mousePos);
            worldRay.direction = worldRay.direction.normalized;
            var dist = worldRay.origin.y / (-worldRay.direction.y);
            var worldPos = worldRay.origin + worldRay.direction * dist;
            if (parent != null)
            {
                worldPos = parent.InverseTransformPoint(worldPos);
            }

            return worldPos;
        }
    }
}