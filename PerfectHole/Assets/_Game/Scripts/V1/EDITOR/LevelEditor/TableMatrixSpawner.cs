#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Path = System.IO.Path;

public class TableMatrixSpawner : SerializedScriptableObject {
    [TableMatrix(HorizontalTitle = "Blueprint", DrawElementMethod = "DrawGameObjectElement", ResizableColumns = true, SquareCells = true, RowHeight = 30, RespectIndentLevel = true,
        IsReadOnly = false)]
    public GameObject[,] levelMatrix;

    [SerializeField, Tooltip(" ONLY FOR DEBUGING PURPOSES , IT SHOULD BE OFF .")] bool debug = false;

    [TableMatrix(ResizableColumns = true, SquareCells = true, RowHeight = 30)]
    [ShowIf("debug")] public float[,] playerRotationMatrix;

    [TableMatrix(ResizableColumns = true, SquareCells = true, RowHeight = 30),]
    [ShowIf("debug")] public GameObject[,] playerGameobjectsMatrix;

    [ShowIf("debug")] public Color[,] playerPreviewColors;

    [SerializeField, Range(10, 50), PropertyOrder(-1)]
    [Tooltip("Update Matrix Button To Apply New Changes")]
    private int columns = 10;

    [SerializeField, Range(10, 50), PropertyOrder(-1)]
    [Tooltip("Update Matrix Button To Apply New Changes")]
    private int rows = 10;

    [SerializeField, PropertyOrder(0), BoxGroup("Preset"), LabelText("Level To Load / Levels ")] private Level levelToLoad;

    [SerializeField]
    private float gridCellSize = 1f;

    private float gridSpacing = 1f;

    //[SerializeField, LabelText("Add Grass In Empty Tiles?")] private bool addGrassInEmptyTiles = true;
    [SerializeField, LabelText("Add Walls To the Level?")] private bool drawWalls = true;
    [SerializeField, LabelText("Close Level Editor After Generating a Level?")] private bool AutoCloseLevelEditor = true;

    [SerializeField, ReadOnly]
    [Tooltip("Only For Debug...")]
    private GameObject selectedPrefab;


    #region Prefabs

    [FoldoutGroup("Prefabs")] public GameObject fabriqueArmeFloorPrefab; // lab floor

    [FoldoutGroup("Prefabs")] public GameObject normalFloorPrefab; // the main area floor 
    [FoldoutGroup("Prefabs")] public GameObject emptyStandPrefab; // just a useless empty stand

    [FoldoutGroup("Prefabs")] public GameObject deliveryZonePrefab; // a contacting point between the enginners and delivery soliders
    [FoldoutGroup("Prefabs")] public GameObject wallPrefab; // dummy wall for the current version
    [FoldoutGroup("Prefabs")] public GameObject zombieZoneWallPrefab; // dummy wall for the current version

    [FoldoutGroup("Prefabs")] public GameObject zombieZoneFloorPrefab; // floor where zombie will spawn, (grassFloor)
    [FoldoutGroup("Prefabs")] public GameObject zombieZoneGatePrefab; // zombie zone gate

    [FoldoutGroup("Prefabs")] public GameObject soliderSeats; // customers seats
    //[FoldoutGroup("Prefabs")] public GameObject gate; // Zombie Gate

    // Spawn pointer
    [FoldoutGroup("Prefabs")] public GameObject engineersSpawnPoint; // cooks
    [FoldoutGroup("Prefabs")] public GameObject deliverySpawnPoint; // waiters
    [FoldoutGroup("Prefabs")] public GameObject solidersSpawnPoint; // customers
    [FoldoutGroup("Prefabs")] public GameObject zombiesSpawnPoint; // cooks

    #endregion

    // require stand names

    #region Stands

    [Title("Stand Prefabs", TitleAlignment = TitleAlignments.Centered)]
    [FoldoutGroup("Stands")] public GameObject HandgunStandPrefab;

    [FoldoutGroup("Stands")] public GameObject ShotgunStandPrefab;
    [FoldoutGroup("Stands")] public GameObject SMGStandPrefab;
    [FoldoutGroup("Stands")] public GameObject AkStandPrefab;

    [FoldoutGroup("Stands")] public GameObject M249StandPrefab;
    //[FoldoutGroup("Stands")] public GameObject PB_Stand_Burger;
    //[FoldoutGroup("Stands")] public GameObject PB_Stand_Fries;
    //[FoldoutGroup("Stands")] public GameObject PB_Stand_Cocktail_01;

    // upgraders :
    [Title("Upgrader Prefabs", TitleAlignment = TitleAlignments.Centered)]
    [FoldoutGroup("Stands")] public GameObject HandgunStandUpgraderPrefab;

