using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine.VFX;

public class Player : NetworkBehaviour
{

    private Vector2 movementInput, pointerInput;
    private Rigidbody2D rb;
    private bool FacingRight = true;
    private PlayerInput inputAction;
    private float lastDashTime;

    public Vector2 PointerInput => pointerInput;

    [SerializeField]
    private InputActionReference movement, shoot, pointerPositions;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashCooldown = 1f;

   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            OnWalk();
            if (shoot.action.triggered)
            {
                Dash();
                
            }
        }
        
    }

    private void OnWalk()
    {
        movementInput = movement.action.ReadValue<Vector2>();
        rb.velocity = movementInput * moveSpeed;
    }
    private void Dash()
    {
        if(Time.time < lastDashTime + dashCooldown) return;

        lastDashTime = Time.time;

        Vector2 dashDirection = movementInput;
        
        if(dashDirection == Vector2.zero)
        {
            dashDirection = Vector2.up;
        }

        CmdDash(dashDirection);
    }

    [Command]
    void CmdDash(Vector2 direction)
    {
        rb.AddForce(direction * dashForce, ForceMode2D.Impulse);
        Debug.Log("Dash");
    }
    


    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPositions.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
        
    }

    public void Flip()
    {
        Vector3 tmpScale = transform.localScale;
        tmpScale.x = - tmpScale.x;
        transform.localScale = tmpScale;
        FacingRight = !FacingRight;
    }
}
