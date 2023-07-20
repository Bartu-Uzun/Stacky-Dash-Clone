using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    public static CamFollowPlayer Instance;

    [SerializeField] private GameObject _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _lerpTime;
    private float _xPos = 16.65f;

    private bool _isOnBridge = false;


    private Vector3 _destination;

    private void Awake() {
        
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(base.gameObject);
        }
    }

    private void Start()
    {
        InitializeCamPos();
    }

    private void InitializeCamPos()
    {
        _destination = new Vector3(_xPos, _target.transform.position.y + _offset.y, _target.transform.position.z + _offset.z);
        transform.position = Vector3.Lerp(transform.position, _destination, _lerpTime);
    }


    // Update is called once per frame
    void LateUpdate()
    {
        UpdateCamPos();

    }

    private void UpdateCamPos()
    {
        // TODO: make camera stand still when player is on platform, and follow along when they on the bridge

        // if (_isOnBridge) { //if player is on bridge, follow along in the x axis as well
        //     _destination.x = _target.transform.position.x + _offset.x;
        // }
        // else { //if not, x position of the camera stands still
        //     _destination.x = _xPos;
        // }


        _destination.x = _target.transform.position.x + _offset.x;
        _destination.y = this.transform.position.y;
        _destination.z = _target.transform.position.z + _offset.z;

        transform.position = Vector3.Lerp(transform.position, _destination, _lerpTime);
    }

    public void SetIsOnBridge(bool isOnBridge) {
        _isOnBridge = isOnBridge;
    }
}
