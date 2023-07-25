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

    private bool _hasStoppedOnBridge = false;
    [SerializeField] private float _currentDistanceTravelled;

    private float _currentPathMovementFactor;

    private GameObject _currentPath;
    [SerializeField] private PathCreator _currentPathCreator;
    private Vector3 _previousPos;

    private MovementDirection _lastMovementDirection;


    public enum MovementDirection {Left, Right, Up, Down}

    private void Awake() {
        
        if (Instance != null) {
            Destroy(this);
        }
        else {
            Instance = this;
        }

        _previousPos = transform.position;

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

        _previousPos = transform.position;
        _currentDistanceTravelled += _speed * _currentPathMovementFactor * Time.deltaTime;
        transform.position = _currentPathCreator.path.GetPointAtDistance(_currentDistanceTravelled, EndOfPathInstruction.Stop);
        //transform.rotation = _currentPathCreator.path.GetRotationAtDistance(_currentDistanceTravelled, EndOfPathInstruction.Stop);

        if (Vector3.Distance(_previousPos, transform.position) <= _threshold) { // player has stopped
            Stop();
        }

    }

    public void SlideOnBridge(GameObject path) {

        if (_currentPath == null) {


            _currentPath = path;

            Path pathComponent = _currentPath.GetComponent<Path>();
                
            _isSliding = true;
            _isMoving = true;

            _currentPathCreator = _currentPath.GetComponent<PathCreator>();
            _currentPathMovementFactor = pathComponent.GetMovementFactor();
            _currentDistanceTravelled = pathComponent.GetDistanceTravelled();

            pathComponent.ReverseMovementFactor();
            pathComponent.ReverseDistanceTravelled();
        }
    }

    public void EndOfBridgeMovement(GameObject edgePath) {

        _currentPath = edgePath;

        Path pathComponent = _currentPath.GetComponent<Path>();

        _isSliding = false;
        _isMoving = true;

        _currentPathCreator = _currentPath.GetComponent<PathCreator>();
        _currentPathMovementFactor = pathComponent.GetMovementFactor();
        _currentDistanceTravelled = pathComponent.GetDistanceTravelled();

        pathComponent.ReverseMovementFactor();
        pathComponent.ReverseDistanceTravelled();


        Path.Direction pathDir = pathComponent.GetDirection();
        if (pathDir == Path.Direction.Down) {
            pathComponent.SetDirection(Path.Direction.Up);
        }
        else if (pathDir == Path.Direction.Up) {
            pathComponent.SetDirection(Path.Direction.Down);
        }
        else if (pathDir == Path.Direction.Right) {
            pathComponent.SetDirection(Path.Direction.Left);
        }
        else if (pathDir == Path.Direction.Left) {
            pathComponent.SetDirection(Path.Direction.Right);
        }
        
    }

    public void Stop() {

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
    public void BridgeStop(GameObject path) {
        
        _shouldGoDown = false;
        _shouldGoLeft = false;
        _shouldGoRight = false;
        _shouldGoUp = false;

        _isMoving = false;
        _isSliding = false;

        // We preserve the _currentPath
        
        _currentPath = path;
        _currentPathCreator = _currentPath.GetComponent<PathCreator>();

        _hasStoppedOnBridge = true;
    }

    private void ReadInput()
    {

        if (!_isMoving) {

            if (!_hasStoppedOnBridge) { //player did not stop on the bridge

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
            else { // player has stopped on a bridge

                if (_lastMovementDirection == MovementDirection.Right && !_shouldGoLeft) { // if last movement was right, player can move left

                    _shouldGoLeft = Input.GetKeyDown(KeyCode.LeftArrow) || MobileInput.Instance.swipeLeft;
                }
                if (_lastMovementDirection == MovementDirection.Left && !_shouldGoRight) { // if last movement was left, player can move right
                    
                    _shouldGoRight = Input.GetKeyDown(KeyCode.RightArrow) || MobileInput.Instance.swipeRight;
                }
                if (_lastMovementDirection == MovementDirection.Up && !_shouldGoDown) { // if last movement was up, player can move down

                    _shouldGoDown = Input.GetKeyDown(KeyCode.DownArrow) || MobileInput.Instance.swipeDown;
                }
                if (_lastMovementDirection == MovementDirection.Down && !_shouldGoUp) { // if last movement was down, player can move up

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

            if (_hasStoppedOnBridge) {

                if (_shouldGoLeft) {

                    Path pathComponent = _currentPath.GetComponent<Path>();
                    _currentPathMovementFactor = pathComponent.GetMovementFactor();
                    _currentDistanceTravelled = pathComponent.GetDistanceTravelled();

                    pathComponent.ReverseMovementFactor();
                    pathComponent.ReverseDistanceTravelled();

                    _isMoving = true;
                    _isSliding = true;
                    _shouldGoLeft = false;
                    _hasStoppedOnBridge = false;

                }
                if (_shouldGoRight) {

                    Path pathComponent = _currentPath.GetComponent<Path>();
                    _currentPathMovementFactor = pathComponent.GetMovementFactor();
                    _currentDistanceTravelled = pathComponent.GetDistanceTravelled();

                    pathComponent.ReverseMovementFactor();
                    pathComponent.ReverseDistanceTravelled();

                    _isMoving = true;
                    _isSliding = true;
                    _shouldGoRight = false;
                    _hasStoppedOnBridge = false;

                }
                if (_shouldGoDown) {

                    Path pathComponent = _currentPath.GetComponent<Path>();
                    _currentPathMovementFactor = pathComponent.GetMovementFactor();
                    _currentDistanceTravelled = pathComponent.GetDistanceTravelled();

                    pathComponent.ReverseMovementFactor();
                    pathComponent.ReverseDistanceTravelled();

                    _isMoving = true;
                    _isSliding = true;
                    _shouldGoDown = false;
                    _hasStoppedOnBridge = false;

                }
                if (_shouldGoUp) {

                    Path pathComponent = _currentPath.GetComponent<Path>();
                    _currentPathMovementFactor = pathComponent.GetMovementFactor();
                    _currentDistanceTravelled = pathComponent.GetDistanceTravelled();

                    pathComponent.ReverseMovementFactor();
                    pathComponent.ReverseDistanceTravelled();

                    _isMoving = true;
                    _isSliding = true;
                    _shouldGoUp = false;
                    _hasStoppedOnBridge = false;

                }

            }
            else {

                // find movement direction, set path accordingly, update path's allowed direction
                if (_shouldGoLeft) {

                    _lastMovementDirection = MovementDirection.Left; // save lastPressedKey value

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

                    _lastMovementDirection = MovementDirection.Right; // save lastPressedKey value

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

                    _lastMovementDirection = MovementDirection.Down; // save lastPressedKey value

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

                    _lastMovementDirection = MovementDirection.Up; // save lastPressedKey value

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
    }

    public bool GetIsSliding() {

        return _isSliding;
    }
    public bool GetHasStoppedOnBridge() {

        return _hasStoppedOnBridge;
    }
}
