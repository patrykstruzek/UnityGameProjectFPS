using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dash : MonoBehaviour
{
    [Header("Dash Variables")]
    [SerializeField] public float dashStrenght = 60;
    [SerializeField] Transform orientation;
    Vector3 moveDirection;

    float horizontalMovement;
    float verticalMovement;

    //Rigidbody rb;

    bool isDashing;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PlayerInput();



        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isDashing = true;
        }
    }

    private void FixedUpdate()
    {
        if(isDashing)
        {
            Dashing();
        }
    }

    private void Dashing()
    {
        transform.position = Vector3.Lerp(moveDirection, transform.forward, Time.deltaTime * dashStrenght);

        //rb.AddForce(moveDirection * dashStrenght, ForceMode.Impulse);
        isDashing = false;
    }

    void PlayerInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }
}
