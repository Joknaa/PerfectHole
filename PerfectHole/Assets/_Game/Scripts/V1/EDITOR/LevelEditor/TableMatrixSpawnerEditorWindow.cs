#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class TableMatrixSpawnerEditorWindow : OdinMenuEditorWindow {
    private string UPGRADESPATH = "Assets/_Game/LevelsData";
    private Texture2D editorIcon;
    private string creditText = "Level Editor v0.11 stable - WarLordz";

    [MenuItem("Proto/Level Editor  %#E")]
    public static void ShowWindow() {
        TableMatrixSpawnerEditorWindow window = GetWindow<TableMatrixSpawnerEditorWindow>();

        window.position = new Rect(Screen.currentResolution.width / 2f - 400f / 2f,
            Screen.currentResolution.height / 2f - 300f / 2f,
            500,
            1100);
        window.MenuWidth = -10;
        window.ResizableMenuWidth = false;
    }

    protected override OdinMenuTree BuildMenuTree() {
        var tree = new OdinMenuTree();

        tree.AddAllAssetsAtPath("", UPGRADESPATH, typeof(TableMatrixSpawner));

        return tree;
    }

    protected override void Initialize() {
        // get the local path of this script
        string localPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
        // get the path of the folder this script is in
        string basePath = System.IO.Path.GetDirectoryName(localPath);
        // get the path of the image relative to this script
        string imagePath = basePath + "/Icons/LevelEditorText.png";
        // string imagePath = "Assets/Editor/Icons/LevelEditorText.png";
        editorIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(imagePath);
    }

    protected override void DrawEditors() {
        GUILayout.Space(10f);


        Rect imageRect = GUILayoutUtility.GetRect(position.width * .1f, position.height * 0.1f);
        float aspectRatio = (float)editorIcon.width / editorIcon.height;
        float scaledHeight = imageRect.width / aspectRatio * 0.6f;
        imageRect.height = scaledHeight;

        GUI.DrawTexture(imageRect, editorIcon, ScaleMode.ScaleToFit, true);

        base.DrawEditors();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(creditText);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
#endif