    [FoldoutGroup("Stands")] public GameObject ShotgunStandUpgraderPrefab;
    [FoldoutGroup("Stands")] public GameObject SMGStandUpgraderPrefab;
    [FoldoutGroup("Stands")] public GameObject AkStandUpgraderPrefab;

    [FoldoutGroup("Stands")] public GameObject M249StandUpgraderPrefab;
    //[FoldoutGroup("Stands")] public GameObject StandUpgrader_Fries;
    //[FoldoutGroup("Stands")] public GameObject StandUpgrader_StandCocktail_01;
    //[FoldoutGroup("Stands")] public GameObject StandUpgrader_Burger;
    //[FoldoutGroup("Stands")] public GameObject DJ_Stand_Prefab;

    #endregion

    // icons for the editor

    #region Icons

    [FoldoutGroup("Icons")] public Sprite table1x1Icon;
    [FoldoutGroup("Icons")] public Sprite table2x2Icon;
    [FoldoutGroup("Icons")] public Sprite customerSeatIcon;

    [FoldoutGroup("Icons")] public Sprite idleSpotsIcon;
    [FoldoutGroup("Icons")] public Sprite spawnPointIcon;

    [FoldoutGroup("Icons")] public Sprite arrowIcon;

    #endregion

    // editor colors

    #region Colors

    [FoldoutGroup("Colors")] public Color FabriqueArmeColor = new(0.1f, 0.8f, 0.2f);
    [FoldoutGroup("Colors")] public Color EmptyStandColor = new(0.2f, 0.4f, 0.8f);
    [FoldoutGroup("Colors")] public Color NormalFloorColor = new(0.2f, 0.4f, 0.8f);
    [FoldoutGroup("Colors")] public Color wallColor = new(0.2f, 0.4f, 0.8f);
    [FoldoutGroup("Colors")] public Color grassColor = new(0.2f, 0.4f, 0.8f);
    [FoldoutGroup("Colors")] public Color StandsColor = new(0.2f, 0.4f, 0.8f);

    #endregion

    #region privates

    [ReadOnly] private List<GameObject> zombieZoneTiles = new List<GameObject>();

    #endregion

    public Level GeneratedLevel;

    [OnValueChanged("UpdateSelectedPrefab")]
    public bool PaintStands;

    [EnumToggleButtons]
    [OnValueChanged("UpdateSelectedPrefab")]
    [HideIf("PaintStands")]
    //[TabGroup("tab1", "Brush Tool", SdfIconType.ImageAlt, TextColor = "green",)]
    public ObjectPrefabType brushTool;

    [OnValueChanged("UpdateSelectedPrefab")]
    [EnumPaging] public KitchenStands standsTool;

    //[TabGroup("tab1", "Brush Tool", SdfIconType.ImageAlt, TextColor = "green")]
    [ShowIf("PaintStands")]
    public enum KitchenStands {
        HandgunStand,
        HandgunStandUpgrader,
        ShotgunStand,
        ShotgunStandUpgrader,
        SMGStand,
        SMGStandUpgrader,
        AkStand,
        AkStandUpgrader,
        M249Stand,
        M249StandUpgrader,
        //Cocktail01Stand,
        //StandCocktail01StandUpgrader,
        //FriesStand,
        //FriesStandUpgrader,
        //BurgerStand,
        //BurgerStandUpgrader,
    }

    public enum ObjectPrefabType {
        [LabelText(SdfIconType.Shop)] fabriqueArmeFloor,
        [LabelText(SdfIconType.SunFill)] normalFloor,
        [LabelText(SdfIconType.EmojiAngryFill)] zombieZoneFloor,
        [LabelText(SdfIconType.CupFill)] soliderSeat,
        [LabelText(SdfIconType.DoorClosed)] gate,
        [LabelText(SdfIconType.EnvelopeExclamation1)] emptyStand,
        [LabelText(SdfIconType.GripHorizontal)] deliveryZone,
        [LabelText(SdfIconType.GeoAltFill)] enginnersSpawnPoint,
        [LabelText(SdfIconType.GeoAltFill)] deliverySpawnPoint,
        [LabelText(SdfIconType.GeoAltFill)] solidersSpawnPoint,
        [LabelText(SdfIconType.GeoAltFill)] zombiesSpawnPoint,
    }

