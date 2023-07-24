using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathPlayerController : MonoBehaviour
{

    public static PathPlayerController Instance;

    [SerializeField] private float _speed = 5;
    
    private float _threshold = 0.005f;

    [SerializeField] private bool _shouldGoLeft = false;
    [SerializeField] private bool _shouldGoRight = false;
    [SerializeField] private bool _shouldGoUp = false;
    [SerializeField] private bool _shouldGoDown = false;

    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isSliding = false;

    private bool _stoppedOnBridge = false;
    [SerializeField] private float _currentDistanceTravelled;

    private float _currentPathMovementFactor;

    private GameObject _currentPath;
    [SerializeField] private PathCreator _currentPathCreator;
    private Vector3 previousPos;

    private void Awake() {
        
        if (Instance != null) {
            Destroy(this);
        }
        else {
            Instance = this;
        }

        previousPos = transform.position;

        //DontDestroyOnLoad(gameObject); //NEED TO CHECK
    }

    /* Lesson: FixedUpdate is unreliable when reading functions like Input.GetKeyDown. To read them, we still
    ** need to use Update function so that the game does not miss any inputs.
    */
    private void Update()
    {
        if (!GameManager.Instance.GetLevelFinished()) {
            ReadInput();
        }

        if (_isMoving && _currentPathCreator != null) {
            Move();

        }
        

    }

    private void Move() {

        previousPos = transform.position;
        _currentDistanceTravelled += _speed * _currentPathMovementFactor * Time.deltaTime;
        transform.position = _currentPathCreator.path.GetPointAtDistance(_currentDistanceTravelled, EndOfPathInstruction.Stop);
        //transform.rotation = _currentPathCreator.path.GetRotationAtDistance(_currentDistanceTravelled, EndOfPathInstruction.Stop);


        if (Vector3.Distance(previousPos, transform.position) <= _threshold) { // player has stopped
            Stop();
        }

    }

    public void SlideOnBridge(GameObject path) {

        Debug.Log("i slide on: " + path.gameObject.transform.parent.name);

        _currentPath = path;

        Path pathComponent = _currentPath.GetComponent<Path>();
        // pathComponent.SetDirection(Path.Direction.Right); might need a new enum val for bridges that stays constant
            
        _isSliding = true;
        _isMoving = true;

        _currentPathCreator = _currentPath.GetComponent<PathCreator>();
        _currentPathMovementFactor = pathComponent.GetMovementFactor();
        _currentDistanceTravelled = pathComponent.GetDistanceTravelled();

        pathComponent.ReverseMovementFactor();
        pathComponent.ReverseDistanceTravelled();
    }

    public void Stop() {

        //Debug.Log("STOPP");

        _shouldGoDown = false;
        _shouldGoLeft = false;
        _shouldGoRight = false;
        _shouldGoUp = false;

        _isMoving = false;
        _isSliding = false;

        
        
        _currentPath = null;
        _currentPathCreator = null;
    }

    // called whenever player should stop on a bridge
    public void BridgeStop() {
         _shouldGoDown = false;
        _shouldGoLeft = false;
        _shouldGoRight = false;
        _shouldGoUp = false;

        _isMoving = false;

        _currentPathCreator = null;

        //might use distance travelled to start back from the bridge??

        _stoppedOnBridge = true;
    }

    private void ReadInput()
    {

        if (!_isMoving) {

            if (!_stoppedOnBridge) { //player did not stop on the bridge

                //read inputs
                if (!_shouldGoLeft)
                {
                    _shouldGoLeft = Input.GetKeyDown(KeyCode.LeftArrow) || MobileInput.Instance.swipeLeft;
                }
                if (!_shouldGoRight)
                {

                    _shouldGoRight = Input.GetKeyDown(KeyCode.RightArrow) || MobileInput.Instance.swipeRight;
                }
                if (!_shouldGoDown)
                {

                    _shouldGoDown = Input.GetKeyDown(KeyCode.DownArrow) || MobileInput.Instance.swipeDown;
                }
                if (!_shouldGoUp)
                {

                    _shouldGoUp = Input.GetKeyDown(KeyCode.UpArrow) || MobileInput.Instance.swipeUp;
                }

            }            

        }
        
    }

    private void FixedUpdate() {
        
        SetPath();
    }

    private void SetPath() {


        if (!_isMoving) {

            // find movement direction, set path accordingly, update path's allowed direction
            if (_shouldGoLeft) {

                _currentPath = PathManager.Instance.GetPath(Path.Direction.Left);

                if (_currentPath != null) {
                    Path pathComponent = _currentPath.GetComponent<Path>();
                    pathComponent.SetDirection(Path.Direction.Right);
                    
                    
                    _isMoving = true;

                    _currentPathCreator = _currentPath.GetComponent<PathCreator>();
                    _currentPathMovementFactor = pathComponent.GetMovementFactor();
                    _currentDistanceTravelled = pathComponent.GetDistanceTravelled();

                    pathComponent.ReverseMovementFactor();
                    pathComponent.ReverseDistanceTravelled();
                }
                

                _shouldGoLeft = false;
            }
            if (_shouldGoRight) {

                _currentPath = PathManager.Instance.GetPath(Path.Direction.Right);

                if (_currentPath != null) {
                    Path pathComponent = _currentPath.GetComponent<Path>();
                    pathComponent.SetDirection(Path.Direction.Left);
                    _isMoving = true;

                    _currentPathCreator = _currentPath.GetComponent<PathCreator>();
                    _currentPathMovementFactor = pathComponent.GetMovementFactor();
                    _currentDistanceTravelled = pathComponent.GetDistanceTravelled();

                    pathComponent.ReverseMovementFactor();
                    pathComponent.ReverseDistanceTravelled();
                }
                

                _shouldGoRight = false;
            }
            if (_shouldGoDown) {

                _currentPath = PathManager.Instance.GetPath(Path.Direction.Down);

                if (_currentPath != null) {
                    Path pathComponent = _currentPath.GetComponent<Path>();
                    pathComponent.SetDirection(Path.Direction.Up);
                    _isMoving = true;

                    _currentPathCreator = _currentPath.GetComponent<PathCreator>();
                    _currentPathMovementFactor = pathComponent.GetMovementFactor();
                    _currentDistanceTravelled = pathComponent.GetDistanceTravelled();

                    pathComponent.ReverseMovementFactor();
                    pathComponent.ReverseDistanceTravelled();
                }

                _shouldGoDown = false;
                
            }
            if (_shouldGoUp) {

                _currentPath = PathManager.Instance.GetPath(Path.Direction.Up);

                if (_currentPath != null) {
                    Path pathComponent = _currentPath.GetComponent<Path>();
                    pathComponent.SetDirection(Path.Direction.Down);
                    _isMoving = true;

                    _currentPathCreator = _currentPath.GetComponent<PathCreator>();
                    _currentPathMovementFactor = pathComponent.GetMovementFactor();
                    _currentDistanceTravelled = pathComponent.GetDistanceTravelled();

                    pathComponent.ReverseMovementFactor();
                    pathComponent.ReverseDistanceTravelled();
                }

                _shouldGoUp = false;
                
            }
            
            
        }
    }

    public bool GetIsSliding() {

        return _isSliding;
    }
}
