using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GenerateDungeon : MonoBehaviour
{
    public GameObject marker;

    //organizing hierarchy view and clutter
    public GameObject parentPool;
    public GameObject deleterPrefab;
    public GameObject buildingParent;
    public GameObject corridorParent;
    public GameObject otherParent;

    //corridor and building layout blocks
    private GameObject newMarker;
    private GameObject emptyGO;
    private GameObject IO_obj;

    //prefabs
    public GameObject corridorWallPrefab;
    public GameObject corridorCeillingPrefab;
    public GameObject corridorFloorPrefab;
    public GameObject corridorStairsPrefab;

    private GameObject roomFloorPrefab;
    private GameObject roomCeillingPrefab;
    public GameObject roomWallPrefab;
    public GameObject roomWindowPrefab;
    private GameObject roomsStairsPrefab;
    public GameObject roomDoorPrefab;
    private GameObject roomInnerWallPrefab;
    private GameObject roomInnerDoorPrefab;
    //we have upper and bottom body of stairs so it is modular to fit any number of floors
    public GameObject stairsModular;
    public GameObject stairsWalls;
    public GameObject stairsFLoor;
    public GameObject stairsCeilling;
    public GameObject stairsWindows;

    private GameObject newPrefab;
    //Settings for the rooms, corridors and general generation

    //prefab height and width is the distance between prefabs, meaning how big a wall,window,floor should be,
    //if you have 2 meters high and 3 meters wide wall prefab, put 2 in prefabHeight and 3 in prefab width at the inspector
    public float prefabHeight;
    public float prefabWidth;
    public int minRooms;
    public int maxRooms;
    public int minRoomWidth; // in meters 10 = 10 meter wide building
    public int maxRoomWidth;
    public int minRoomHeight;
    public int maxRoomHeight;
    public int minRoomLength;
    public int maxRoomLength;
    public int maxDoors;
    public int windowProbability;
    public int minDistRooms;//min distance of the corridors
    public int maxDistRooms; //max distance of the corridors
    public int distYRooms; // height/floor variation of the corridors

    //private variables for choosing locations and room variables
    private int buildingsCreated;
    private int rndDirX;
    private int rndDirY;
    private int rndDirZ;
    private int roomCount;
    private int doorNum;
    private int noSpawnW1;
    private int noSpawnW2;
    private int noSpawnL1;
    private int noSpawnL2;

    private Vector3 startDoorCor;
    private float roomX;
    private float roomY;
    private float roomZ;
    private float difX;
    private float difY;
    private float difZ;
    private float roomHeight;
    private float roomWidth;
    private float roomLength;


    //list to store the prefabs which were created so later we can remove them
    private List<GameObject> allPrefabsList = new List<GameObject>();
    private List<GameObject> doorPrefabs = new List<GameObject>();
    private List<GameObject> coridorPrefabs = new List<GameObject>();
    private List<GameObject> corridorFloor = new List<GameObject>();
    private List<GameObject> toBeDeletedList = new List<GameObject>();
    public List<GameObject> floorPrefabList = new List<GameObject>();
    public List<GameObject> wallPrefabList = new List<GameObject>();
    public List<int> RndChanceWallList = new List<int>();
    public List<int> rndInnerWDoorList = new List<int>();
    public List<int> rndInnerLDoorList = new List<int>();
    public List<InnerDoor> innerDoorsList = new List<InnerDoor>();
    [SerializeField]
    public List<BuildingStyle> buildingStyleList = new List<BuildingStyle>();
    public List<Cell> cellMapList = new List<Cell>();
    public List<Cell> usedCellsList = new List<Cell>();
    public List<Cell> usableCellsList = new List<Cell>();
    public List<Cell> usableNBList = new List<Cell>();
    private bool frontDoor;
    private bool backDoor;

    //random windows is an option to have windows, above there is the windowsProbability option to set how much their occur
    public bool randomWindows;
    // if windows should skip a floor so it looks like a real building
    public bool windowSkipFloor;
    //if enabled it will randomize the placement of the stairs in the corners on each floor, otherwise all stairs will be placed in one corner on all floors
    public bool randomStairs;
    //other private ingame bools
    private bool newCorDirForward;
    private bool newCorDirBackward;
    private bool firstGenDone;
    private bool forwardDir;
    private bool backwardDir;
    private bool leftDir;
    private bool rightDir;
    private bool upwardDir;
    private bool downwardDir;

    private Vector3 oldRoomPos;
    private Vector3 oldCorridorPos;
    private Vector3 randomSpot;
    private Vector3 frontDoorPos;
    private Vector3 backDoorPos;
    private Vector3 stairsPosOffset;
    private Vector3 corridorCoordinate;
    private Vector3 entranceDoorCor;
    private Vector3 exitDoorCor;
    private Vector3 roomStairsCor;

    //how probable it is to spawn or not spawn a wall 100 is all inner walls spawn, 0 no inner walls, 50 will semi-empty big rooms, 75 should create diverse small-medium rooms
    public float roomPercentage;
    public int prefabSpawnChance;
    //offsets
    public float roomFloorOffsetX;
    public float roomFloorOffsetY;
    public float roomFloorOffsetZ;

    public float roomCeillingOffsetX;
    public float roomCeillingOffsetY;
    public float roomCeillingOffsetZ;

    public float roomWallOffsetX;
    public float roomWallOffSetY;
    public float roomWallOffsetZ;

    public float corridorFloorOffsetX;
    public float corridorFloorOffsetY;
    public float corridorFloorOffsetZ;

    public float corridorCeillingOffsetX;
    public float corridorCeillingOffsetY;
    public float corridorCeillingOffsetZ;

    public float corridorWallOffsetX;
    public float corridorWallOffSetY;
    public float corridorWallOffsetZ;

    public float stairsDownOffsetX;
    public float stairsDownOffsetY;
    public float stairsDownOffsetZ;

    public float stairsUpOffsetX;
    public float stairsUpOffsetY;
    public float stairsUpOffsetZ;

    public float doorOffsetX;
    public float doorOffsetY;
    public float doorOffsetZ;

    private string IO_name;
    private float IO_verticalVar;
    private float IO_horizontalVarX;
    private float IO_horizontalVarZ;
    private float IO_LocalVarX;
    private float IO_LocalVarZ;
    private float IO_LocalVarY;
    private Vector3 IO_rotationVar;
    private float IO_offSetX;
    private float IO_offSetY;
    private float IO_offSetZ;
    //controls how far or apart the walls will be, 0 to 1, 0.6 should be good
    public float spacing;
    private int oldIntW;
    private int oldIntL;
    private int counterW;
    private int counterL;
    private int firstXwall;
    private int secondXwall;
    private int thirdXwall;
    private int firstZwall;
    private int secondZwall;
    private int thirdZwall;
    private int z;
    private int x;
    private int entranceDoorZ;
    private int exitDoorX;
    private int exitDoorZ;
    private int buildingN;
    private int floorN;
    private bool secRowDoorBool;
    private bool thirdRowDoorBool;
    private bool secColDoorBool;
    private bool thirdColDoorBool;
    private bool noSpawn;
    private bool horWall;
    private bool verWall;
    private bool random_rotate;
    private int loopCount;
    private int optStairs;

    void Start()
    {
        //GenLayout();
        //spacing = 0.4f;
    }

    public void GenLayout()
    {
        CleanDungeon();
        roomCount = Random.Range(minRooms, maxRooms);
        print("-----------------room Count: " + roomCount + " -----------------");
        GenRoom();

    }


    void BuildObj(string name, int w, int h, int l, GameObject obj)
    {
        
            if (name.Contains("InnerVerWall") || name.Contains("InnerVerDoor"))
                newMarker = Instantiate(obj, new Vector3(randomSpot.x + (w * prefabWidth) - prefabWidth / 2, randomSpot.y + (h * prefabHeight) + roomFloorOffsetY, randomSpot.z + (l * prefabWidth)), Quaternion.identity);
            else
                newMarker = Instantiate(obj, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight) + roomFloorOffsetY, randomSpot.z + (l * prefabWidth) - prefabWidth / 2), Quaternion.identity);
        
  
        //EMPTYGO WAS HERE
        newMarker.transform.parent = buildingParent.transform;
        if(name.Contains("InnerVerWall") || name.Contains("InnerVerDoor"))
        newMarker.transform.Rotate(Vector3.up * 90);

        newMarker.name = name + buildingsCreated;
        allPrefabsList.Add(newMarker);

        //save front/back pos of the door, so we can stop prefabs from spawning later in code
        if (name.Contains("InnerHorDoor"))
        {
            noSpawnL1 = l - 1;
            noSpawnL2 = l + 1;
            noSpawnW1 = -1;
            noSpawnW2 = -1;
        }
        else if (name.Contains("InnerVerDoor"))
        {
            noSpawnW1 = w - 1;
            noSpawnW2 = w + 1;
            noSpawnL1 = -1;
            noSpawnL2 = -1;
        }
    }


    void RandomizeInnerDoors()
    {
        
        for (int w = 0; w < 3; w++)
        {
            
            if (w == 0)
                z = firstZwall;
            else if (w == 1)
                z = secondZwall;
            else if (w == 2)
                z = thirdZwall;
            // if (w == 0)
            innerDoorsList.Add(new InnerDoor("wf" + w, Random.Range(1, firstXwall), z));
            //else if (w == 1)
                innerDoorsList.Add(new InnerDoor("ws" + w, Random.Range(firstXwall, secondXwall), z));
            //else if (w == 2)
                innerDoorsList.Add(new InnerDoor("wt" + w, Random.Range(secondXwall, thirdXwall), z));
           // else if (w == 3)
                innerDoorsList.Add(new InnerDoor("wft" + w, Random.Range(thirdXwall, Mathf.RoundToInt(roomLength)), z));
           // print("innerDoorsList Count: " + innerDoorsList.Count);
        }
        for (int l = 0; l < 3; l++)
        {
            if (l == 0)
                x = firstXwall;
            else if (l == 1)
                x = secondXwall;
            else if (l == 2)
                x = thirdXwall;
            //for InnerDoor xpos or zpos we place -1 because later we check if w or l counters match our door's position which we place now, and we don't want a 0 otherwise it will be false positive
            //if(l == 0)
            innerDoorsList.Add(new InnerDoor("lf" + l, x, Random.Range(1, firstZwall)));
            //else if (l == 1)
            innerDoorsList.Add(new InnerDoor("ls" + l, x, Random.Range(firstZwall, secondZwall)));
            // else if (l == 2)
            innerDoorsList.Add(new InnerDoor("lt" + l, x, Random.Range(secondZwall, thirdZwall)));
            //else if (l == 3)
            innerDoorsList.Add(new InnerDoor("lft" + l, x, Random.Range(thirdZwall, Mathf.RoundToInt(roomLength))));
            //print("innerDoorsList Count: " + innerDoorsList.Count);
        }
 
    }

    public void GenRoom()
    {

        newMarker = null;
        roomLength = Random.Range(minRoomLength, maxRoomLength);
        roomWidth = Random.Range(minRoomWidth, maxRoomWidth);
        roomHeight = Random.Range(minRoomHeight, maxRoomHeight);

        randomSpot = new Vector3(0,0,0);
        //room generation
        //x = w = width
        //y = h = height
        //z = l = length
        //random number for doors

        //randomly choose corner for the stairs location
        if(randomStairs == false)
        optStairs = Random.Range(0, 4);


        //  print("room lenght: " + roomLength + " / room width: " + roomWidth + " / room height: " + roomHeight);
        //build room
        for (int h = 0; h < roomHeight; h++)
        {

            if (randomStairs == true)
                optStairs = Random.Range(0, 4);
            for (int i = 0; i < 24; i++)
            {
                //pick a number from 1 to 100, if it is less than roomPercentage for an example 75, build the wall, if its bigger, don't
                RndChanceWallList.Add(Random.Range(1, 101));
            }
    
            int roomWallW = Random.Range(Mathf.RoundToInt(roomWidth / 2) - 1, Mathf.RoundToInt(roomWidth / 2) + 1);
            int roomWallL = Random.Range(Mathf.RoundToInt(roomLength / 2) - 1, Mathf.RoundToInt(roomLength / 2) + 1);

            int wallIntersectionL = Random.Range(Mathf.RoundToInt(roomLength / 2 - Random.Range(0, 3)), Mathf.RoundToInt(roomLength / 2) + Random.Range(0, 3));
            int wallIntersectionW = Random.Range(Mathf.RoundToInt(roomWidth / 2) - Random.Range(0, 3), Mathf.RoundToInt(roomWidth / 2) + Random.Range(0, 3));
            int doorW = Random.Range(oldIntW, wallIntersectionW);
            int doorL = Random.Range(oldIntL, wallIntersectionL);
        

            if (h != 0 && h != roomHeight - 1)
            {
                secRowDoorBool = false;
                thirdRowDoorBool = false;
                secColDoorBool = false;
                thirdColDoorBool = false;
               // print("roomWidth: " + roomWidth + " / roomLength: " + roomLength);

                if (buildingStyleList.Count > 0)
                {
                    int bsl = Random.Range(0, buildingStyleList.Count);
                    if (buildingStyleList[bsl].floorStyleList.Count > 0)
                    {
                        
                        int fsl = Random.Range(0, buildingStyleList[bsl].floorStyleList.Count);
                        buildingN = bsl;
                        floorN = fsl;
                        roomFloorPrefab = buildingStyleList[bsl].floorStyleList[fsl].floor;
                        roomCeillingPrefab = buildingStyleList[bsl].floorStyleList[fsl].ceilling;
                        roomWindowPrefab = buildingStyleList[bsl].building_window;
                        roomWallPrefab = buildingStyleList[bsl].building_wall;
                        roomsStairsPrefab = buildingStyleList[bsl].floorStyleList[fsl].stairs;
                        //window-----
                        roomInnerWallPrefab = buildingStyleList[bsl].floorStyleList[fsl].wall;
                        roomInnerDoorPrefab = buildingStyleList[bsl].floorStyleList[fsl].door;

                        
                        floorPrefabList.Clear();
                        wallPrefabList.Clear();
                        //floor prefabs
                        for (int f = 0; f < buildingStyleList[bsl].floorStyleList[fsl].floorPrefabs.Count; f++)
                        {
                            floorPrefabList.Add(buildingStyleList[bsl].floorStyleList[fsl].floorPrefabs[f].floorPrefab);
                        }
                        //wall prefabs
                        for (int w = 0; w < buildingStyleList[bsl].floorStyleList[fsl].wallPrefabs.Count; w++)
                        {
                            wallPrefabList.Add(buildingStyleList[bsl].floorStyleList[fsl].wallPrefabs[w].wallPrefab);
                        }
                    }
                }
                if (roomLength > 3 && roomLength < 8)
                {
                    counterL = 1;
                    firstZwall = Random.Range(Mathf.RoundToInt(Mathf.RoundToInt(roomLength / 2)), Mathf.RoundToInt(roomLength / 2 + 1));
                    secondZwall = Mathf.RoundToInt(roomLength);
                    thirdZwall = 0;
                }

                else if (roomLength >= 8 && roomLength < 12)
                {
                    counterL = 3;
                    firstZwall = Random.Range(Mathf.RoundToInt(Mathf.RoundToInt(roomLength / 3)), Mathf.RoundToInt(roomLength / 3 + 1));
                    secondZwall = Random.Range(Mathf.RoundToInt((firstZwall * 2f) - 1), Mathf.RoundToInt((firstZwall * 2f)));
                    thirdZwall = Mathf.RoundToInt(roomLength);
                }

                else if (roomLength >= 12)
                {
                    counterL = 4;
                    firstZwall = Random.Range(Mathf.RoundToInt(Mathf.RoundToInt(roomLength / 3 - 1)), Mathf.RoundToInt(roomLength / 3));
                    secondZwall = Random.Range(Mathf.RoundToInt((firstZwall * 1.7f) - 1), Mathf.RoundToInt((firstZwall * 1.7f)));
                    thirdZwall =  Random.Range((secondZwall + firstZwall) - 1, (secondZwall + firstZwall));
                }

                if (roomWidth > 3 && roomWidth < 8)
                {
                    counterW = 2;
                    firstXwall = Random.Range(Mathf.RoundToInt(roomWidth / 2), Mathf.RoundToInt(roomWidth / 2 + 1));
                    secondXwall = Mathf.RoundToInt(roomWidth);
                    thirdXwall = 0;
                }

                else if (roomWidth >= 8 && roomWidth < 12)
                {
                    counterW = 3;
                    firstXwall = Random.Range(Mathf.RoundToInt(Mathf.RoundToInt(roomWidth / 3)), Mathf.RoundToInt(roomWidth / 3 + 1));
                    secondXwall = Random.Range(Mathf.RoundToInt((firstXwall * 2f) - 1), Mathf.RoundToInt((firstXwall * 2f)));
                    thirdXwall = Mathf.RoundToInt(roomWidth); ;
                }

                else if (roomWidth >= 12)
                {
                    counterW = 4;
                    firstXwall = Random.Range(Mathf.RoundToInt(Mathf.RoundToInt(roomWidth / 3 - 1)), Mathf.RoundToInt(roomWidth / 3));
                    secondXwall = Random.Range(Mathf.RoundToInt((firstXwall * 1.7f) - 1), Mathf.RoundToInt((firstXwall * 1.7f)));
                    thirdXwall = Random.Range((secondXwall + firstXwall) - 1, (secondXwall + firstXwall));

                }
                //choose door random position along the walls
                RandomizeInnerDoors();

                for (int l = 0; l < roomLength; l++)
                {
                    //random door placement - randomDoorPosF for front door, randomDoorPosB for back door, a random number between 1 and room width is generated
                   

                    for (int w = 0; w < roomWidth; w++)
                    {

                        //create the building's floor, ceilling
                        if (h == 0 || h == roomHeight - 1 || l == 0 || l == roomLength - 1 || w == 0 || w == roomWidth - 1)
                        {
                            // spawn floor 
                            if (h == 0)
                            {
                            newMarker = Instantiate(roomFloorPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + roomFloorOffsetX, randomSpot.y + (h * prefabHeight) + roomFloorOffsetY, randomSpot.z + (l * prefabWidth) + roomFloorOffsetZ), Quaternion.identity);
                            newMarker.transform.parent = buildingParent.transform;
                            allPrefabsList.Add(newMarker);
                            newMarker.name = "Ground" + buildingsCreated;
                            }
                            //spawn ceilling
                            else if (h == roomHeight - 1)
                            {
                            newMarker = Instantiate(roomCeillingPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + roomCeillingOffsetX, randomSpot.y + (h * prefabHeight) + roomCeillingOffsetY, randomSpot.z + (l * prefabWidth) + roomCeillingOffsetZ), Quaternion.identity);
                            newMarker.transform.parent = buildingParent.transform;
                            allPrefabsList.Add(newMarker);
                            newMarker.name = "Ceilling" + buildingsCreated;
                            }
                                   
                            
                        }
                        //spawn outer walls and windows
                         if (l == 0 || l == roomLength - 1 || w == 0 || w == roomWidth - 1)
                        {
                           // print("l: " + l + " /w: " + w + " /a wall or a window should be built");
                            //spot reserved for door, dont build anything
                            // if (w == 0 && l == doorNum && h == 1) { }
                           
                                int windowChance = Random.Range(1, 100);
                                if (windowChance <= windowProbability)
                                {
                                    if (w == 0)
                                    {
                                        newMarker = Instantiate(roomWindowPrefab, new Vector3(randomSpot.x + (w * prefabWidth) - prefabWidth * spacing, randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                        //when the object is at the right wall, rotate it left so it looks inwards
                                        newMarker.transform.Rotate(Vector3.up * 90);
                                    }
                                    else if (w == roomWidth - 1)
                                    {
                                        newMarker = Instantiate(roomWindowPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing, randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                        //when the object is at the left wall rotate it right so it looks inwards
                                        newMarker.transform.Rotate(-Vector3.up * 90);
                                    }
                                    else if (l == roomLength - 1)
                                    {
                                        newMarker = Instantiate(roomWindowPrefab, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth) + prefabWidth * spacing), Quaternion.identity);
                                        newMarker.transform.Rotate(Vector3.up * 180);
                                    }
                                    else if (l == 0)
                                    {
                                        newMarker = Instantiate(roomWindowPrefab, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth) - prefabWidth * spacing), Quaternion.identity);
                                    }

                                    newMarker.transform.parent = buildingParent.transform;
                                    newMarker.name = "window" + buildingsCreated;

                                    Vector3 offSetPos = newMarker.transform.position + newMarker.transform.forward * roomWallOffsetZ;
                                    newMarker.transform.position = offSetPos;
                                    offSetPos = newMarker.transform.position + newMarker.transform.right * roomWallOffsetX;
                                    newMarker.transform.position = offSetPos;
                                    allPrefabsList.Add(newMarker);
                                }
                               else
                                {
                                    if (w == 0)
                                    {
                                        newMarker = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) - prefabWidth * spacing, randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                        //when the object is at the right wall, rotate it left so it looks inwards
                                        newMarker.transform.Rotate(Vector3.up * 90);
                                    }
                                    else if (w == roomWidth - 1)
                                    {
                                        newMarker = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing, randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                        //when the object is at the left wall rotate it right so it looks inwards
                                        newMarker.transform.Rotate(-Vector3.up * 90);
                                    }
                                    else if (l == roomLength - 1)
                                    {
                                        newMarker = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth) + prefabWidth * spacing), Quaternion.identity);
                                        newMarker.transform.Rotate(Vector3.up * 180);
                                    }
                                    else if (l == 0)
                                    {
                                        newMarker = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth) - prefabWidth * spacing), Quaternion.identity);
                                    }

                           
                                    newMarker.name = "Wall" + buildingsCreated;
                                    //emptygo
                                    newMarker.transform.parent = buildingParent.transform;
                                    allPrefabsList.Add(newMarker);
                                }

                        }
                        //create corners
                        if (w == 0 && l == 0 || w == roomWidth - 1 && l == roomLength - 1 || w == 0 && l == roomLength - 1 || l == 0 && w == roomWidth - 1)
                        {

                                if (w == 0 && l == 0)
                                {
                                    newMarker = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth) - prefabWidth * spacing), Quaternion.identity);
                                    //when the object is at the right wall, rotate it left so it looks inwards
                                    //newMarker.transform.Rotate(Vector3.up * 90);
                                }
                                else if (l == 0 && w == roomWidth - 1)
                                {
                                    newMarker = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth) - prefabWidth * spacing), Quaternion.identity);
                                    //when the object is at the left wall rotate it right so it looks inwards
                                    // newMarker.transform.Rotate(Vector3.up * 180);
                                }
                                else if (w == 0 && l == roomLength - 1)
                                {
                                    newMarker = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth) + prefabWidth * spacing), Quaternion.identity);
                                    newMarker.transform.Rotate(Vector3.up * 180);
                                }
                                else if (w == roomWidth - 1 && l == roomLength - 1)
                                {
                                    newMarker = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth) + prefabWidth * spacing), Quaternion.identity);
                                    newMarker.transform.Rotate(Vector3.up * 180);
                                }
                                    
                                    //emptygo
                                newMarker.transform.parent = buildingParent.transform;
                                newMarker.name = "Wall" + buildingsCreated;

                                Vector3 offSetPos = newMarker.transform.position + newMarker.transform.forward * roomWallOffsetZ;
                                newMarker.transform.position = offSetPos;
                                offSetPos = newMarker.transform.position + newMarker.transform.right * roomWallOffsetX;
                                newMarker.transform.position = offSetPos;
                                allPrefabsList.Add(newMarker);
                        }
                      
                     //spawn inner walls and doors
                        if (l == firstZwall && w < firstXwall  && RndChanceWallList[0] <= roomPercentage)
                        {

                            if(w == innerDoorsList[0].doorXpos)
                            {
                                BuildObj("0InnerHorDoor", w, h, l, roomInnerDoorPrefab);
                                
                            }
                            else
                            BuildObj("0InnerHorWall",w,h,l, roomInnerWallPrefab);

                            horWall = true;
                            //print("l == firstZwall && w < firstXwall, building 0 innerHorWall");
                        }
                        //building horizontal wall toward the second intersection point
                        if (l == firstZwall && w >= firstXwall && w < secondXwall && RndChanceWallList[1] <= roomPercentage)
                        {
                            if (w == innerDoorsList[1].doorXpos)
                            {
                                BuildObj("1InnerHorDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("1InnerHorWall", w, h, l, roomInnerWallPrefab);

                            horWall = true;
                            // print("l == firstZwall && w >= firstXwall && w < secondXwall, building 1 innerHorWall");
                        }
                        //building horizontal wall toward the third intersection point
                        if (l == firstZwall && w >= secondXwall && w < thirdXwall  && RndChanceWallList[2] <= roomPercentage)
                        {
                            if (w == innerDoorsList[2].doorXpos)
                            {
                                BuildObj("2InnerHorDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("2InnerHorWall", w, h, l, roomInnerWallPrefab);

                            horWall = true;
                            //  print("l == firstZwall && w >= secondXwall && w < thirdXwall, building 2 innerHorWall");
                        }
                        //building last wall horizontal wall after the third intersection point toward the building's outer wall
                        if (l == firstZwall && w >= thirdXwall && thirdXwall != 0  && RndChanceWallList[3] <= roomPercentage)
                        {
                            if (w == innerDoorsList[3].doorXpos)
                            {
                                BuildObj("3InnerHorDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("3InnerHorWall", w, h, l, roomInnerWallPrefab);

                            horWall = true;
                            // print("l == firstZwall && w >= thirdXwall, building 3 innerHorWall");
                        }
                        //building second horizontal wall toward the first intersection point
                        if (l == secondZwall && w < firstXwall && secondZwall != 0 && RndChanceWallList[4] <= roomPercentage)
                        {
                            if (w == innerDoorsList[4].doorXpos)
                            {
                                BuildObj("4InnerHorDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("4InnerHorWall", w, h, l, roomInnerWallPrefab);

                            horWall = true;
                            // print("l == secondZwall && w < firstXwall, building 4 innerHorWall");
                        }
                        //building second horizontal wall toward the second intersection point
                        if (l == secondZwall && w >= firstXwall && w < secondXwall && secondZwall != 0 && RndChanceWallList[5] <= roomPercentage)
                        {
                            if (w == innerDoorsList[5].doorXpos)
                            {
                                BuildObj("5InnerHorDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("5InnerHorWall", w, h, l, roomInnerWallPrefab);

                            horWall = true;
                            // print("l == secondZwall && w >= firstXwall && w < secondXwall, building 5 innerHorWall");
                        }
                        //building second horizontal wall toward the third intersection point
                        if (l == secondZwall && w >= secondXwall && w < thirdXwall && secondZwall != 0 && RndChanceWallList[6] <= roomPercentage)
                        {
                            if (w == innerDoorsList[6].doorXpos)
                            {
                                BuildObj("6InnerHorDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("6InnerHorWall", w, h, l, roomInnerWallPrefab);

                            horWall = true;
                            // print("l == secondZwall && w > secondXwall && w < thirdXwall, building 6 innerHorWall");
                        }
                        //building second horizontal wall toward after the third intersection point toward the building's outer wall
                        if (l == secondZwall && w >= thirdXwall && secondZwall != 0 && thirdXwall != 0 && RndChanceWallList[7] <= roomPercentage)
                        {
                            if (w == innerDoorsList[7].doorXpos)
                            {
                                BuildObj("7InnerHorDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("7InnerHorWall", w, h, l, roomInnerWallPrefab);

                            horWall = true;
                            // print("l == secondZwall && w >= thirdXwall, building 7 innerHorWall");
                        }
                        //building third horizontal wall toward the intersection point
                        if (l == thirdZwall && w < firstXwall && thirdZwall != 0 && RndChanceWallList[8] <= roomPercentage)
                        {
                            if (w == innerDoorsList[8].doorXpos)
                            {
                                BuildObj("8InnerHorDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("8InnerHorWall", w, h, l, roomInnerWallPrefab);

                            horWall = true;
                            // print("l == thirdZwall && w < firstXwall, building 8 innerHorWall");
                        }
                        //building third horizontal wall toward the second intersection point
                        if (l == thirdZwall && w >= firstXwall && w < secondXwall && thirdZwall != 0 && RndChanceWallList[9] <= roomPercentage)
                        {
                            if (w == innerDoorsList[9].doorXpos)
                            {
                                BuildObj("9InnerHorDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("9InnerHorWall", w, h, l, roomInnerWallPrefab);

                            horWall = true;
                            //  print("l == thirdZwall && w >= firstXwall && w < secondXwall, building 9 innerHorWall");
                        }
                        //building third horizontal wall toward the third intersection point
                        if (l == thirdZwall && w >= secondXwall && w < thirdXwall && thirdZwall != 0 && RndChanceWallList[10] <= roomPercentage)
                        {
                            if (w == innerDoorsList[10].doorXpos)
                            {
                                BuildObj("10InnerHorDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("10InnerHorWall", w, h, l, roomInnerWallPrefab);

                            horWall = true;
                            // print("l == thirdZwall && w >= secondXwall && w < thirdXwall, building 10 innerHorWall");
                        }
                        //building third horizontal wall toward after the third intersection point toward the building's outer wall
                        if (l == thirdZwall && w >= thirdXwall && thirdZwall != 0 && thirdXwall != 0 && RndChanceWallList[11] <= roomPercentage)
                        {
                            if (w == innerDoorsList[11].doorXpos)
                            {
                                BuildObj("11InnerHorDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("11InnerHorWall", w, h, l, roomInnerWallPrefab);

                            horWall = true;
                            // print("l == thirdZwall && w >= thirdXwall, building 11 innerHorWall");
                        }
                       
                        //building first vertical wall toward the first intersection point
                        if (w == firstXwall && l < firstZwall  && RndChanceWallList[12] <= roomPercentage)
                        {
                            if (l == innerDoorsList[12].doorZpos)
                            {
                                BuildObj("0InnerVerDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("0InnerVerWall", w, h, l, roomInnerWallPrefab);

                            verWall = true;
                            // print("w == firstXwall && l < firstZwall, building 12 innerVerWall");
                        }
                        //building first vertical wall toward the second intersection point
                        if (w == firstXwall && l >= firstZwall && l < secondZwall &&  RndChanceWallList[13] <= roomPercentage)
                        {
                            if (l == innerDoorsList[13].doorZpos)
                            {
                                BuildObj("1InnerVerDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("1InnerVerWall", w, h, l, roomInnerWallPrefab);

                            verWall = true;
                            // print("w == firstXwall && l >= firstZwall && l < secondZwall, building 13 innerVerWall");
                        }
                    //building first horizontal wall toward the third intersection point
                        if (w == firstXwall && l >= secondZwall && l < thirdZwall  && RndChanceWallList[14] <= roomPercentage)
                        {
                            if (l == innerDoorsList[14].doorZpos)
                            {
                                BuildObj("2InnerVerDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("2InnerVerWall", w, h, l, roomInnerWallPrefab);

                            verWall = true;
                            // print("w == firstXwall && l >= secondZwall && l < thirdZwall, building 14 innerVerWall");
                        }
                    //building first horizontal wall toward after the third intersection point toward the building's outer wall
                        if (w == firstXwall && l >= thirdZwall  && thirdZwall != 0 && RndChanceWallList[15] <= roomPercentage)
                        {
                            if (l == innerDoorsList[15].doorZpos)
                            {
                                BuildObj("3InnerVerDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("3InnerVerWall", w, h, l, roomInnerWallPrefab);

                            verWall = true;
                            // print("w == firstXwall && l >= thirdZwall, building 15 innerVerWall");
                        }
                    //building second horizontal wall toward the first intersection point
                        if (w == secondXwall && l < firstZwall && secondXwall != 0 && secondXwall != 0 && RndChanceWallList[16] <= roomPercentage)
                        {
                            if (l == innerDoorsList[16].doorZpos)
                            {
                                BuildObj("4InnerVerDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("4InnerVerWall", w, h, l, roomInnerWallPrefab);

                            verWall = true;
                            // print("w == secondXwall && l < firstZwall, building 16 innerVerWall");
                        }
                        //building second horizontal wall toward the second intersection point
                        if (w == secondXwall && l >= firstZwall && l < secondZwall && secondXwall != 0 && RndChanceWallList[17] <= roomPercentage)
                        {
                            if (l == innerDoorsList[17].doorZpos)
                            {
                                BuildObj("5InnerVerDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("5InnerVerWall", w, h, l, roomInnerWallPrefab);

                            verWall = true;
                            // print("w == secondXwall && l >= firstZwall && l < secondZwall, building 17 innerVerWall");
                        }
                        //building second horizontal wall toward the third intersection point
                        if (w == secondXwall && l >= secondZwall && l < thirdZwall && secondXwall != 0 && RndChanceWallList[18] <= roomPercentage)
                        {
                            if (l == innerDoorsList[18].doorZpos)
                            {
                                BuildObj("6InnerVerDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("6InnerVerWall", w, h, l, roomInnerWallPrefab);

                            verWall = true;
                            // print("w == secondXwall && l >= secondZwall && l < thirdZwall, building 18 innerVerWall");
                        }
                        //building second horizontal wall toward after the third intersection point toward the building's outer wall
                        if (w == secondXwall && l >= thirdZwall && secondXwall != 0 && thirdZwall != 0 && RndChanceWallList[19] <= roomPercentage)
                        {
                            if (l == innerDoorsList[19].doorZpos)
                            {
                                BuildObj("7InnerVerDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("7InnerVerWall", w, h, l, roomInnerWallPrefab);

                            verWall = true;
                            //  print("w == secondXwall && l >= thirdZwall, building 19 innerVerWall");
                        }
                        //building third horizontal wall toward the intersection point
                        if (w == thirdXwall && l < firstZwall && thirdXwall != 0 && RndChanceWallList[20] <= roomPercentage)
                        {
                            if (l == innerDoorsList[20].doorZpos)
                            {
                                BuildObj("8InnerVerDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("8InnerVerWall", w, h, l, roomInnerWallPrefab);

                            verWall = true;
                            // print("w == thirdXwall && l < firstZwall, building 20 innerVerWall");
                        }
                        //building third horizontal wall toward the second intersection point
                        if (w == thirdXwall && l >= firstZwall && l < secondZwall && thirdXwall != 0  && RndChanceWallList[21] <= roomPercentage)
                        {
                            if (l == innerDoorsList[21].doorZpos)
                            {
                                BuildObj("9InnerVerDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("9InnerVerWall", w, h, l, roomInnerWallPrefab);

                            verWall = true;
                            // print("w == thirdXwall && l > firstZwall && l < secondZwall, building 21 innerVerWall");
                        }
                        //building third horizontal wall toward the third intersection point
                        if (w == thirdXwall && l >= secondZwall && l < thirdZwall && thirdXwall != 0 && RndChanceWallList[22] <= roomPercentage)
                        {
                            if (l == innerDoorsList[22].doorZpos)
                            {
                                BuildObj("10InnerVerDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("10InnerVerWall", w, h, l, roomInnerWallPrefab);

                            verWall = true;
                            // print("w == thirdXwall && l >= secondZwall && l < thirdZwall, building 22 innerVerWall");
                        }
                        //building third horizontal wall toward after the third intersection point toward the building's outer wall
                        if (w == thirdXwall && l >= thirdZwall && thirdZwall != 0 && thirdXwall != 0 && RndChanceWallList[23] <= roomPercentage)
                        {
                            if (l == innerDoorsList[23].doorZpos)
                            {
                                BuildObj("11InnerVerDoor", w, h, l, roomInnerDoorPrefab);
                            }
                            else
                                BuildObj("11InnerVerWall", w, h, l, roomInnerWallPrefab);

                            verWall = true;
                           // print("w == thirdXwall && l >= thirdZwall, building 23 innerVerWall");
                        }
                       
                      // spawn stairs, inner floors and prefabs

                        //dont create a floor at the corner, instead create stairs
                        //stairs left back corner
                        if (optStairs == 0 && l == 0 && w == 0)
                        {
                            //floor
                            if (h == 1)
                            {
                                newMarker = Instantiate(roomFloorPrefab, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                newMarker.transform.parent = buildingParent.transform;
                                newMarker.name = h + "floor" + buildingsCreated;
                                allPrefabsList.Add(newMarker);
                                if (!randomStairs)
                                {
                                    GameObject newWall = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing, randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                    newWall.transform.Rotate(-Vector3.up * 90);
                                    allPrefabsList.Add(newWall);
                                }

                            }
                            else if (h != roomHeight - 1)
                            {
                                newMarker = Instantiate(stairsModular, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight) + prefabHeight / 2 - prefabHeight, randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                newMarker.transform.parent = buildingParent.transform;
                                newMarker.name = h + "stairs" + buildingsCreated;
                                allPrefabsList.Add(newMarker);

                                //wall
                                GameObject newWall = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing, randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                newWall.transform.Rotate(-Vector3.up * 90);
                                allPrefabsList.Add(newWall);
                                if (randomStairs)
                                {
                                    //wall at the bottom of the stairs
                                    GameObject newWall2 = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing, randomSpot.y + (h * prefabHeight) - prefabHeight, randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                    newWall2.transform.Rotate(-Vector3.up * 90);
                                    allPrefabsList.Add(newWall2);
                                }
                            }
                           
                        }
                        //stairs left front corner
                        else if (optStairs == 1 && l == roomLength - 1 && w == 0)
                        {
                            //floor
                            if (h == 1)
                            {
                                newMarker = Instantiate(roomFloorPrefab, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                newMarker.transform.parent = buildingParent.transform;
                                newMarker.name = h + "floor" + buildingsCreated;
                                allPrefabsList.Add(newMarker);
                                if (!randomStairs)
                                {
                                    GameObject newWall = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing, randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                    newWall.transform.Rotate(-Vector3.up * 90);
                                    allPrefabsList.Add(newWall);
                                }

                            }
                            else if (h != roomHeight - 1)
                            {
                                newMarker = Instantiate(stairsModular, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight) + prefabHeight / 2 - prefabHeight, randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                newMarker.transform.parent = buildingParent.transform;
                                newMarker.name = h + "stairs" + buildingsCreated;
                                allPrefabsList.Add(newMarker);
                                newMarker.transform.Rotate(Vector3.up * 180);

                                //wall
                                GameObject newWall = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing, randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                newWall.transform.Rotate(-Vector3.up * 90);
                                allPrefabsList.Add(newWall);
                                if (randomStairs)
                                {
                                    //wall at the bottom of the stairs
                                    GameObject newWall2 = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing, randomSpot.y + (h * prefabHeight) - prefabHeight, randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                    newWall2.transform.Rotate(-Vector3.up * 90);
                                    allPrefabsList.Add(newWall2);
                                }
                            }
                           
                        }
                        //stairs right back corner
                        else if (optStairs == 2 && w == roomWidth - 1 && l == 0 )
                        {

                            //floor
                            if (h == 1)
                            {
                                newMarker = Instantiate(roomFloorPrefab, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                newMarker.transform.parent = buildingParent.transform;
                                newMarker.name = h + "floor" + buildingsCreated;
                                allPrefabsList.Add(newMarker);
                                if (!randomStairs)
                                {
                                    GameObject newWall = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing - prefabWidth * 1.1f, randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                    newWall.transform.Rotate(Vector3.up * 90);
                                    allPrefabsList.Add(newWall);
                                }

                            }
                            else if (h != roomHeight - 1)
                            {
                                newMarker = Instantiate(stairsModular, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight) + prefabHeight / 2 - prefabHeight, randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                newMarker.transform.parent = buildingParent.transform;
                                newMarker.name = h + "stairs" + buildingsCreated;
                                allPrefabsList.Add(newMarker);

                                //wall at the top of the stairs
                                GameObject newWall = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing - prefabWidth * 1.1f, randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                newWall.transform.Rotate(Vector3.up * 90);
                                allPrefabsList.Add(newWall);
                                if(randomStairs)
                                {
                                    //wall at the bottom of the stairs
                                    GameObject newWall2 = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing - prefabWidth * 1.1f, randomSpot.y + (h * prefabHeight) - prefabHeight, randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                    newWall2.transform.Rotate(Vector3.up * 90);
                                    allPrefabsList.Add(newWall2);
                                }
                                
                            }
                            
                        }
                        //stairs right front corner
                        else if (optStairs == 3 && l == roomLength - 1 && w == roomWidth - 1)
                        {
                            //floor
                            if (h == 1)
                            {
                                newMarker = Instantiate(roomFloorPrefab, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                newMarker.transform.parent = buildingParent.transform;
                                newMarker.name = h + "floor" + buildingsCreated;
                                allPrefabsList.Add(newMarker);
                                if (!randomStairs)
                                {
                                    GameObject newWall = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing - prefabWidth * 1.1f, randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                    newWall.transform.Rotate(Vector3.up * 90);
                                    allPrefabsList.Add(newWall);
                                }

                            }
                            else if (h != roomHeight - 1) 
                            {
                                newMarker = Instantiate(stairsModular, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight) + prefabHeight / 2 - prefabHeight, randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                newMarker.transform.parent = buildingParent.transform;
                                newMarker.name = h + "stairs" + buildingsCreated;
                                allPrefabsList.Add(newMarker);
                                newMarker.transform.Rotate(Vector3.up * 180);
                                //wall
                                GameObject newWall = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing - prefabWidth * 1.1f, randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                newWall.transform.Rotate(Vector3.up * 90);
                                allPrefabsList.Add(newWall);
                                if (randomStairs)
                                {
                                    //wall at the bottom of the stairs
                                    GameObject newWall2 = Instantiate(roomWallPrefab, new Vector3(randomSpot.x + (w * prefabWidth) + prefabWidth * spacing - prefabWidth * 1.1f, randomSpot.y + (h * prefabHeight) - prefabHeight, randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                                    newWall2.transform.Rotate(Vector3.up * 90);
                                    allPrefabsList.Add(newWall2);
                                }
                            }
                            
                        }
                        //--------------make floor and spawn prefabs----------------------
                        else
                        {
                            int rndFloorPrefab = Random.Range(0, floorPrefabList.Count - 1);
                            int rndWallPrefab = Random.Range(0, wallPrefabList.Count - 1);
                            int rndChancePrefab = Random.Range(1, 101);
                            //dont spawn prefab at the door or behind/in front of the door as well as dont spawn prefabs in front of the entrance/exit doors of the building
                            for (int i = 0; i < innerDoorsList.Count; i++)
                            {

                                if (innerDoorsList[i].doorXpos == w && innerDoorsList[i].doorZpos == l ||
                                    innerDoorsList[i].doorXpos == w + 1 && innerDoorsList[i].doorZpos == l && innerDoorsList[i].name.Contains("l") ||
                                    innerDoorsList[i].doorXpos == w && innerDoorsList[i].doorZpos == l + 1 && innerDoorsList[i].name.Contains("w") ||
                                    entranceDoorZ == l && w == 1 || exitDoorX == w && exitDoorZ == l)
                                    noSpawn = true;
                                    
                            }
                           
                                //floor
                                newMarker = Instantiate(roomFloorPrefab, new Vector3(randomSpot.x + (w * prefabWidth), randomSpot.y + (h * prefabHeight), randomSpot.z + (l * prefabWidth)), Quaternion.identity);
                         
                                //prefab 
                                if (rndChancePrefab <= prefabSpawnChance && noSpawn == false && horWall == false && verWall == false && buildingStyleList[buildingN].floorStyleList[floorN].floorPrefabs.Count >= rndFloorPrefab)
                                {
                                    float fx_offset = buildingStyleList[buildingN].floorStyleList[floorN].floorPrefabs[rndFloorPrefab].x_offset;
                                    float fy_offset = buildingStyleList[buildingN].floorStyleList[floorN].floorPrefabs[rndFloorPrefab].y_offset;
                                    float fz_offset = buildingStyleList[buildingN].floorStyleList[floorN].floorPrefabs[rndFloorPrefab].z_offset;
                                    random_rotate = buildingStyleList[buildingN].floorStyleList[floorN].floorPrefabs[rndFloorPrefab].random_Rotation;
                                    //print("trying to instantiate floor prefab: " + floorPrefabList[rndFloorPrefab].name);
                                    newPrefab = Instantiate(floorPrefabList[rndFloorPrefab], new Vector3(randomSpot.x + (w * prefabWidth) + fx_offset, randomSpot.y + (h * prefabHeight) + fy_offset, randomSpot.z + (l * prefabWidth) + fz_offset), Quaternion.identity);

                                    if (random_rotate)
                                        newPrefab.transform.Rotate(Vector3.up * Random.Range(0, 359));
                                }
                                //if we spawn a prefab at the wall, rotate the obj 90* to align with the wall
                                else if (rndChancePrefab <= prefabSpawnChance && noSpawn == false && buildingStyleList[buildingN].floorStyleList[floorN].wallPrefabs.Count >= rndWallPrefab)
                                {
                                    float wx_offset = buildingStyleList[buildingN].floorStyleList[floorN].wallPrefabs[rndWallPrefab].x_offset;
                                    float wy_offset = buildingStyleList[buildingN].floorStyleList[floorN].wallPrefabs[rndWallPrefab].y_offset;
                                    float wz_offset = buildingStyleList[buildingN].floorStyleList[floorN].wallPrefabs[rndWallPrefab].z_offset;
                                    bool flip_rotate = buildingStyleList[buildingN].floorStyleList[floorN].wallPrefabs[rndWallPrefab].rotate_90_degrees;

                                    if (horWall)
                                    {
                                        newPrefab = Instantiate(wallPrefabList[rndWallPrefab], new Vector3(randomSpot.x + (w * prefabWidth) + wx_offset, randomSpot.y + (h * prefabHeight) + wy_offset, randomSpot.z + (l * prefabWidth) + wz_offset), Quaternion.identity);
                                        if (flip_rotate)
                                            newPrefab.transform.Rotate(Vector3.up * 90);
                                    }
                                    else if (verWall)
                                    {
                                        newPrefab = Instantiate(wallPrefabList[rndWallPrefab], new Vector3(randomSpot.x + (w * prefabWidth) + wx_offset, randomSpot.y + (h * prefabHeight) + wy_offset, randomSpot.z + (l * prefabWidth) + wz_offset), Quaternion.identity);
                                        newPrefab.transform.Rotate(Vector3.up * 90);
                                        if (flip_rotate)
                                            newPrefab.transform.Rotate(Vector3.up * 90);
                                    }
                                }
                                
                            
                            newMarker.transform.parent = buildingParent.transform;
                            newMarker.name = h + "floor" + buildingsCreated;
                            allPrefabsList.Add(newMarker);

                            if (newPrefab != null)
                            {
                                newPrefab.transform.parent = buildingParent.transform;
                                newPrefab.name = h + newPrefab.name + buildingsCreated;
                                allPrefabsList.Add(newPrefab);
                            }
                            
                        }
                        noSpawn = false;
                        horWall = false;
                        verWall = false;
                    }
                    oldIntW = wallIntersectionW;
                    oldIntL = wallIntersectionL;
                    


                }
            }
            RndChanceWallList.Clear();
            rndInnerLDoorList.Clear();
            rndInnerWDoorList.Clear();
            innerDoorsList.Clear();
            
        }



        //depending on the layout create mini rooms with walls and doors
        //generate objects on free/buildable spots
        //depending on size, objet can be attached to walls,corners if small enough (less than half of prefabWidth)
        buildingsCreated++;
    }
  

    public void CleanDungeon()
    {
        frontDoor = false;
        backDoor = false;
        leftDir = false;
        rightDir = false;
        forwardDir = false;
        backwardDir = false;
        downwardDir = false;
        upwardDir = false;
        newCorDirBackward = false;
        newCorDirForward = false;
        firstGenDone = false;
        oldRoomPos = Vector3.zero;
        oldCorridorPos = Vector3.zero;
        randomSpot = Vector3.zero;
        buildingsCreated = 0;
        rndDirX = 0;
        rndDirY = 0;
        rndDirZ = 0;
        newMarker = null;

        foreach (GameObject go in allPrefabsList)
        {
            DestroyImmediate(go);
        }
        usedCellsList.Clear();
        allPrefabsList.Clear();
        doorPrefabs.Clear();
        coridorPrefabs.Clear();
        corridorFloor.Clear();
        toBeDeletedList.Clear();
        RndChanceWallList.Clear();
        rndInnerWDoorList.Clear();
        rndInnerLDoorList.Clear();
        innerDoorsList.Clear();
        wallPrefabList.Clear();
        floorPrefabList.Clear();
    }

    

}