using UnityEngine;
using UnityEditor;
using Dreamteck.Splines;

namespace PixelFlow3D.Editor
{
    /// <summary>
    /// Builds conveyor belt visuals along a SplineComputer using original game models.
    /// Places straight pieces along edges, turn pieces at corners, start/end markers.
    /// </summary>
    public class ConveyorModelBuilder : EditorWindow
    {
        [MenuItem("PixelFlow3D/Build Conveyor Models")]
        public static void Build()
        {
            // Find the SplineComputer
            SplineComputer spline = FindObjectOfType<SplineComputer>();
            if (spline == null)
            {
                Debug.LogError("[ConveyorBuilder] No SplineComputer found in scene!");
                return;
            }

            // Find or create parent
            string parentName = "ConveyorModels";
            GameObject parent = GameObject.Find(parentName);
            if (parent != null)
                DestroyImmediate(parent);
            parent = new GameObject(parentName);

            // Load meshes and materials
            Mesh meshStraight = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Mesh/Conveyor1x1.asset");
            Mesh meshStart = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Mesh/Conveyor1x1Start.asset");
            Mesh meshEnd = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Mesh/Conveyor1x1End.asset");
            Mesh meshTurn = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Mesh/ConveyorTurn.asset");

            Material matStraight = AssetDatabase.LoadAssetAtPath<Material>("Assets/Material/Mat_Conveyor.mat");
            Material matTurn = AssetDatabase.LoadAssetAtPath<Material>("Assets/Material/Mat_ConveyorTurn.mat");
            Material matEnd = AssetDatabase.LoadAssetAtPath<Material>("Assets/Material/Mat_ConveyorEnd.mat");

            // Sample spline and place models facing the direction
            float splineLength = spline.CalculateLength();
            int count = Mathf.RoundToInt(splineLength / 0.5f); // One piece per 0.5 units

            for (int i = 0; i < count; i++)
            {
                float t = (float)i / count;
                SplineSample sample = spline.Evaluate(t);

                // Create object
                GameObject piece = new GameObject($"Conveyor_{i:D3}");
                piece.transform.SetParent(parent.transform);
                piece.transform.position = sample.position;

                // Face along spline direction
                if (sample.forward != Vector3.zero)
                    piece.transform.forward = sample.forward;

                // Add mesh
                var mf = piece.AddComponent<MeshFilter>();
                var mr = piece.AddComponent<MeshRenderer>();

                if (meshStraight != null)
                {
                    mf.sharedMesh = meshStraight;
                    mr.sharedMaterial = matStraight;
                }
            }

            // Place turn pieces at corners (every 25% of spline)
            for (int i = 0; i < 4; i++)
            {
                float t = (i * 0.25f + 0.125f) % 1f;
                SplineSample sample = spline.Evaluate(t);

                GameObject turn = new GameObject($"Turn_{i}");
                turn.transform.SetParent(parent.transform);
                turn.transform.position = sample.position;
                if (sample.forward != Vector3.zero)
                    turn.transform.forward = sample.forward;

                var mf = turn.AddComponent<MeshFilter>();
                var mr = turn.AddComponent<MeshRenderer>();
                if (meshTurn != null)
                {
                    mf.sharedMesh = meshTurn;
                    mr.sharedMaterial = matTurn;
                }
            }

            // Start piece at t=0
            {
                SplineSample s0 = spline.Evaluate(0);
                GameObject start = new GameObject("Conveyor_Start");
                start.transform.SetParent(parent.transform);
                start.transform.position = s0.position;
                if (s0.forward != Vector3.zero) start.transform.forward = s0.forward;
                var mf = start.AddComponent<MeshFilter>();
                var mr = start.AddComponent<MeshRenderer>();
                if (meshStart != null) { mf.sharedMesh = meshStart; mr.sharedMaterial = matStraight; }
            }

            // End piece at t=0.99
            {
                SplineSample se = spline.Evaluate(0.99);
                GameObject end = new GameObject("Conveyor_End");
                end.transform.SetParent(parent.transform);
                end.transform.position = se.position;
                if (se.forward != Vector3.zero) end.transform.forward = se.forward;
                var mf = end.AddComponent<MeshFilter>();
                var mr = end.AddComponent<MeshRenderer>();
                if (meshEnd != null) { mf.sharedMesh = meshEnd; mr.sharedMaterial = matStraight; }
            }

            Debug.Log($"[ConveyorBuilder] Built {count} straight + 4 turn + start/end conveyor pieces.");
        }
    }
}
