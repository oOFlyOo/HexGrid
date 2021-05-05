using System;
using Hex.Brush;
using UnityEditor;

namespace Hex.Editor
{
    public class BrushDataEditorWindow: EditorWindow
    {
        private BrushData _brushData;
        private UnityEditor.Editor _brushEditor;

        public static void Open(BrushData brushData)
        {
            var win = GetWindow<BrushDataEditorWindow>(true, "BrushData");
            win._brushData = brushData;
            win._brushEditor = UnityEditor.Editor.CreateEditor(brushData);
        }


        private void OnGUI()
        {
            if (_brushEditor == null)
            {
                return;
            }
            
            _brushEditor.OnInspectorGUI();
        }
    }
}