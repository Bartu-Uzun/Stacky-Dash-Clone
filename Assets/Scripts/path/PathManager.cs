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

    // add path to list whenever player touches the path
    public void AddPath(GameObject path) {

        if (!path.GetComponent<Path>().GetIsCollected()) { //if path is not already in the list

            _paths.Add(path);

            path.GetComponent<Path>().SetIsCollected(true);

        }

        

    }

    // remove path from list whenever player loses contact with the path
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

        /* if a path matches with the input direction, fitPath1 points at it
        ** if another path also matches with the direction, fitPath2 points at it
        */
        GameObject fitPath1 = null;
        GameObject fitPath2 = null;
        for (int i = 0; i < _paths.Count; i++) {

            GameObject currentPath = _paths[i];

            Path currentPathComponent = currentPath.GetComponent<Path>();

            if (currentPathComponent.GetDirection() == direction) {

                if (fitPath1 == null) {

                    fitPath1 = currentPath;
                }
                else {

                    fitPath2 = currentPath;
                    break;
                }
            }
        }

        // if both fitPath1 and fitPath2 points at a path, compare the two paths to see which one has a priority in the input direction
        if (fitPath1 != null && fitPath2 != null) {

            GameObject priorityPath = CheckPriority(fitPath1, fitPath2, direction);

            return priorityPath;
        }

        // if fitPath1 is not null but fitPath2 is null, then just return fitPath1
        else if (fitPath1 != null && fitPath2 == null) {

            return fitPath1;
        }

        // if both fitPaths are null, check if there is any bi-directional path that points to the reverse direction, similar to the previous process
        for (int i = 0; i < _paths.Count; i++) { 

            GameObject currentPath = _paths[i];
            Path currentPathComponent = _paths[i].GetComponent<Path>();

            if (currentPathComponent.GetIsTwoSided() && currentPathComponent.GetDirection() == reverseDirection && !AreDirectionsEqual(reverseDirection, lastDirection)) { 

                if (fitPath1 == null) {
                    fitPath1 = currentPath;

                    
                }
                else {
                    fitPath2 = currentPath;
                    break;
                }

            }
        }

        if (fitPath1 != null && fitPath2 != null) {

            GameObject priorityPath = CheckPriority(fitPath1, fitPath2, direction);

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

    // given two paths and a direction, returns the path that has the priority in that direction
    private GameObject CheckPriority(GameObject path1, GameObject path2, Path.Direction direction) {

        Path pathComponent1 = path1.GetComponent<Path>();
        Path pathComponent2 = path2.GetComponent<Path>();

        if (pathComponent1.GetHasPriority(path2 ,direction)) {
            return path1;
        }
        else {
            return path2;
        }
    }

    // compares Path.Direction and PathPlayerController.MovementDirection enums to see if they point to the same direction
    private bool AreDirectionsEqual(Path.Direction pathDir, PathPlayerController.MovementDirection movementDir) {

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

    // called when checking for a follow-up path
    public GameObject GetPath(Path.Direction direction, Path.Direction reverseDirection, GameObject pathToCheck) {

        // if there is a path that points the direction, return it as the follow-up
        for (int i = 0; i < _paths.Count; i++) {

            GameObject currentPath = _paths[i];

            Path currentPathComponent = currentPath.GetComponent<Path>();

            if (currentPathComponent.GetDirection() == direction) {

                return currentPath;
            }
        }

        // else, check if there is a bi-directional path (that is not the input pathToCheck) that points to the reverse direction
        for (int i = 0; i < _paths.Count; i++) {

            GameObject currentPath = _paths[i];

            Path currentPathComponent = currentPath.GetComponent<Path>();

            if (currentPathComponent.GetIsTwoSided() && currentPathComponent.GetDirection() == reverseDirection && !currentPath.Equals(pathToCheck) && !pathToCheck.GetComponent<Path>().GetHasPriority(currentPath, direction)) { 

                Debug.Log("follow reverse");
                Debug.Log("current path: " + currentPath + "pathToCheck: " + pathToCheck + " reverseDirection: " + reverseDirection);

                currentPathComponent.ReverseDistanceTravelled();
                currentPathComponent.ReverseMovementFactor();

                return currentPath;

            }
        }

        return null;

    }
}
