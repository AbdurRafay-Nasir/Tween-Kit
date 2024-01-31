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

    [MenuItem("Assets/Create/DOTween Modular/DOComponentEditor")]
    public static void CreateDoComponentEditor()
    {
        string templatePath = "Assets/Plugins/DOTween Modular/Editor/Script Template/DOComponentEditor.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "DOComponentEditor.cs");
    }
}

#endif