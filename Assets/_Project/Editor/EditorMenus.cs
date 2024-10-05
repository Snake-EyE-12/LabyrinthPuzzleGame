using System;
using System.Reflection;
using UnityEditor;

static class EditorMenus
{
    [MenuItem("Tools/Toggle Lock #q")]
    static void ToggleInspectorLock() // Inspector must be inspecting something to be locked
    {
        var typ = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
        EditorWindow window = EditorWindow.GetWindow(typ);
        
        
    
        Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
        PropertyInfo propertyInfo = type.GetProperty("isLocked");
        bool value = (bool)propertyInfo.GetValue(window, null);
        propertyInfo.SetValue(window, !value, null);
        window.Repaint();
    }
}