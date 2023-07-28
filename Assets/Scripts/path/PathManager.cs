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

        GameObject fitPath1 = null;
        GameObject fitPath2 = null;
        for (int i = 0; i < _paths.Count; i++) {

            Path currentPath = _paths[i].GetComponent<Path>();

            if (currentPath.GetDirection() == direction) {

                if (fitPath1 == null) {

                    fitPath1 = _paths[i];
                }
                else {

                    fitPath2 = _paths[i];
                    break;
                }
            }

        }

        if (fitPath1 != null && fitPath2 != null) {

            GameObject priorityPath = CheckPriority(fitPath1, fitPath2, direction);

            return priorityPath;
        }

        else if (fitPath1 != null && fitPath2 == null) {

            return fitPath1;
        }
        
        //check if path is two directional and needs a reverse operation
        for (int i = 0; i < _paths.Count; i++) { 

            Path currentPath = _paths[i].GetComponent<Path>();

            if (currentPath.GetIsTwoSided() && currentPath.GetDirection() == reverseDirection && !ArePathsEqual(reverseDirection, lastDirection)) { 

                //Debug.Log("reverse path direction: " + reverseDirection + " last movement dir: " + lastDirection + " isEqual: " + ArePathsEqual(reverseDirection, lastDirection));
                
                //currentPath.ReverseDistanceTravelled();
                //currentPath.ReverseMovementFactor();

                if (fitPath1 == null) {
                    fitPath1 = _paths[i];

                    
                }
                else {
                    fitPath2 = _paths[i];

                    //Debug.Log("fitPath1: " + fitPath1 + " fitPath2: " + fitPath2);
                    break;
                }

            }
        }

        if (fitPath1 != null && fitPath2 != null) {

            GameObject priorityPath = CheckPriority(fitPath1, fitPath2, direction);

            //Debug.Log("priority path: " + priorityPath);

            Path pathComponent = priorityPath.GetComponent<Path>();

            pathComponent.ReverseDistanceTravelled();
            pathComponent.ReverseMovementFactor();

            return priorityPath;
        }

        else if (fitPath1 != null && fitPath2 == null) {

            Path pathComponent = fitPath1.GetComponent<Path>();

            pathComponent.ReverseDistanceTravelled();
            pathComponent.ReverseMovementFactor();

            return fitPath1;
        }



        return null;
    }

    private GameObject CheckPriority(GameObject path1, GameObject path2, Path.Direction direction) {

        //Debug.Log("direction: " + direction);

        Path pathComponent1 = path1.GetComponent<Path>();
        Path pathComponent2 = path2.GetComponent<Path>();

        if (pathComponent1.GetHasPriority(path2 ,direction)) {
            return path1;
        }
        else {
            return path2;
        }
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

    // called when checking for a follow-up
    public GameObject GetPath(Path.Direction direction, Path.Direction reverseDirection, GameObject pathToCheck) {

        for (int i = 0; i < _paths.Count; i++) {

            Path currentPath = _paths[i].GetComponent<Path>();

            if (currentPath.GetDirection() == direction) {

                //Debug.Log("i follow u");

                return _paths[i];
            }
        }

        for (int i = 0; i < _paths.Count; i++) { 

            Path currentPathComponent = _paths[i].GetComponent<Path>();

            if (currentPathComponent.GetIsTwoSided() && currentPathComponent.GetDirection() == reverseDirection && !_paths[i].Equals(pathToCheck)) { 

                Debug.Log("follow reverse");
                Debug.Log("current path: " + _paths[i] + "pathToCheck: " + pathToCheck + " reverseDirection: " + reverseDirection);

                currentPathComponent.ReverseDistanceTravelled();
                currentPathComponent.ReverseMovementFactor();

                return _paths[i];

            }
        }

        return null;

    }

    private bool HasPriorityOver(GameObject pathToCheck, GameObject currentPath, Path.Direction direction) {


        Path pathComponent = pathToCheck.GetComponent<Path>();

        bool flag = pathComponent.GetHasPriority(currentPath, direction);

        Debug.Log("im called. pathToCheck: " + pathToCheck + " currentPath: " + currentPath + " direction: " + direction + " HasPriority: " + flag);


        return flag;


    }
}
