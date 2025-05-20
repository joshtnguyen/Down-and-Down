using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Disruptions {
    public string name;
    public int stacks;
    public int maxStacks;

    public Disruptions(string n, int s, int m) {
        name = n;
        stacks = s;
        maxStacks = m;
    }
}

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
    public static int mapEasing = 13;
    public static int row = 0;
    public static int col = 0;
    public static int floorNumber = -1;
    public static int gold = 20;
    public static List<Gear> gear = new List<Gear>();

    // Disruptions
    public static List<Disruptions> disruptions = new List<Disruptions>();
    public static int disruptionCount = -3;

    // FLOOR INFO
    public static int steps = 0;
    public static int enemiesKilled = 0;
    public static int timesDowned = 0;

    public static Room[,] map = new Room[mapLength, mapLength];
    public static List<int[]> combos = new List<int[]>();
    public static List<string> enemies = new List<string>();
    public static string[] enemyList = {"Slime", "Cave Bull", "Sandbat", "Koy Vamp", "Sledger", "Big Tox", "Unskilled"};

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
    public GameObject shrineArea;
    public GameObject gearArea;

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

            foreach (Character c in characters)
            {
                c.skills.Add(SkillsRegistry.getSkill("Attack"));
                c.skills.Add(SkillsRegistry.getSkill("Block"));
                c.verifyMod();
                switch (c.character)
                {
                    case "Walter":
                        Skills s1 = SkillsRegistry.getSkill("Heavy Slice");
                        c.skills.Add(s1);
                        c.skill1 = s1;
                        break;
                    case "Benedict":
                        Skills s2 = SkillsRegistry.getSkill("Fist to the Face");
                        c.skills.Add(s2);
                        c.skill1 = s2;
                        break;
                    case "Sherri":
                        Skills s3 = SkillsRegistry.getSkill("Motivating Touch");
                        c.skills.Add(s3);
                        c.skill1 = s3;
                        break;
                    case "Jade":
                        Skills s4 = SkillsRegistry.getSkill("Loving Meal");
                        c.skills.Add(s4);
                        c.skill1 = s4;
                        break;
                }
            }

            for (int i = 0; i < mapLength; i++)
            {
                for (int j = 0; j < mapLength; j++)
                {
                    map[i, j] = new Room();
                }
            }

            disruptions.Add(new Disruptions("Imprisonment", 0, 100));
            disruptions.Add(new Disruptions("Enemy Duplication", 0, -1));
            disruptions.Add(new Disruptions("Enemy Corruption", 0, 45));
            disruptions.Add(new Disruptions("Enemy Pack", 0, 250));
            disruptions.Add(new Disruptions("Strengthening", 0, -1));
            disruptions.Add(new Disruptions("Polishing", 0, -1));
            disruptions.Add(new Disruptions("Heartiness", 0, -1));
            disruptions.Add(new Disruptions("Bleed", 0, 200));
            disruptions.Add(new Disruptions("Freeze", 0, 200));
            disruptions.Add(new Disruptions("Energy Drain", 0, 125));

            createFloor();

        }
        
        gameEvent = "Roaming";
    }

    public static Disruptions getDisruption(string disruption) {
        foreach(Disruptions d in disruptions) {
            if (d.name == disruption) {
                return d;
            }
        }
        return null;
    }

    public static int getEnemyCount() {
        Disruptions d = getDisruption("Enemy Pack");
        return convertToLoops(d.stacks * 2) + 1;
    }

    public static int convertToLoops(double chance) {
        var rand = new System.Random();
        int times = 0;
        while (chance >= 100) {
            chance -= 100;
            times++;
        }
        if ((rand.NextDouble() * 100.0) < chance) {
            times++;
        }
        return times;
    }

    public static void createFloor() {

        steps = 0;
        enemiesKilled = 0;
        timesDowned = 0;
        disruptionCount += 3;

        List<Disruptions> usableDisruptions = new List<Disruptions>();
        foreach(Disruptions d in disruptions) {
            usableDisruptions.Add(d);
            d.stacks = 0;
        }

        for (int i = 0; i < disruptionCount; i++) {
            if (usableDisruptions.Any()) {
                int d = UnityEngine.Random.Range(0, usableDisruptions.Count);
                usableDisruptions[d].stacks++;
                if (usableDisruptions[d].maxStacks >= 0 && usableDisruptions[d].stacks >= usableDisruptions[d].maxStacks) {
                    usableDisruptions.RemoveAt(d);
                }
            }
        }

        enemies.Clear();
        for (int i = 0; i < 3; i++) {
            enemies.Add(enemyList[Random.Range(0, enemyList.Length)]);
        }
        
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
        for (int i = 0; i < 3; i++) {
            roomTypes.Add("Shrine Room");
        }
        for (int i = 0; i < 1; i++) {
            roomTypes.Add("Demon Room");
        }
        for (int i = 0; i < 1; i++) {
            roomTypes.Add("Break Room");
        }
        for (int i = 0; i < 1; i++) {
            roomTypes.Add("Gear Room");
        }
        for (int i = 0; i < 24; i++) {
            roomTypes.Add("Empty Room");
        }

        for (int i = 0; i < mapLength; i++) {
            for (int j = 0; j < mapLength; j++) {
                if (map[i, j].roomType == null) {
                    int type = Random.Range(0, roomTypes.Count);
                    string room = roomTypes[type];
                    map[i, j].roomType = roomTypes[type];
                    roomTypes.RemoveAt(type);

                    var rand = new System.Random();
                    
                    // IMPRISONMENT
                    if ((rand.NextDouble() * 100.0) < getDisruption("Imprisonment").stacks) {
                        map[i, j].imprisonment = true;
                    }

                    // ENEMY CORRUPTION
                    if (map[i, j].roomType == "Empty Room") {
                        if ((rand.NextDouble() * 100.0) < getDisruption("Enemy Corruption").stacks * 2) {
                            map[i, j].roomType = "Normal Enemy Room";
                        }
                    }

                    switch (map[i, j].roomType) {
                        case "Normal Enemy Room":
                            Disruptions enemyDuplication = getDisruption("Enemy Duplication");
                            map[i, j].enemiesLeft = 1 + convertToLoops(enemyDuplication.stacks * 8);
                            break;
                        case "Exit":
                            map[i, j].enemiesLeft = 1;
                            break;
                        case "Break Room":
                            break;
                        case "Wizard Room":
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

        if (Input.GetKeyDown(KeyCode.K)) {
            for (int i = 0; i < mapLength; i++) {
                for (int j = 0; j < mapLength; j++) {
                    map[i, j].hasEntered = true;
                }
            }
        }

        updateRoom();

        if (updateEnemies) {
            teleporterArea.SetActive(false);
            wizardArea.SetActive(false);
            breakArea.SetActive(false);
            shrineArea.SetActive(false);
            gearArea.SetActive(false);
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
                case "Shrine Room" or "Demon Room":
                    shrineArea.SetActive(true);
                    break;
                case "Gear Room":
                    gearArea.SetActive(true);
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

                    switch (room.roomType) {
                        case "Entrance" or "Exit":
                            roomIndicatorClone.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
                            break;
                        case "Wizard Room":
                            roomIndicatorClone.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                            break;
                        case "Break Room" or "Gear Room":
                            roomIndicatorClone.GetComponent<Image>().color = new Color32(255, 0, 255, 255);
                            break;
                        case "Shrine Room" or "Demon Room":
                            roomIndicatorClone.GetComponent<Image>().color = new Color32(255, 255, 0, 255);
                            break;   
                        case "Normal Enemy Room":
                            if (room.enemiesLeft > 0) {
                                roomIndicatorClone.GetComponent<Image>().color = new Color32(141, 141, 141, 255);
                            }
                            break;
                    }

                    if (i == row && j == col) {
                        roomIndicatorClone.GetComponent<Image>().color = new Color32(0, 141, 0, 255);
                    }
                    mapObjects.Add(roomIndicatorClone);
                }
            }
        }
    }

}
