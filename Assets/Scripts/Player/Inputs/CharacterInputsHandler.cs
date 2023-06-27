using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputsHandler : MonoBehaviour
{
    float _movementInput;
    bool _isJumpPressed;
    bool _iscanHook;
    bool _isShield;


    NetworkInputData _inputData;
    private void Awake()
    {
        _inputData = new NetworkInputData();
    }
    void Update()
    {
        
        _movementInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _iscanHook = true;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _isShield = true;
        }
    }

    public NetworkInputData GetInputs()
    {
        _inputData.movementInput = _movementInput;
        _inputData.isJumpPressed = _isJumpPressed;
        _inputData.isCanHook = _iscanHook;
        _inputData.isShield = _isShield;
        _isJumpPressed = false;
        _iscanHook = false;
        _isShield = false;
        return _inputData;
    }
}
