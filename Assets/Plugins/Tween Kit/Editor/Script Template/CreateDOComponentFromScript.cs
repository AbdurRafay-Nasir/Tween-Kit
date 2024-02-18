#if UNITY_EDITOR

using UnityEditor;

public static class CreateDOComponentFromScript
{
    [MenuItem("Assets/Create/Tween Kit/DOComponent", priority = 20)]
    public static void CreateDoComponent()
    {
        string templatePath = "Assets/Plugins/Tween Kit/Editor/Script Template/DOComponent.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "DOComponent.cs");
    }

    [MenuItem("Assets/Create/Tween Kit/DOChildsComponent", priority = 20)]
    public static void CreateDoChildsComponent()
    {
        string templatePath = "Assets/Plugins/Tween Kit/Editor/Script Template/DOChildsComponent.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "DOChildsComponent.cs");
    }

    [MenuItem("Assets/Create/Tween Kit/DOComponentEditor")]
    public static void CreateDoComponentEditor()
    {
        string templatePath = "Assets/Plugins/Tween Kit/Editor/Script Template/DOComponentEditor.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "DOComponentEditor.cs");
    }

    [MenuItem("Assets/Create/Tween Kit/DOChildsComponentEditor")]
    public static void CreateDoChildsComponentEditor()
    {
        string templatePath = "Assets/Plugins/Tween Kit/Editor/Script Template/DOChildsComponentEditor.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "DOChildsComponentEditor.cs");
    }
}

#endif