    [OnInspectorInit]
    private void UpdateSelectedPrefab() {
        if (PaintStands)
            switch (standsTool) {
                case KitchenStands.HandgunStand:
                    selectedPrefab = HandgunStandPrefab;
                    break;
                case KitchenStands.M249Stand:
                    selectedPrefab = M249StandPrefab;
                    break;
                case KitchenStands.AkStand:
                    selectedPrefab = AkStandPrefab;
                    break;
                case KitchenStands.SMGStand:
                    selectedPrefab = SMGStandPrefab;
                    break;
                case KitchenStands.ShotgunStand:
                    selectedPrefab = ShotgunStandPrefab;
                    break;
                case KitchenStands.HandgunStandUpgrader:
                    selectedPrefab = HandgunStandUpgraderPrefab;
                    break;
                case KitchenStands.M249StandUpgrader:
                    selectedPrefab = M249StandUpgraderPrefab;
                    break;
                case KitchenStands.AkStandUpgrader:
                    selectedPrefab = AkStandUpgraderPrefab;
                    break;
                case KitchenStands.SMGStandUpgrader:
                    selectedPrefab = SMGStandUpgraderPrefab;
                    break;
                case KitchenStands.ShotgunStandUpgrader:
                    selectedPrefab = ShotgunStandUpgraderPrefab;
                    break;
                //case KitchenStands.BurgerStand:
                //    selectedPrefab = PB_Stand_Burger;
                //    break;
                //case KitchenStands.Cocktail01Stand:
                //    selectedPrefab = PB_Stand_Cocktail_01;
                //    break;
                //case KitchenStands.FriesStand:
                //    selectedPrefab = PB_Stand_Fries;
                //    break;
                //case KitchenStands.BurgerStandUpgrader:
                //    selectedPrefab = StandUpgrader_Burger;
                //    break;
                //case KitchenStands.FriesStandUpgrader:
                //    selectedPrefab = StandUpgrader_Fries;
                //    break;
                //case KitchenStands.StandCocktail01StandUpgrader:
                //    selectedPrefab = StandUpgrader_StandCocktail_01;
                //    break;
            }
        else
            switch (brushTool) {
                case ObjectPrefabType.fabriqueArmeFloor:
                    selectedPrefab = fabriqueArmeFloorPrefab;
                    break;
                case ObjectPrefabType.normalFloor:
                    selectedPrefab = normalFloorPrefab;
                    break;
                case ObjectPrefabType.emptyStand:
                    selectedPrefab = emptyStandPrefab;
                    break;
                case ObjectPrefabType.soliderSeat:
                    selectedPrefab = soliderSeats;
                    break;
                case ObjectPrefabType.zombieZoneFloor:
                    selectedPrefab = zombieZoneFloorPrefab;
                    break;
                case ObjectPrefabType.gate:
                    selectedPrefab = zombieZoneGatePrefab;
                    break;
                case ObjectPrefabType.enginnersSpawnPoint:
                    selectedPrefab = engineersSpawnPoint;
                    break;
                case ObjectPrefabType.deliverySpawnPoint:
                    selectedPrefab = deliverySpawnPoint;
                    break;
                case ObjectPrefabType.solidersSpawnPoint:
                    selectedPrefab = solidersSpawnPoint;
                    break;
                case ObjectPrefabType.deliveryZone:
                    selectedPrefab = deliveryZonePrefab;
                    break;
                case ObjectPrefabType.zombiesSpawnPoint:
                    selectedPrefab = zombiesSpawnPoint;
                    break;
            }
    }

    #region Core

