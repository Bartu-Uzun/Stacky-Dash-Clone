using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathManager : MonoBehaviour
{

    public static PathManager Instance;


    // each _path object contains the component PathCreator, and an enum value direction
    // size of _paths should never be greater than 4
    [SerializeField] private List<GameObject> _paths; 

    private void Awake() {
        
        if (Instance != null) {
            Destroy(this);
        }
        else {
            Instance = this;
        }

        _paths = new List<GameObject>();
    }
    public void AddPath(GameObject path) {

        if (!path.GetComponent<Path>().GetIsCollected()) { //if path is not already in the list

            _paths.Add(path);

            path.GetComponent<Path>().SetIsCollected(true);

        }

        

    }

    public void RemovePath(GameObject path) {

        if (path.GetComponent<Path>().GetIsCollected()) { //if path is in the list

            _paths.Remove(path);

            path.GetComponent<Path>().SetIsCollected(false);

        }

        
    }

    public List<GameObject> GetPaths() {

        return _paths;
    }

    public GameObject GetPath(Path.Direction direction) { // give input direction, return path thats in that direction


        for (int i = 0; i < _paths.Count; i++) {

            //Debug.Log("name: " + _paths[i].name + " direction: " + _paths[i].GetComponent<Path>().GetDirection());

            if (_paths[i].GetComponent<Path>().GetDirection() == direction) {

                //Debug.Log("found ya'");

                return _paths[i];
            }
        }

        return null;
    }
}
