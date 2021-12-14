using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ReadOnlyAttribute : PropertyAttribute
{



#if UNITY_EDITOR
    public virtual void DrawInInspector(FieldInfo i, object o)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(i.Name);
        object value = i.GetValue(o);
        EditorGUILayout.LabelField(i.GetValue(o).ToString());

        //put this in to debug new drawing types
        //	EditorGUILayout.LabelField (i.GetValue (o).ToString () + " (" +  value.GetType ().ToString () + ")");
        GUILayout.EndHorizontal();
        GUI.enabled = false;
        if (value is Color)
        {
            EditorGUILayout.ColorField((Color)value);
        }
        GUI.enabled = true;

    }
#endif


}