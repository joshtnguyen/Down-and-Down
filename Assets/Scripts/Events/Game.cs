using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    public static List<Character> characters = new List<Character>();
    public static Character walter = new Character("Walter", 1, 100, 15, 10, 10, 5, 12);
    public static Character benedict = new Character("Benedict", 1, 95, 10, 12, 11, 8, 8);
    public static Character sherri = new Character("Sherri", 1, 120, 7, 12, 12, 8, 3);
    public static Character jade = new Character("Jade", 1, 150, 5, 18, 9, 2, 3);

    public static bool firstRun = false;
    
    public static bool gameMovementFreeze = false;

    public static string gameEvent = "Roaming";
    
    public GameObject originalEnemy;
    public static bool updateEnemies = false;

    public static int mapLength = 7;
    public static int mapEasing = 10;
    public static int row = 0;
    public static int col = 0;
    public static int floorNumber = -1;
    public static int disruptions = 0;
    public static int gold = 500;

    // FLOOR INFO
    public static int steps = 0;
    public static int enemiesKilled = 0;
    public static int timesDowned = 0;

    public static Room[,] map = new Room[mapLength, mapLength];
    public static List<int[]> combos = new List<int[]>();

    public GameObject northBlocker;
    public GameObject eastBlocker;
    public GameObject southBlocker;
    public GameObject westBlocker;

    public static bool showMap = false;
    public static List<GameObject> mapObjects = new List<GameObject>();
    public GameObject mapObject;
    public GameObject roomIndicator;
    public GameObject roomIndicatorContainer;
    public GameObject directionIndicatorContainer;
    public GameObject directionIndicator;
    public Text floorIndicator;

    public GameObject teleporterArea;
    public GameObject teleporter;

    public GameObject wizardArea;
    public GameObject breakArea;

    // Start is called before the first frame update
    void Start()
    {

        if (!firstRun) {
            firstRun = true;
            SkillsRegistry.firstRun();
            characters.Add(walter);
            characters.Add(benedict);
            characters.Add(sherri);
            characters.Add(jade);

            foreach (Character c in characters) {
                c.skills.Add(SkillsRegistry.getSkill("Attack"));
                c.skills.Add(SkillsRegistry.getSkill("Block"));
                c.verifyMod();
            }

            for (int i = 0; i < mapLength; i++) {
                for (int j = 0; j < mapLength; j++) {
                    map[i, j] = new Room();
                }
            }

            createFloor();

        }
        
        gameEvent = "Roaming";
    }

    public static void createFloor() {

        steps = 0;
        enemiesKilled = 0;
        timesDowned = 0;
        
        for (int i = 0; i < mapLength; i++) {
            for (int j = 0; j < mapLength; j++) {
                map[i, j].Reset();
                int[] c = {i, j};
                combos.Add(c);
            }
        }

        row = Random.Range(0, mapLength);
        col = Random.Range(0, mapLength);
        setRoom(row, col, "null");
        setRoom(row, col, "null");
        setRoom(row, col, "null");

        map[row, col].roomType = "Entrance";

        List<string> roomTypes = new List<string>();
        for (int i = 0; i < 1; i++) {
            roomTypes.Add("Exit");
        }
        for (int i = 0; i < 2; i++) {
            roomTypes.Add("Wizard Room");
        }
        for (int i = 0; i < 15; i++) {
            roomTypes.Add("Normal Enemy Room");
        }
        for (int i = 0; i < 9; i++) {
            roomTypes.Add("Treasure Room");
        }
        for (int i = 0; i < 1; i++) {
            roomTypes.Add("Mimic Room");
        }
        for (int i = 0; i < 1; i++) {
            roomTypes.Add("Break Room");
        }
        for (int i = 0; i < 19; i++) {
            roomTypes.Add("Empty Room");
        }

        for (int i = 0; i < mapLength; i++) {
            for (int j = 0; j < mapLength; j++) {
                if (map[i, j].roomType == null) {
                    int type = Random.Range(0, roomTypes.Count);
                    string room = roomTypes[type];
                    map[i, j].roomType = roomTypes[type];
                    roomTypes.RemoveAt(type);

                    switch (room) {
                        case "Normal Enemy Room":
                            map[i, j].enemiesLeft = 1;
                            break;
                        case "Exit":
                            Debug.Log("EXIT: " + i + "/" + j);
                            map[i, j].enemiesLeft = 1;
                            break;
                        case "Break Room":
                            Debug.Log("BREAK: " + i + "/" + j);
                            break;
                        case "Wizard Room":
                            Debug.Log("WIZARD: " + i + "/" + j);
                            map[i, j].ResetSkills();
                            break;
                    }
                }
            }
        }

        for (int i = 0; i < mapLength * mapLength; i++) {

            if (combos.Any()) {
                var roomCombo = combos[Random.Range(0, combos.Count)];
                setRoom(roomCombo[0], roomCombo[1], "null");
            }

        }

        bool[,] checker = new bool[mapLength, mapLength];
        int check = 0;

        while (check < mapLength * mapLength) {
            check = 0;
            for (int i = 0; i < mapLength; i++) {
                for (int j = 0; j < mapLength; j++) {
                    checker[i, j] = false;
                }
            }
            verifyRooms(row, col, ref checker, ref check);
            Debug.Log(check);
            if (check != mapLength * mapLength) {
                bool changedRoom = false;
                for (int i = 0; i < mapLength && !changedRoom; i++) {
                    for (int j = 0; j < mapLength && !changedRoom; j++) {
                        if (!checker[i, j]) {
                            if (i > 0) {
                                    if (checker[i - 1, j]) {
                                    map[i, j].north = true;
                                    map[i - 1, j].south = true;
                                    changedRoom = true;
                                }
                            }
                            
                            if (i < mapLength - 1) {
                                if (checker[i + 1, j]) {
                                    map[i, j].south = true;
                                    map[i + 1, j].north = true;
                                    changedRoom = true;
                                }
                            }

                            if (j < mapLength - 1) {
                                if (checker[i, j + 1]) {
                                    map[i, j].east = true;
                                    map[i, j + 1].west = true;
                                    changedRoom = true;
                                }
                            }

                            if (j > 0) {
                                if (checker[i, j - 1]) {
                                    map[i, j].west = true;
                                    map[i, j - 1].east = true;
                                    changedRoom = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < mapEasing; i++) {
            List<string> directions = new List<string>();
            int changeRow = Random.Range(0, mapLength);
            int changeCol = Random.Range(0, mapLength);
            if (changeRow > 0) {
                directions.Add("North");
            }
            
            if (changeRow < mapLength - 1) {
                directions.Add("South");
            }

            if (changeCol < mapLength - 1) {
                directions.Add("East");
            }

            if (changeCol > 0) {
                directions.Add("West");
            }

            string direction = directions[Random.Range(0, directions.Count)];
            switch (direction) {
                case "North":
                    map[changeRow, changeCol].north = true;
                    map[changeRow - 1, changeCol].south = true;
                    break;
                case "South":
                    map[changeRow, changeCol].south = true;
                    map[changeRow + 1, changeCol].north = true;
                    break;
                case "East":
                    map[changeRow, changeCol].east = true;
                    map[changeRow, changeCol + 1].west = true;
                    break;
                case "West":
                    map[changeRow, changeCol].west = true;
                    map[changeRow, changeCol - 1].east = true;
                    break;
            }
        }

        updateEnemies = true;
        
    }

    public static bool verifyRooms(int row, int col, ref bool[,] checker, ref int check) {
        if (!checker[row, col]) {
            checker[row, col] = true;
            check++;
            if (map[row, col].north) {
                verifyRooms(row - 1, col, ref checker, ref check);
            }
            if (map[row, col].south) {
                verifyRooms(row + 1, col, ref checker, ref check);
            }
            if (map[row, col].east) {
                verifyRooms(row, col + 1, ref checker, ref check);
            }
            if (map[row, col].west) {
                verifyRooms(row, col - 1, ref checker, ref check);
            }
        }

        return false;
    }

    public static bool setRoom(int row, int col, string blockedDirection) {

        string flag;
        for (int i = 0; i < combos.Count(); i++) {
            if (combos[i][0] == row && combos[i][1] == col) {
                combos.RemoveAt(i);
                break;
            }
        }
        
        List<string> directions = new List<string>();
        if (row > 0 && blockedDirection != "North") {
            directions.Add("North");
        }
        if (row < (mapLength - 1) && blockedDirection != "South") {
            directions.Add("South");
        }
        if (col > 0 && blockedDirection != "West") {
            directions.Add("West");
        }
        if (col < (mapLength - 1) && blockedDirection != "East") {
            directions.Add("East");
        }

        if (!directions.Any()) {
            return false;
        } else {
            string direction = directions[Random.Range(0, directions.Count)];
            switch (direction) {
                case "North":
                    flag = map[row - 1, col].ToString();
                    map[row, col].north = true;
                    map[row - 1, col].south = true;
                    if (flag != null) {
                        return false;
                    }
                    return setRoom(row - 1, col, "South");
                case "South":
                    flag = map[row + 1, col].ToString();
                    map[row, col].south = true;
                    map[row + 1, col].north = true;
                    if (flag != null) {
                        return false;
                    }
                    return setRoom(row + 1, col, "North");
                case "East":
                    flag = map[row, col + 1].ToString();
                    map[row, col].east = true;
                    map[row, col + 1].west = true;
                    if (flag != null) {
                        return false;
                    }
                    return setRoom(row, col + 1, "West");
                case "West":
                    flag = map[row, col - 1].ToString();
                    map[row, col].west = true;
                    map[row, col - 1].east = true;
                    if (flag != null) {
                        return false;
                    }
                    return setRoom(row, col - 1, "East");
            }

        }
        
        return false;

    }

    public static bool isRoomsSet() {
        for (int i = 0; i < mapLength; i++) {
            for (int j = 0; j < mapLength; j++) {
                if (map[i, j].ToString() == null) {
                    return false;
                }
            }
        }
        return true;
    }

    public void updateRoom() {
        if (map[row, col].north) {
            northBlocker.SetActive(false);
        } else {
            northBlocker.SetActive(true);
        }

        if (map[row, col].east) {
            eastBlocker.SetActive(false);
        } else {
            eastBlocker.SetActive(true);
        }

        if (map[row, col].south) {
            southBlocker.SetActive(false);
        } else {
            southBlocker.SetActive(true);
        }

        if (map[row, col].west) {
            westBlocker.SetActive(false);
        } else {
            westBlocker.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z)) {
            showMap = !showMap;
            updateMap();
        }

        updateRoom();

        if (updateEnemies) {
            teleporterArea.SetActive(false);
            wizardArea.SetActive(false);
            breakArea.SetActive(false);
            updateEnemies = false;
            EnemyAI.emptyTrash();
            map[row, col].hasEntered = true;

            switch (map[row, col].roomType) {
                case "Entrance":
                    teleporterArea.SetActive(true);
                    teleporter.GetComponent<Tilemap>().color = new Color32(255, 0, 0, 64);
                    break;
                case "Exit":
                    teleporterArea.SetActive(true);
                    teleporter.GetComponent<Tilemap>().color = new Color32(0, 255, 255, 255);
                    EnemyAI.CreateBoss(originalEnemy, map[row, col].enemiesLeft);
                    break;
                case "Normal Enemy Room":
                    EnemyAI.CreateEnemy(originalEnemy, map[row, col].enemiesLeft);
                    break;
                case "Wizard Room":
                    wizardArea.SetActive(true);
                    break;
                case "Break Room":
                    breakArea.SetActive(true);
                    break;
            }
            
            
            updateMap();

        }
    }

    public void updateMap() {
        if (!showMap) {
            mapObject.SetActive(false);
            return;
        }

        mapObject.SetActive(true);
        while (mapObjects.Any()) {
            GameObject t = mapObjects[0];
            mapObjects.RemoveAt(0);
            Destroy(t);
        }

        floorIndicator.text = "Floor " + floorNumber;

        for (int i = 0; i < mapLength; i++) {
            for (int j = 0; j < mapLength; j++) {
                var room = map[i, j];
                if (room.hasEntered) {

                    if (room.north) {
                        GameObject directionIndicatorClone = Instantiate(directionIndicator, new Vector3(directionIndicator.transform.position.x + (j * 75), directionIndicator.transform.position.y + (i * -75) + (75 / 2)), directionIndicator.transform.rotation);
                        directionIndicatorClone.transform.Rotate(0, 0, 90);
                        directionIndicatorClone.transform.SetParent(directionIndicatorContainer.transform);
                        directionIndicatorClone.name = "directionIndicatorCloneN" + i + "-" + j;
                        directionIndicatorClone.SetActive(true);
                        mapObjects.Add(directionIndicatorClone);
                    }

                    if (room.south) {
                        GameObject directionIndicatorClone = Instantiate(directionIndicator, new Vector3(directionIndicator.transform.position.x + (j * 75), directionIndicator.transform.position.y + (i * -75) - (75 / 2)), directionIndicator.transform.rotation);
                        directionIndicatorClone.transform.Rotate(0, 0, 90);
                        directionIndicatorClone.transform.SetParent(directionIndicatorContainer.transform);
                        directionIndicatorClone.name = "directionIndicatorCloneS" + i + "-" + j;
                        directionIndicatorClone.SetActive(true);
                        mapObjects.Add(directionIndicatorClone);
                    }

                    if (room.east) {
                        GameObject directionIndicatorClone = Instantiate(directionIndicator, new Vector3(directionIndicator.transform.position.x + (j * 75) + (75 / 2), directionIndicator.transform.position.y + (i * -75)), directionIndicator.transform.rotation);
                        directionIndicatorClone.transform.Rotate(0, 0, 0);
                        directionIndicatorClone.transform.SetParent(directionIndicatorContainer.transform);
                        directionIndicatorClone.name = "directionIndicatorCloneE" + i + "-" + j;
                        directionIndicatorClone.SetActive(true);
                        mapObjects.Add(directionIndicatorClone);
                    }

                    if (room.west) {
                        GameObject directionIndicatorClone = Instantiate(directionIndicator, new Vector3(directionIndicator.transform.position.x + (j * 75) - (75 / 2), directionIndicator.transform.position.y + (i * -75)), directionIndicator.transform.rotation);
                        directionIndicatorClone.transform.Rotate(0, 0, 0);
                        directionIndicatorClone.transform.SetParent(directionIndicatorContainer.transform);
                        directionIndicatorClone.name = "directionIndicatorCloneW" + i + "-" + j;
                        directionIndicatorClone.SetActive(true);
                        mapObjects.Add(directionIndicatorClone);
                    }


                    GameObject roomIndicatorClone = Instantiate(roomIndicator, new Vector3(roomIndicatorContainer.transform.position.x + (j * 75), roomIndicatorContainer.transform.position.y + (i * -75)), roomIndicator.transform.rotation);
                    roomIndicatorClone.transform.SetParent(roomIndicatorContainer.transform);
                    roomIndicatorClone.name = "roomIndicatorClone" + i + "-" + j;
                    roomIndicatorClone.SetActive(true);
                    if (i == row && j == col) {
                        roomIndicatorClone.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                    }
                    mapObjects.Add(roomIndicatorClone);
                }
            }
        }
    }

}
