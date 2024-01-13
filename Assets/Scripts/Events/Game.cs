using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    public static List<Character> characters = new List<Character>();
    public static Character walter = new Character("Walter", 1, 100, 15, 10, 10, 5, 12);
    public static Character benedict = new Character("Benedict", 1, 95, 10, 12, 11, 8, 8);
    public static Character sherri = new Character("Sherri", 1, 120, 7 * 100, 12, 12, 8, 3);
    public static Character jade = new Character("Jade", 1, 150, 5, 18, 9, 2, 3);

    public static bool firstRun = false;
    
    public static bool gameMovementFreeze = false;

    public static string gameEvent = "Roaming";
    
    public GameObject originalEnemy;
    public static bool updateEnemies = false;

    public static int mapLength = 7;
    public static int row = 0;
    public static int col = 0;
    public static int floorNumber = 0;
    public static int disruptions = 0;
    public static int gold = 0;

    public static Room[,] map = new Room[7, 7];
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
                c.skills.Add(SkillsRegistry.getSkill("Test"));
                c.skills.Add(SkillsRegistry.getSkill("Ally Thing"));
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
        
        for (int i = 0; i < mapLength; i++) {
            for (int j = 0; j < mapLength; j++) {
                if (map[i, j].ToString() == null) {
                    map[i, j].Reset();
                    int[] c = {i, j};
                    combos.Add(c);
                }
            }
        }

        row = Random.Range(0, mapLength);
        col = Random.Range(0, mapLength);
        setRoom(row, col, "null");
        setRoom(row, col, "null");
        setRoom(row, col, "null");

        for (int i = 0; i < mapLength * mapLength; i++) {

            if (combos.Any()) {
                var roomCombo = combos[Random.Range(0, combos.Count)];
                setRoom(roomCombo[0], roomCombo[1], "null");
            }

        }

        updateEnemies = true;
        

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
        Debug.Log("ROOMS SET!");
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
            EnemyAI.emptyTrash();
            EnemyAI.CreateEnemy(originalEnemy, map[row, col].enemiesLeft);
            updateEnemies = false;
            map[row, col].hasEntered = true;
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
