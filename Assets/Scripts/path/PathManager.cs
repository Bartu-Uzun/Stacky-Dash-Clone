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

    //isFollowUp: if GetPath is called when game is looking for a FollowUp path, then bidirection should not be checked as it will cause a loop
    public GameObject GetPath(Path.Direction direction, Path.Direction reverseDirection, bool isFollowUp) { // give input direction, return path thats in that direction

        /* if _paths has 2 paths with the same Direction, player is on a biDirectional path (meaning player can go both left & right, or up & down)
        ** if this same Direction is the reverseDirection, then player is trying to move on this biDirectional path
        */
        int reverseDirectionCount = 0;

        for (int i = 0; i < _paths.Count; i++) {

            Path currentPath = _paths[i].GetComponent<Path>();

            if (currentPath.GetDirection() == direction) {

                return _paths[i];
            }
            else if (currentPath.GetDirection() == reverseDirection) {

                reverseDirectionCount++;
            }
        }
        
        if (!isFollowUp && reverseDirectionCount == 2) {

            //check if path is two directional and needs a reverse operation
            for (int i = 0; i < _paths.Count; i++) { 

                Path currentPath = _paths[i].GetComponent<Path>();

                if (currentPath.GetIsTwoSided() && currentPath.GetDirection() == reverseDirection) { 

                    currentPath.ReverseDistanceTravelled();
                    currentPath.ReverseMovementFactor();

                    return _paths[i];

                }
            }
        }

        return null;
    }
}
