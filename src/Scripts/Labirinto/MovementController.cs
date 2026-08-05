using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneController
{
    public static void StartPosition(Transform targetTransform)
    {
        targetTransform.position = new Vector3(905, 99, -12); // Altere para a posição desejada
        targetTransform.rotation = Quaternion.Euler(0, 0, 0); // Altere para a rotação desejada
    }
}

public class MovementController : MonoBehaviour
{
    //Define the speed at which the object moves.
    public float moveSpeed = 5000;
    private Rigidbody2D rb;

    private float angle;

    //public GameObject success_panel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //SceneController.StartPosition(transform);
        Debug.Log(transform.position);
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        //Get the value of the Horizontal input axis.

        float verticalInput = Input.GetAxis("Vertical");
        //Get the value of the Vertical input axis.

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0).normalized;

        rb.velocity =  movement * moveSpeed * Time.fixedDeltaTime;

        if (movement.x != 0 || movement.y != 0)
        {
            angle = Mathf.Atan2(-movement.x, movement.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collider){
        Debug.Log("Trigger");
        success_panel.SetActive(true);
    }*/
}
