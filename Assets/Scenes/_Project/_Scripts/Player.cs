using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player
{
    public event Action<IClick> OnFirstClick; 
    
    private readonly MouseInput _mouseInput;
    private readonly Camera _camera;
    private IClick _click;
    private bool _isFirstClick;
    
    public Player(MouseInput mouseInput,Camera camera)
    {
        _mouseInput = mouseInput;
        _camera = camera;
        _mouseInput.Mouse.Position.performed += MouseChangePosition;
        _mouseInput.Mouse.LeftButton.started += LeftButtonDown;
        _mouseInput.Mouse.LeftButton.canceled += LeftButtonUp;
        _mouseInput.Mouse.RightButton.started += RightButtonDown;
    }

    private void FirstClickOnCell()
    {
        if (_isFirstClick)
        {
            return;
        }

        if (_click != null)
        {
            OnFirstClick?.Invoke(_click);
            _isFirstClick = true;
        }
    }
    
    private void RightButtonDown(InputAction.CallbackContext callback)
    {
        _click?.RightButtonDown();
    }

    private void LeftButtonUp(InputAction.CallbackContext callback)
    {
        FirstClickOnCell();
        _click?.LeftButtonUp();
    }

    private void LeftButtonDown(InputAction.CallbackContext callback)
    {
        FirstClickOnCell();
        _click?.LeftButtonDown();
    }

    private void MouseChangePosition(InputAction.CallbackContext callback)
    {
        if (TryGetComponentInMouseRay(out IClick click,callback.ReadValue<Vector2>()))
        {
            if (_click == click)
            {
                return;
            }
            
            _click?.Deselect();
            _click = click;
            UpdateNewClick(_click);
            _click.Select();
        }
        else
        {
            _click?.Deselect();
            _click = null;
        }
    }

    private void UpdateNewClick(IClick click)
    {
        if (_mouseInput.Mouse.LeftButton.IsPressed())
        {
            click.LeftButtonDown();
        }
    }
    
    private bool TryGetComponentInMouseRay<T>(out T component, Vector2 mousePosition)
    {
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit2D RaycastHit2D = Physics2D.GetRayIntersection(ray);
        if (RaycastHit2D == false)
        {
            component = default;
            return false;
        }

        return RaycastHit2D.collider.TryGetComponent(out component);
    }
}