    [ButtonGroup("Group1")]
    [Button("Spawn", ButtonSizes.Large, Icon = SdfIconType.PlayFill)]
    public void GenerateLevel() {
        DeleteAllChildren();
        FindLevelGameobject();

        zombieZoneTiles.Clear();
        // init

        // Initialize the wall matrix
        GameObject[,] wallMatrix = new GameObject[columns, rows];
        GameObject[,] ZombieWallsMatrix = new GameObject[columns, rows];

        // Spawn the level objects and walls
        for (int row = 0; row < columns; row++) {
            for (int col = 0; col < rows; col++) {
                GameObject prefab = levelMatrix[row, col];
                GameObject secondMatrixPrefabs = playerGameobjectsMatrix[row, col];
                float rotation = playerRotationMatrix[row, col];

                Vector3 position = CalculateSpawnPosition(row, col);

                if (prefab == null && false) {
                    GameObject spawnedGrassObject = PrefabUtility.InstantiatePrefab(zombieZoneFloorPrefab, GeneratedLevel.transform) as GameObject;
                    spawnedGrassObject.transform.position = position;
                }
                else if (prefab != null) {
                    HandleSpawnPoints(prefab, position);
                    if (prefab != solidersSpawnPoint && prefab != deliverySpawnPoint && prefab != engineersSpawnPoint) {
                        GameObject spawnedObject = PrefabUtility.InstantiatePrefab(prefab, GeneratedLevel.transform) as GameObject;
                        if (prefab != fabriqueArmeFloorPrefab && prefab != normalFloorPrefab) {
                            spawnedObject.transform.SetPositionAndRotation(position, Quaternion.Euler(0f, rotation, 0f));
                        }
                        else spawnedObject.transform.position = position;

                        if (prefab == zombiesSpawnPoint) GeneratedLevel.zombiesSpawnPoint.Add(spawnedObject.transform);
                        if ((prefab == zombieZoneFloorPrefab || prefab == zombieZoneGatePrefab) && spawnedObject) {
                            zombieZoneTiles.Add(spawnedObject);
                            HandleZombieZoneWalls(ZombieWallsMatrix, row, col);
                        }

                        if (prefab == zombieZoneGatePrefab) {
                            GeneratedLevel.targetWalls.Add(spawnedObject.transform);
                        }

                    }

                    if ((prefab == soliderSeats || prefab == zombieZoneGatePrefab || prefab == solidersSpawnPoint || prefab == deliverySpawnPoint || prefab == engineersSpawnPoint)) {
                        if (secondMatrixPrefabs == null) continue;
                        if (prefab == secondMatrixPrefabs) continue;

                        GameObject gSpawnedObject = PrefabUtility.InstantiatePrefab(secondMatrixPrefabs, GeneratedLevel.transform) as GameObject;
                        //if (prefab == )
                        gSpawnedObject.name = "gSpawnedObject " + gSpawnedObject.name;
                        gSpawnedObject.transform.position = position;
                    }

                    if (drawWalls) HandleWallsGeneration(row, col, wallMatrix);
                }
            }
        }

        GameObject Agents = new("Agents");
        Agents.tag = "AgentsParent";
        Agents.transform.parent = GeneratedLevel.transform;
        Agents.transform.SetAsFirstSibling();
        GeneratedLevel.AgentParent = Agents;


        GeneratedLevel.transform.localRotation = Quaternion.Euler(0f, 90, 0f);
        SaveMatrixDataToLevel();
    }
    // drawMatrix- start

    // one of the most important methods : it's resposible to draw the matrix in other words it's the MATRIX 
    private EventType eventType;

    private Vector2 mousePosition;

    // private Color previousColor = Color.clear;
    private GameObject DrawGameObjectElement(Rect rect, GameObject obj, int col, int row) {
        Color previousColor = GetRectColor(obj, playerPreviewColors[col, row]); // Initialize with a default value

        eventType = Event.current.type;
        mousePosition = Event.current.mousePosition;

        if (eventType == EventType.MouseDown && rect.Contains(mousePosition)) {
            HandleMouseDownEvent(rect, ref obj, col, row, previousColor);
        }
        else if (eventType == EventType.MouseDrag && rect.Contains(mousePosition)) {
            HandleMouseDragEvent(rect, ref obj, col, row);
        }

        Draw(rect, obj, col, row);

        return obj;
    }

    private void HandleMouseDownEvent(Rect rect, ref GameObject obj, int col, int row, Color previousColor) {
        if (Event.current.button == 0) // Left mouse button
        {
            // previousColor = GetRectColor(obj, playerPreviewColors[col, row]);
            // Toggle the GameObject's selection state
            if (obj != null) {
                if (obj == soliderSeats || obj == zombieZoneGatePrefab) {
                    RotatePlayerPrefab(ref obj, col, row);
                }
                else if (IsExcludedObject(obj)) {
                    obj = GetGameObjectFromPrefab(selectedPrefab);
                    RotatePlayerPrefab(ref obj, col, row);
                }
                else {
                    if (IsExcludedObject(obj))
                        playerGameobjectsMatrix[col, row] = obj;
                }
            }
            else {
                obj = selectedPrefab;
                if (IsExcludedObject(obj))
                    playerGameobjectsMatrix[col, row] = obj;
            }

            if (previousColor != null) playerPreviewColors[col, row] = previousColor;
        }
        else if (Event.current.button == 1) // Right mouse button
        {
            // Erase the spawned tile
            obj = null;
            if (IsExcludedObject(obj))
                playerGameobjectsMatrix[col, row] = obj;
        }

        GUI.changed = true;
        Event.current.Use();
    }

    private void HandleMouseDragEvent(Rect rect, ref GameObject obj, int col, int row) {
        if (obj == null) {
            obj = selectedPrefab;
            if (IsExcludedObject(obj))
                playerGameobjectsMatrix[col, row] = obj;
        }

        if (Event.current.button == 1) // Right mouse button
        {
            // Erase the spawned tile
            obj = null;
            if (IsExcludedObject(obj))
                playerGameobjectsMatrix[col, row] = obj;
        }

        GUI.changed = true;
        Event.current.Use();
    }

