using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public static int floorNumber = 0;
    public static int disruptions = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameEvent = "Roaming";
        EnemyAI.CreateEnemy(originalEnemy, Room.enemiesLeft);

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

        }

    }

    // Update is called once per frame
    void Update()
    {
    }

}
