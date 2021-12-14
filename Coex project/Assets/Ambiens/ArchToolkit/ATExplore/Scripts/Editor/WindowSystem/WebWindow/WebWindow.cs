using UnityEditor;
using System;
using System.Reflection;

public class WebWindow : EditorWindow
{
    private static string Url = "https://www.archtoolkit.com/";

    private static MethodInfo methodInfo;

    static WebWindow()
    {
        if (EditorPrefs.HasKey("ATKey"))
        {
            if(!EditorPrefs.GetBool("ATKey"))
            {
                EditorPrefs.SetBool("ATKey", true);
                Open();
            }
        }
        else
        {
            EditorPrefs.SetBool("ATKey",true);
            Open();
        }
    }
    
    static void Open()
    {
        string typeName = "UnityEditor.Web.WebViewEditorWindowTabs";

        Type type = Assembly.Load("UnityEditor.dll").GetType(typeName);
        
        BindingFlags Flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        methodInfo = type.GetMethod("Create", Flags);
        methodInfo = methodInfo.MakeGenericMethod(type);

        methodInfo.Invoke(null, new object[] { "Info", Url, 200, 530, 1000, 800 });
    }
}