    private void RotatePlayerPrefab(ref GameObject obj, int col, int row) {
        float rotation = playerRotationMatrix[col, row];
        rotation += 90f;
        rotation %= 360f;
        playerRotationMatrix[col, row] = rotation;
    }

    private void Draw(Rect rect, GameObject obj, int col, int row) {
        if (IsExcludedObject(obj)) {
            Color rectColor = GetRectColor(obj, playerPreviewColors[col, row]);
            EditorGUI.DrawRect(rect.Padding(1), rectColor);

            if (obj == HandgunStandPrefab
                || obj == deliveryZonePrefab
                || obj == zombiesSpawnPoint
                || obj == wallPrefab
                || obj == M249StandPrefab
                || obj == AkStandPrefab
                || obj == SMGStandPrefab
                || obj == ShotgunStandPrefab
                || obj == HandgunStandUpgraderPrefab
                || obj == SMGStandUpgraderPrefab
                || obj == ShotgunStandUpgraderPrefab
                || obj == AkStandUpgraderPrefab
                || obj == M249StandUpgraderPrefab
                //|| obj == PB_Stand_Fries
                //|| obj == StandUpgrader_Burger
                //|| obj == PB_Stand_Burger
                //|| obj == PB_Stand_Cocktail_01
                //|| obj == StandUpgrader_Fries
                //|| obj == StandUpgrader_StandCocktail_01
                && obj != null) {
                DrawRotatingGameObject(rect, playerRotationMatrix[col, row], arrowIcon.texture);
            }
        }
        else if (obj == soliderSeats || obj == zombieZoneGatePrefab) {
            Color rectPlayerColor = playerPreviewColors[col, row];
            EditorGUI.DrawRect(rect.Padding(1), rectPlayerColor);
            DrawRotatingGameObject(rect, playerRotationMatrix[col, row], arrowIcon.texture, true);
        }
        else if (obj == solidersSpawnPoint || obj == deliverySpawnPoint || obj == engineersSpawnPoint) {
            DrawGameObject(rect, playerPreviewColors[col, row], spawnPointIcon.texture);
        }
    }

    // a quick check
    private bool IsExcludedObject(GameObject obj) {
        return obj != soliderSeats
               && obj != zombieZoneGatePrefab
               && obj != solidersSpawnPoint
               && obj != deliverySpawnPoint
               && obj != engineersSpawnPoint;
    }

    // DRAW THE GAMEOBJECT AS A RECT WITH GUI TOOLKIT
    void DrawGameObject(Rect rect, Color rectPlayer, Texture2D iconTexture) {
        // Get the stored preview color for this tile
        Color rectPlayerColor = rectPlayer;
        EditorGUI.DrawRect(rect.Padding(1), rectPlayerColor);
        // Calculate the position and size of the player icon
        Rect iconRect = new(rect.x + (rect.width * 0.25f), rect.y + (rect.height * 0.25f), rect.width * 0.5f, rect.height * 0.5f);
        // Draw the player icon
        GUI.DrawTexture(iconRect, iconTexture, ScaleMode.ScaleToFit, true);
    }

    // ROTATION STAFF
    void DrawRotatingGameObject(Rect rect, float rotation, Texture2D iconTexture, bool isSeat = false) {
        // Save the current matrix
        Matrix4x4 matrix = GUI.matrix;
        // Calculate the position and size of the player icon
        Vector2 pivot = new Vector2(rect.x + (rect.width * 0.5f), rect.y + (rect.height * 0.5f));
        GUIUtility.RotateAroundPivot(rotation, pivot);
        Rect iconRect = new Rect(rect.x + (rect.width * 0.25f), rect.y + (rect.height * 0.25f), rect.width * 0.5f, rect.height * 0.5f);

        if (isSeat) GUI.DrawTexture(iconRect, customerSeatIcon.texture, ScaleMode.ScaleToFit, true);
        // Draw the player icon with the rotated matrix
        GUI.DrawTexture(iconRect, iconTexture, ScaleMode.ScaleToFit, true);
        // Restore the original matrix
        GUI.matrix = matrix;
    }

    GameObject GetGameObjectFromPrefab(GameObject objectPrefab) {
        GameObject obj = null;
        if (objectPrefab == soliderSeats) {
            obj = soliderSeats;
        }

        if (objectPrefab == zombieZoneGatePrefab) {
            obj = zombieZoneGatePrefab;
        }
        else if (objectPrefab == solidersSpawnPoint) {
            obj = solidersSpawnPoint;
        }
        else if (objectPrefab == deliverySpawnPoint) {
            obj = deliverySpawnPoint;
        }
        else if (objectPrefab == engineersSpawnPoint) {
            obj = engineersSpawnPoint;
        }

        return obj;
    }

