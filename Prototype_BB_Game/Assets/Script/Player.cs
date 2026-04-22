using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    private Vector2 movementInput, pointerInput;
    private Rigidbody2D rb;
    private bool FacingRight = true;

    public Vector2 PointerInput => pointerInput;

    [SerializeField]
    private InputActionReference movement, shoot, pointerPositions;
    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        pointerInput = GetPointerInput();

        movementInput = movement.action.ReadValue<Vector2>();

        rb.velocity = movementInput * moveSpeed;

        //Flip Character
        
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPositions.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
        if (mousePos.x < transform.position.x && FacingRight)
        {
            Flip();
        }
        else if(mousePos.x > transform.position.x && FacingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        Vector3 tmpScale = transform.localScale;
        tmpScale.x = - tmpScale.x;
        transform.localScale = tmpScale;
        FacingRight = !FacingRight;
    }
}
