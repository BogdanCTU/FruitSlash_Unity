using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    #region Variables

    // Object RigidBody
    private Rigidbody objectRigidbody;

    // Horizontal Forces
    protected static float horizontalForce, horizontalForceMin = 2, horizontalForceMax = 5, verticalForceMin = 10, verticalForceMax = 15;

    #endregion

    #region Methods

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()   // FixedUpdate is called once every fixed interval
                         // (setted on project settings)
    {
        DeactivateFood();
    }

    private void OnMouseDown()
    {
        this.gameObject.SetActive(false);
        GetGameMode();
    }

    private void GetGameMode()
    {
        int gameMode = Gameplay_Controller.SharedInstance.GetGameMode();
        if (gameMode == 0)   // Easy
        {
            Gameplay_Controller.SharedInstance.SubtractTime(1.0f);
        }
        else if (gameMode == 1)   // Medium
        {
            Gameplay_Controller.SharedInstance.SubtractTime(1.0f);
            Gameplay_Controller.SharedInstance.SubtractLife();
        }
        else if (gameMode == 2)   // Hard
        {
            Gameplay_Controller.SharedInstance.SubtractTime(5.0f);
            Gameplay_Controller.SharedInstance.SubtractLife();
        }
    }

    public void SetPosition()
    {
        Vector3 pos = new Vector3(Random.Range(-2.5f, 2.5f), -6.0f, 0);
        this.gameObject.transform.position = pos;
    }

    public void MoveObject()
    {
        // Resetting (= 0) gameObject velocity, needed when reactivating the game object
        objectRigidbody.velocity = Vector3.zero;

        // Moving the food game object up
        objectRigidbody.AddForce(Vector3.up * Random.Range(verticalForceMin, verticalForceMax), ForceMode.Impulse);   // ForceMode.Impulse is used to apply on instant the movement

        // Moving the food game object left or right
        horizontalForce = Random.Range(horizontalForceMin, horizontalForceMax);
        if (this.gameObject.transform.position.x < 0.0f) objectRigidbody.AddForce(Vector3.right * horizontalForce, ForceMode.Impulse);
        else if (this.gameObject.transform.position.x > 0.0f) objectRigidbody.AddForce(Vector3.left * horizontalForce, ForceMode.Impulse);

        // Rotating game object on random values
        objectRigidbody.AddTorque(Random.Range(-25, 25), Random.Range(-25, 25), Random.Range(-25, 25));
    }

    public void DeactivateFood()
    {
        if (this.gameObject.transform.position.y <= -7f) this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Colliding Left Edge
        if (this.gameObject.transform.position.x < 0.0f) objectRigidbody.AddForce(Vector3.right * horizontalForce, ForceMode.Impulse);
        // Colliding Right Edge
        else if (this.gameObject.transform.position.x > 0.0f) objectRigidbody.AddForce(Vector3.left * horizontalForce, ForceMode.Impulse);
    }

    #endregion
}
   // EOF - End Of File