    // HANDLING THE COLORING - GET THE COLOR BASED ON THE PREFAB
    private Color GetRectColor(GameObject obj, Color rectPlayer) {
        if (obj == null) {
            return Color.clear;
        }
        else if (obj == fabriqueArmeFloorPrefab) {
            return FabriqueArmeColor;
        }
        else if (obj == emptyStandPrefab) {
            return EmptyStandColor;
        }
        else if (obj == normalFloorPrefab) {
            return NormalFloorColor;
        }
        else if (obj == wallPrefab) {
            return wallColor;
        }
        else if (obj == zombieZoneFloorPrefab) {
            return grassColor;
        }
        else if (obj == soliderSeats || obj == zombieZoneGatePrefab) {
            return rectPlayer;
        }
        else {
            return StandsColor;
        }
    }

    // drawMatrix - end

    #endregion

    #region Systems

    // wall Generation - start
    private void HandleWallsGeneration(int row, int col, GameObject[,] wallMatrix) {
        // Check if there are adjacent empty spaces
        bool hasEmptySpaceLeft = col > 0 && levelMatrix[row, col - 1] == null;
        bool hasEmptySpaceRight = col < rows - 1 && levelMatrix[row, col + 1] == null;
        bool hasEmptySpaceUp = row > 0 && levelMatrix[row - 1, col] == null;
        bool hasEmptySpaceDown = row < columns - 1 && levelMatrix[row + 1, col] == null;

        // Generate walls based on the adjacent empty spaces
        if (hasEmptySpaceLeft) {
            GameObject wall = HandleWallsSpawning(row, col - 1, "Left", 90, wallPrefab, wallMatrix);
            wall.transform.localPosition = new(wall.transform.localPosition.x + 0.5f, wall.transform.localPosition.y, wall.transform.localPosition.z);
        }

        if (hasEmptySpaceRight) {
            GameObject wall = HandleWallsSpawning(row, col + 1, "Right", -90, wallPrefab, wallMatrix);
            wall.transform.localPosition = new(wall.transform.localPosition.x - 0.5f, wall.transform.localPosition.y, wall.transform.localPosition.z);
        }

        if (hasEmptySpaceUp) {
            GameObject wall = HandleWallsSpawning(row - 1, col, "Up", 180, wallPrefab, wallMatrix);
            wall.transform.localPosition = new(wall.transform.localPosition.x, wall.transform.localPosition.y, wall.transform.localPosition.z + 0.5f);
        }

        if (hasEmptySpaceDown) {
            GameObject wall = HandleWallsSpawning(row + 1, col, "Down", -180, wallPrefab, wallMatrix);
            wall.transform.localPosition = new(wall.transform.localPosition.x, wall.transform.localPosition.y, wall.transform.localPosition.z - 0.5f);
        }
    }

    private GameObject HandleWallsSpawning(int row, int col, string direction, float rotationAngle, GameObject prefab, GameObject[,] wallMatrix) {
        GameObject spawnedWall = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        if (prefab == null) Debug.Log("<color=red> Wall Prefab Is Null Please Assign a  </color>" + prefab.name);

        if (spawnedWall == null) return null;
        Vector3 position = CalculateSpawnPosition(row, col);
        spawnedWall.transform.position = position;
        spawnedWall.name = "Wall " + direction;
        spawnedWall.transform.SetParent(GeneratedLevel.transform);
        HandleWallsRotations(spawnedWall, rotationAngle);
        wallMatrix[row, col] = spawnedWall;
        return spawnedWall;
    }

    private void HandleWallsRotations(GameObject wallObject, float rotationAngle) {
        Vector3 rotation = wallObject.transform.rotation.eulerAngles;
        rotation.y = rotationAngle;
        wallObject.transform.rotation = Quaternion.Euler(rotation);
    }

