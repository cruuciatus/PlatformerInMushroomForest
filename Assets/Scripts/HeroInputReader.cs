using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;
    private HeroInputAction _inputActions;


    private void Awake()
    {
        _inputActions = new HeroInputAction();
        _inputActions.Hero.Movement.performed += OnHorizontalMovement;
        _inputActions.Hero.Movement.canceled += OnHorizontalMovement;

        _inputActions.Hero.Interact.performed += OnInteract;
        _inputActions.Hero.Interact.canceled += OnInteract;

        _inputActions.Hero.Attack.performed += OnAttack;
        _inputActions.Hero.Attack.canceled += OnAttack;

        _inputActions.Hero.Throw.performed += OnUseInventory;
        _inputActions.Hero.Throw.canceled += OnUseInventory;

        _inputActions.Hero.NextItem.performed += OnNextItem;
        _inputActions.Hero.NextItem.canceled += OnNextItem;


        _inputActions.Hero.Shield.performed += OnUsePerk;
        _inputActions.Hero.Shield.canceled += OnUsePerk;

        _inputActions.Hero.SuperThrow.performed += OnDoSuperThrow;
        _inputActions.Hero.SuperThrow.canceled += OnDoSuperThrow;

        _inputActions.Hero.Dash.performed += OnDash;
       _inputActions.Hero.Dash.canceled += OnDash;


    }



    private void OnEnable()
    {
        _inputActions.Enable();
    }



    public void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _hero.SetDirection(direction);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _hero.SetDirection(direction);
    }



    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.Interact();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.Attack();
        }
    }
    public void OnDoSuperThrow(InputAction.CallbackContext context)
    {
       
        if (context.started)
        {
          //  _hero.Invoke(nameof(_hero.OnDoThrow), 0.1f);
          //  _hero.Invoke(nameof(_hero.OnDoThrow), 0.4f);
           // _hero.Invoke(nameof(_hero.OnDoThrow), 0.8f);
            //  _hero.OnDoThrow();
        }
    }

     public void OnUseInventory(InputAction.CallbackContext context)
    {
     if (context.performed)
     {
         _hero.StartTimerSuperThrow();
     }

     if (context.canceled)
      {
          _hero.UseInventory();
     }
    }
    //public void OnUseInventory(InputAction.CallbackContext context)
    // {
     // if (context.canceled)
      //{
     //  _hero.UseInventory();
    // }
    // }



    public void OnNextItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _hero.NextItem();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _hero.Dash();
        }
    }
    public void OnUsePerk(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _hero.UsePerk();
        }
    }
    public void OnDestroy()
    {

        _inputActions.Hero.Movement.performed -= OnHorizontalMovement;
        _inputActions.Hero.Movement.canceled -= OnHorizontalMovement;

        _inputActions.Hero.Interact.performed -= OnInteract;
        _inputActions.Hero.Interact.canceled -= OnInteract;

        _inputActions.Hero.Attack.performed -= OnAttack;
        _inputActions.Hero.Attack.canceled -= OnAttack;

        _inputActions.Hero.Throw.performed -= OnUseInventory;
        _inputActions.Hero.Throw.canceled -= OnUseInventory;

        _inputActions.Hero.NextItem.performed -= OnNextItem;
        _inputActions.Hero.NextItem.canceled -= OnNextItem;


        _inputActions.Hero.Shield.performed -= OnUsePerk;
        _inputActions.Hero.Shield.canceled -= OnUsePerk;

        _inputActions.Hero.SuperThrow.performed -= OnDoSuperThrow;
        _inputActions.Hero.SuperThrow.canceled -= OnDoSuperThrow;

        _inputActions.Hero.Dash.performed -= OnDash;
        _inputActions.Hero.Dash.canceled -= OnDash;

    }
}
