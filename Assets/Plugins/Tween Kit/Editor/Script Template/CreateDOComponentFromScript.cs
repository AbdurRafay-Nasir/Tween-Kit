#if UNITY_EDITOR

using UnityEditor;

public static class CreateDOComponentFromScript
{
    [MenuItem("Assets/Create/DOTween Modular/DOComponent", priority = 20)]
    public static void CreateDoComponent()
    {
        string templatePath = "Assets/Plugins/DOTween Modular/Editor/Script Template/DOComponent.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "DOComponent.cs");
    }

    [MenuItem("Assets/Create/DOTween Modular/DOChildsComponent", priority = 20)]
    public static void CreateDoChildsComponent()
    {
        string templatePath = "Assets/Plugins/DOTween Modular/Editor/Script Template/DOChildsComponent.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "DOChildsComponent.cs");
    }

    [MenuItem("Assets/Create/DOTween Modular/DOComponentEditor")]
    public static void CreateDoComponentEditor()
    {
        string templatePath = "Assets/Plugins/DOTween Modular/Editor/Script Template/DOComponentEditor.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "DOComponentEditor.cs");
    }

    [MenuItem("Assets/Create/DOTween Modular/DOChildsComponentEditor")]
    public static void CreateDoChildsComponentEditor()
    {
        string templatePath = "Assets/Plugins/DOTween Modular/Editor/Script Template/DOChildsComponentEditor.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "DOChildsComponentEditor.cs");
    }
}

#endif