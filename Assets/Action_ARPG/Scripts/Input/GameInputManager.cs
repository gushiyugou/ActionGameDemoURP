using System;
using System.Collections;
using System.Collections.Generic;
using GGG.Tool.Singleton;
using UnityEngine;

public class GameInputManager : Singleton<GameInputManager>
{
    private GameInputAction _gameInputAction;

    public Vector2 MovementInput => _gameInputAction.GameInput.Movement.ReadValue<Vector2>();
    public Vector2 CameraLook => _gameInputAction.GameInput.CameraLook.ReadValue<Vector2>();
    public bool Run => _gameInputAction.GameInput.Run.triggered;
    public bool LAttack => _gameInputAction.GameInput.LAttack.triggered;
    public bool RAttack => _gameInputAction.GameInput.RAttack.triggered;
    public bool Climb => _gameInputAction.GameInput.Climb.triggered;
    public bool Grab => _gameInputAction.GameInput.Grab.triggered;
    public bool TakeOut => _gameInputAction.GameInput.TakeOut.triggered;

    protected override void Awake()
    {
        base.Awake();
        _gameInputAction??= new GameInputAction();
    }

    private void OnEnable()
    {
        _gameInputAction.Enable();
    }


    private void OnDisable()
    {
        _gameInputAction.Disable();
    }
}