    private void HandleZombieZoneWalls(GameObject[,] wallMatrix, int col, int row) {
        var gateAndWallFacingSameDirection = levelMatrix[col, row] == zombieZoneGatePrefab && (int)playerRotationMatrix[col, row] == 270;
        var canSpawnDown = row < columns - 1 && IsOneOf(levelMatrix[col, row + 1], normalFloorPrefab, soliderSeats) && !gateAndWallFacingSameDirection;
        var canSpawnUp = row > 0 && IsOneOf(levelMatrix[col, row - 1], normalFloorPrefab, soliderSeats);
        var canSpawnLeft = col > 0 && IsOneOf(levelMatrix[col - 1, row], normalFloorPrefab, soliderSeats);
        var canSpawnRight = col < rows - 1 && IsOneOf(levelMatrix[col + 1, row], normalFloorPrefab, soliderSeats);

        // Check if surrounding walls are needed
        if (canSpawnLeft) {
            GameObject spawnedWall = HandleWallsSpawning(col, row - 1, "Left", 0, zombieZoneWallPrefab, wallMatrix);
            if (spawnedWall) {
                var localPosition = spawnedWall.transform.localPosition;
                localPosition = new Vector3(localPosition.x + 1, localPosition.y, localPosition.z - 0.5f);
                spawnedWall.transform.localPosition = localPosition;
            }
        }

        if (canSpawnRight) {
            // right walls
            GameObject spawnedWall = HandleWallsSpawning(col, row + 1, "Right", 180, zombieZoneWallPrefab, wallMatrix);
            if (spawnedWall) spawnedWall.transform.localPosition = new(spawnedWall.transform.localPosition.x - 1, spawnedWall.transform.localPosition.y, spawnedWall.transform.localPosition.z + 0.5f);
        }

        if (canSpawnUp) {
            // down walls
            GameObject spawnedWall = HandleWallsSpawning(col + 1, row, "Up", 90, zombieZoneWallPrefab, wallMatrix);
            if (spawnedWall) spawnedWall.transform.localPosition = new(spawnedWall.transform.localPosition.x - 0.5f, spawnedWall.transform.localPosition.y, spawnedWall.transform.localPosition.z - 1);
        }

        if (canSpawnDown) {
            // up walls
            GameObject spawnedWall = HandleWallsSpawning(col - 1, row, "zombieZoneWallPrefab", 90, zombieZoneWallPrefab, wallMatrix);
            if (spawnedWall) spawnedWall.transform.localPosition = new(spawnedWall.transform.localPosition.x + .5f, spawnedWall.transform.localPosition.y, spawnedWall.transform.localPosition.z + 1);
        }

        bool IsOneOf(GameObject thisTile, params GameObject[] targetTiles) {
            return targetTiles.Any(targetTile => thisTile == targetTile);
        }
    }

    // wall Generation - end

    #endregion

    #region Helper Functions

    private Vector3 CalculateSpawnPosition(int row, int col) {
        float xOffset = col * (gridCellSize + gridSpacing);
        float yOffset = row * (gridCellSize + gridSpacing);
        return new Vector3(xOffset, 0, yOffset);
    }

    private void HandleSpawnPoints(GameObject prefab, Vector3 position) {
        if (prefab == engineersSpawnPoint) {
            GeneratedLevel.engineersSpawnPoint.Add(Instantiate(prefab, position, Quaternion.identity, GeneratedLevel.transform).transform);
        }
        else if (prefab == solidersSpawnPoint) {
            GeneratedLevel.solidersSpawnPoint.Add(Instantiate(prefab, position, Quaternion.identity, GeneratedLevel.transform).transform);
        }
        else if (prefab == deliverySpawnPoint) {
            GeneratedLevel.deliverySpawnPoint.Add(Instantiate(prefab, position, Quaternion.identity, GeneratedLevel.transform).transform);
        }
    }

    private void FindLevelGameobject() {
        if (!GeneratedLevel) {
            GeneratedLevel = new GameObject("Level", typeof(Level)) {
                tag = "Level"
            }.GetComponent<Level>();
        }
    }

    [ButtonGroup("Group1")]
    [Button("Reset Data", ButtonSizes.Large, Icon = SdfIconType.ArrowRepeat)]
    private void ResetMatrixData() // ERASE the matrix and start over 
    {
        DeleteAllChildren();
        UpdateMatrix();
    }

    [ButtonGroup("Group1")]
    [Button("Update Matrix", ButtonSizes.Large, Icon = SdfIconType.Border)]
    private void UpdateMatrix() {
        levelMatrix = new GameObject[columns, rows];
        playerGameobjectsMatrix = new GameObject[columns, rows];
        playerPreviewColors = new Color[columns, rows];
        playerRotationMatrix = new float[columns, rows];
        DeleteAllChildren();
    }

    [ButtonGroup("Group1")]
    [Button("Delete", ButtonSizes.Large, Icon = SdfIconType.ArrowRepeat)]
    private void DeleteAllChildren() {
        FindLevelGameobject();
        DestroyImmediate(GeneratedLevel.gameObject);
    }

    #endregion

    #region SAVE AND LOAD


