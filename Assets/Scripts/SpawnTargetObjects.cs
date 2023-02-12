using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTargetObjects : MonoBehaviour
{ 
    // Loads all prefabs from the directory
    Object[] givenPrefabsList;
    Dictionary<string, GameObject> givenPrefabsDictionary = new Dictionary<string, GameObject>();//Convert the Prefabs fetched with file names as key
    List<string> targetPrefabsList = new List<string> { "Cat",/**  "Chicken", "Flower", "Lion", "Penguin", "Tree"*/"Dog" };//Hardcoding the names of the target object prefabs that we want to spawn
    List<string> targetPrefabsToSpawn = new List<string>(); //Initializes the actual target objects which will be spawned

    int numberOfTargetPrefabsToSpawn; //Fetches from settings the no of target objects
    [SerializeField]
    List<GameObject> spawnLocationRefs; //Placeholders for location references in the World by empty Game Objects

    private void Awake()
    {
        // Loads all prefabs from the directory
        givenPrefabsList = Resources.LoadAll("TargetObjectPrefabs", typeof(GameObject));
        numberOfTargetPrefabsToSpawn = ExperimentSettings.targetObjectsCount; //Fetches from settings the no of target objects
        LoadPrefabs();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //Loads Prefabs from FS and adds the pool of prefabs names from which the random taget GameObjects are to be selected
    void LoadPrefabs()
    {
        //Converts all the fetched prefabs into a dictionary
        foreach (var item in givenPrefabsList)
        {
            givenPrefabsDictionary.Add(item.name, (GameObject)item);
        }

        int i = 0;
        //Feeds the list of the targetObjects choice from which we have to spawn
        while(i < numberOfTargetPrefabsToSpawn)
        {
            int val = Random.Range(0, targetPrefabsList.Count);
            if (!targetPrefabsToSpawn.Contains(targetPrefabsList[val]))
            {
                targetPrefabsToSpawn.Add(targetPrefabsList[val]);
                i++;
            }
        }
        SpawnTargetPrefabsOnLocation();
    }
    //Spawns the target Objects on the locations
    public void SpawnTargetPrefabsOnLocation()
    {
        for (int i = 0; i < targetPrefabsToSpawn.Count; i++)
        {
            GameObject go;
            givenPrefabsDictionary.TryGetValue(targetPrefabsToSpawn[i], out go);//Fetching value by key (name of prefab) from dictionary
            GameObject spawned = Instantiate(go, spawnLocationRefs[i].transform.position, Quaternion.identity);// Spawns the prefab into the referenced location
            spawned.name = go.name;//Changes the name for recording properly in Logs
            spawned.AddComponent<TargetEndPointProperty>().direction = spawnLocationRefs[i].GetComponent<TargetEndPointProperty>().direction;//Adds the property of TargetEndPoint script and assigns the direction as that of the target end point anchor direction to which it is referenced
        }
    }
}
