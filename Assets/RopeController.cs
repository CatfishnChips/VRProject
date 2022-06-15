using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RopeController : MonoBehaviour
{
    private XRIDefaultInputActions _inputActions;

    private Vector3 _startPosL, _startPosR;
    private Vector3 _currentPosL, _currentPosR;
    private Transform _controllerL, _controllerR, _camera;

    [SerializeField] private float _maxDistance;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _cameraOffsetY;

    private Vector3 _headPosition;

    private bool _isMoving;

    private bool _isHoldingL, _isHoldingR;

    private bool _isHolding {
        get {
            if (_isHoldingL || _isHoldingR) return true;
            else return false;
        }
    }

    private float _moveAmount;

    [SerializeField] private float _sensitivity;

    private void Awake() {
        _inputActions = new XRIDefaultInputActions();
        _controllerL = GameObject.Find("LeftHand Controller").transform;
        _controllerR = GameObject.Find("RightHand Controller").transform;
        _camera = Camera.main.transform;
    }

    private void OnEnable() {
        _inputActions.Enable();

        _inputActions.XRILeftHandInteraction.Select.started += ctx => OnLeftHandDown();
        _inputActions.XRILeftHandInteraction.Select.performed += ctx => OnLeftHandDown();
        _inputActions.XRILeftHandInteraction.Select.canceled += ctx => OnLeftHandDown();

        _inputActions.XRIRightHandInteraction.Select.started += ctx => OnRightHandDown();
        _inputActions.XRIRightHandInteraction.Select.performed += ctx => OnRightHandDown();
        _inputActions.XRIRightHandInteraction.Select.canceled += ctx => OnRightHandDown();
    }

    private void OnDisable() {
        _inputActions.XRILeftHandInteraction.Select.started -= ctx => OnLeftHandDown();
        _inputActions.XRILeftHandInteraction.Select.performed -= ctx => OnLeftHandDown();
        _inputActions.XRILeftHandInteraction.Select.canceled -= ctx => OnLeftHandDown();

        _inputActions.XRIRightHandInteraction.Select.started -= ctx => OnRightHandDown();
        _inputActions.XRIRightHandInteraction.Select.performed -= ctx => OnRightHandDown();
        _inputActions.XRIRightHandInteraction.Select.canceled -= ctx => OnRightHandDown();

        _inputActions.Disable();
    }

    private void OnLeftHandDown() {
        if (!CheckIfInRangeL()) return;
        _isHoldingL = true;
        _startPosL = _controllerL.position;
    }

    private void OnLeftHandHeld() {

    }

    private void OnLeftHandUp() {
        _isHoldingL = false;
    }

    private void OnRightHandDown() {
        if (!CheckIfInRangeR()) return;
        _isHoldingR = true;
        _startPosR = _controllerR.position;
        
    }

    private void OnRightHandHeld() {

    }

    private void OnRightHandUp() {
        _isHoldingR = false;
    }

    private bool CheckIfInRangeL() {
        if (Vector3.Distance(transform.position, _controllerL.position) <= _maxDistance) {
            return true;
        }
        else 
        { 
            return false;
        }
    }

    private bool CheckIfInRangeR() {       
        if (Vector3.Distance(transform.position, _controllerR.position) <= _maxDistance) {
            return true;
        }
        else 
        { 
            return false;
        }
    }

    private void CalculateMovementL() {
        //float curveTime = (_startPosL.y - _currentPosL.y) / (_startPosL.y - _headPosition.y);
        //_moveAmount = (_startPosL.y - _currentPosL.y) * _sensitivity * Time.deltaTime;
        if (!_isHoldingL) return;
        if (_isHoldingR) return;
        float distance = (_startPosL.y - _currentPosL.y);
        if (distance > 0) {
            _moveAmount = (_startPosL.y - _currentPosL.y);
        }
        else {
            _moveAmount = (_startPosL.y - _currentPosL.y);
        }
    }

    private void CalculateMovementR() {
        if (!_isHoldingR) return;
        if (_isHoldingL) return;
        float distance = (_startPosR.y - _currentPosR.y);
        if (distance > 0) {
            _moveAmount = (_startPosR.y - _currentPosR.y);
        }
        else {
            _moveAmount = (_startPosR.y - _currentPosR.y);
        }
    }

    private void MoveScene() {
        //sceneStatus = sceneStatus + _moveAmount;
    }

    private void HandleGravity() {
        if (_isHolding) return;
        _moveAmount = Physics.gravity.y * Time.deltaTime;
    }

    private void FixedUpdate() {
        CalculateMovementL();
        CalculateMovementR();
        HandleGravity();
        MoveScene();
    }
}
