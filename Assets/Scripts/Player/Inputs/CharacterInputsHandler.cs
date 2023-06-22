using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputsHandler : MonoBehaviour
{
    float _movementInput;
    bool _isJumPressed;

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
            _isJumPressed = true;
        }
    }

    public NetworkInputData GetInputs()
    {
        _inputData.movementInput = _movementInput;
        _inputData.isJumpPressed = _isJumPressed;
        _isJumPressed = false;

        return _inputData;
    }
}
