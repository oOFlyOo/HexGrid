using System.Collections.Generic;
using System.Linq;
using Hex.Data;
using UnityEngine;

namespace Hex.Brush
{
    public class BrushData : ScriptableObject
    {
        public List<PathBrush> pathBrushes = new List<PathBrush>()
        {
            new PathBrush()
            {
                name = "Pass",
                renderer = new HexRenderer()
                {
                    color = Color.green,
                },
                path = new PathFeature()
                {
                    pathType = PathFeature.PathType.Default,
                },
            },
            new PathBrush()
            {
                name = "Obstacle",
                renderer = new HexRenderer()
                {
                    color = Color.red,
                },
                path = new PathFeature()
                {
                    pathType = PathFeature.PathType.Obstacle,
                },
            }
        };

        public PathBrush GetPathBrushByPathType(PathFeature.PathType type)
        {
            var brush = pathBrushes.FirstOrDefault(pathBrush => pathBrush.path.pathType == type);
            return brush;
        }
    }
}