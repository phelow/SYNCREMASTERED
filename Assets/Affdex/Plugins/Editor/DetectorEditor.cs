using System;
using UnityEditor;

namespace Affdex.Editor
{
    /// <summary>
    /// Unity's Detector custom component UI
    /// </summary>
    [CustomEditor(typeof(Detector))]
    public class DetectorEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Overrides Editor.OnInspectorGUI() to create custom Detector Component UI
        /// </summary>
        public override void OnInspectorGUI()
        {
            Detector detector = (Detector)target;
            Undo.RecordObject(detector, "Affdex Setting");
            detector.startOnWake = EditorGUILayout.Toggle("Start on Awake", detector.startOnWake);
            detector.emotions = (Emotions)EditorGUILayout.EnumMaskField("Emotions", detector.emotions);
            detector.expressions = (Expressions)EditorGUILayout.EnumMaskField("Expressions", detector.expressions);

            
        }
    }
}