    // create a unique scriptable object File Holding All the DATA and export it to the folderPath Level
    // it's also doin' alot of checks to make sure it's create in in a valid place
    private void GenerateLevelTweakable(out string newFolderPath, out string folderName) {
        AssetDatabase.Refresh();

        // Get the existing folder count
        string folderPath = "Assets/_Game/Tweakables/Levels";
        string[] existingFolders = Directory.GetDirectories(folderPath);
        int existingFolderCount = existingFolders.Length;

        Debug.Log(existingFolderCount);

        // Find the next available folder number
        int nextFolderNumber = 1;
        for (int i = 1; i <= existingFolderCount + 1; i++) {
            string folderNumber = i.ToString("D2");
            if (!Array.Exists(existingFolders, folder => folder.EndsWith(folderNumber))) {
                nextFolderNumber = i;
                break;
            }
        }

        // Format the folder name with leading zeros
        folderName = "Level " + nextFolderNumber.ToString("D2");

        // Create the new folder path
        newFolderPath = Path.Combine(folderPath, folderName);

        // Check if the new folder already exists
        bool folderExists = Directory.Exists(newFolderPath);

        // If the folder does not exist or has been deleted, recreate it
        if (!folderExists) {
            // Create the new folder
            Directory.CreateDirectory(newFolderPath);

            // Create the 'Upgrades' directory within the new folder
            string upgradesFolderPath = Path.Combine(newFolderPath, "Upgrades");
            Directory.CreateDirectory(upgradesFolderPath);

            // Save the LevelTweakables instance to the new folder
        }

        // Close the current editor window
        UnityEditor.EditorWindow editorWindow = UnityEditor.EditorWindow.focusedWindow;
        if (editorWindow != null && AutoCloseLevelEditor) {
            editorWindow.Close();
        }

        // Open the Tweakable Editor window
    }

    private void SaveMatrixDataToLevel() {
        GeneratedLevel.rows = columns;
        GeneratedLevel.columns = rows;

        GeneratedLevel.levelMatrix = levelMatrix;
        GeneratedLevel.playerGameobjectsMatrix = playerGameobjectsMatrix;
        GeneratedLevel.playerPreviewColors = playerPreviewColors;
        GeneratedLevel.playerRotationMatrix = playerRotationMatrix;
        //GeneratedLevel.zombiesSpawnPoint = zombiesSpawnPointList;
    }

    // Loading an already existing Level - useful to generate levels faster
    [ShowIf("levelToLoad")]
    [Button("Load Level Data"), PropertyOrder(-0), BoxGroup("Preset")]
    public void LoadLevelData() {
        Level level = levelToLoad;
        // SquareCelledMatrix = 
        columns = level.rows;
        rows = level.columns;

        levelMatrix = new GameObject[columns, rows];
        playerGameobjectsMatrix = new GameObject[columns, rows];
        playerPreviewColors = new Color[columns, rows];

        for (int i = 0; i < level.rows; i++) {
            for (int j = 0; j < level.columns; j++) {
                levelMatrix[i, j] = level.levelMatrix[i, j];
                playerGameobjectsMatrix[i, j] = level.playerGameobjectsMatrix[i, j];
                playerPreviewColors[i, j] = level.playerPreviewColors[i, j];
                playerRotationMatrix[i, j] = level.playerRotationMatrix[i, j];
            }
        }

        levelToLoad = null;
    }

    [Button("Generate Level Prefab", ButtonSizes.Large, Icon = SdfIconType.Save, ButtonHeight = 50)]
    private void HandleSaving() {
        // Check if the matrix is empty
        bool isEmpty = true;
        if (levelMatrix != null) {
            foreach (GameObject obj in levelMatrix) {
                if (obj != null) {
                    isEmpty = false;
                    break;
                }
            }
        }

        if (isEmpty) {
            Debug.LogWarning("<color=blue><b> Cannot generate level prefab. Matrix data is empty. </b></color>");
            EditorApplication.Beep();
            return;
        }

        // Delete any existing "Level" prefab
        GameObject existingPrefab = GameObject.Find("Level");
        if (existingPrefab != null) {
            DestroyImmediate(existingPrefab);
        }

        // Create the level based on the matrix data
        GenerateLevel();
        GenerateLevelTweakable(out string path, out string foldername);
        SaveMatrixDataToLevel();

        string prefabPath = path + "/" + foldername + ".prefab";

        // Create the directory if it doesn't exist
        string directoryPath = System.IO.Path.GetDirectoryName(prefabPath);
        if (!Directory.Exists(directoryPath)) {
            Directory.CreateDirectory(directoryPath);
        }

        GeneratedLevel.RefreshTileReferences();
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(GeneratedLevel.gameObject, prefabPath);

        // Destroy the temporary level GameObject
        DestroyImmediate(GeneratedLevel.gameObject);

        // Select the newly created folder in the Unity Project window
        string folderGUID = AssetDatabase.AssetPathToGUID(path);
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(folderGUID));
        EditorGUIUtility.PingObject(Selection.activeObject);

        // Log a message to indicate that the prefab has been generated
        Debug.Log("Level prefab generated and saved as: <color=cyan><b> " + prefabPath + "</b></color>");
    }

    #endregion
}

#endif