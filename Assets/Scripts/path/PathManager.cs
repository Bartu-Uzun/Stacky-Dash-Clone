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

    // give input direction, return path thats in that direction
    public GameObject GetPath(Path.Direction direction, Path.Direction reverseDirection, PathPlayerController.MovementDirection lastDirection) { 

        for (int i = 0; i < _paths.Count; i++) {

            Path currentPath = _paths[i].GetComponent<Path>();

            if (currentPath.GetDirection() == direction) {

                return _paths[i];
            }

        }
        
        //check if path is two directional and needs a reverse operation
        for (int i = 0; i < _paths.Count; i++) { 

            Path currentPath = _paths[i].GetComponent<Path>();

            if (currentPath.GetIsTwoSided() && currentPath.GetDirection() == reverseDirection && !ArePathsEqual(reverseDirection, lastDirection)) { 

                Debug.Log("reverse path direction: " + reverseDirection + " last movement dir: " + lastDirection + " isEqual: " + ArePathsEqual(reverseDirection, lastDirection));
                currentPath.ReverseDistanceTravelled();
                currentPath.ReverseMovementFactor();

                return _paths[i];

            }
        }

        return null;
    }

    private bool ArePathsEqual(Path.Direction pathDir, PathPlayerController.MovementDirection movementDir) {

        bool flag;

        if (pathDir == Path.Direction.Left && movementDir == PathPlayerController.MovementDirection.Left) {
            flag = true;
        }
        else if (pathDir == Path.Direction.Right && movementDir == PathPlayerController.MovementDirection.Right) {
            flag = true;
        }
        else if (pathDir == Path.Direction.Down && movementDir == PathPlayerController.MovementDirection.Down) {
            flag = true;
        }
        else if (pathDir == Path.Direction.Up && movementDir == PathPlayerController.MovementDirection.Up) {
            flag = true;
        }
        else {
            flag = false;
        }

        return flag;
    }

    public GameObject GetPath(Path.Direction direction, Path.Direction reverseDirection, GameObject pathToCheck) {

        for (int i = 0; i < _paths.Count; i++) {

            Path currentPath = _paths[i].GetComponent<Path>();

            if (currentPath.GetDirection() == direction) {

                return _paths[i];
            }
        }

        for (int i = 0; i < _paths.Count; i++) { 

            Path currentPathComponent = _paths[i].GetComponent<Path>();

            if (currentPathComponent.GetIsTwoSided() && currentPathComponent.GetDirection() == reverseDirection && !_paths[i].Equals(pathToCheck)) { 

                //Debug.Log("current path: " + _paths[i] + "pathToCheck: " + pathToCheck + "isEqual: " + _paths[i].Equals(pathToCheck));

                currentPathComponent.ReverseDistanceTravelled();
                currentPathComponent.ReverseMovementFactor();

                return _paths[i];

            }
        }

        return null;

    }
}
