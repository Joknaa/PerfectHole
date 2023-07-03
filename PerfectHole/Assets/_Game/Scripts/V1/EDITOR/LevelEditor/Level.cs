using OknaaEXTENSIONS;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static OknaaEXTENSIONS.GameObjectExtensions;

public class Level : SerializedMonoBehaviour {
    [PropertyOrder(-1)] public int rows = 10;
    [PropertyOrder(-1)] public int columns = 10;

    public GameObject AgentParent;

    // public List<OrderItem> OrderItem = new List<OrderItem>(0); // Availble Stands 


    [Title("Important Tile References", TitleAlignment = TitleAlignments.Centered)]
    public List<Transform> engineersSpawnPoint = new List<Transform>();

    public List<Transform> deliverySpawnPoint = new List<Transform>();
    public List<Transform> solidersSpawnPoint = new List<Transform>();
    public List<Transform> zombiesSpawnPoint = new List<Transform>();
    public List<Transform> targetWalls = new List<Transform>();

    [HideInInspector] public GameObject[,] levelMatrix;
    [HideInInspector] public GameObject[,] playerGameobjectsMatrix;
    [HideInInspector] public Color[,] playerPreviewColors;
    [HideInInspector] public float[,] playerRotationMatrix;

    private bool isInitialized;
    // private bool isLevelReferencesRefreshed;


    [Button(name: nameof(RefreshTileReferences),
        buttonSize: ButtonSizes.Large,
        ButtonStyle.FoldoutButton,
        Icon = SdfIconType.ArrowClockwise,
        IconAlignment = IconAlignment.LeftOfText,
        Name = "Refresh References")]
    [GUIColor(0.4f, 0.8f, 1)]
    [PropertySpace(10, 0)]
    public void RefreshTileReferences() {
    }

    public void Init() {
        if (isInitialized) return;

        isInitialized = true;
    }

    public void Dispose() => isInitialized = false;
}