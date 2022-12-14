using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables

    // Object RigidBody
    private Rigidbody objectRigidbody;

    // Horizontal Forces
    [SerializeField] private float horizontalForce, horizontalForceMin = 2, horizontalForceMax = 5, verticalForceMin = 10, verticalForceMax = 15;

    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()   // FixedUpdate is called once every fixed interval
                         // (setted on project settings)
    {
        DeactivateFood();
    }

    private void OnEnable()
    {
        SetPosition();
        MoveObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "SwipeTrail":
                {
                    Gameplay_Controller.SharedInstance.GetGameModeEnemy();   // Subtracting lives or time according to game difficulty
                    this.gameObject.SetActive(false);   // Deactivate gameObject
                    Particle_Spawner.SharedInstance.SpawnParticleEffect(this.gameObject);
                    UI_WorldCanvas_Spawner.SharedInstance.SpawnLifeObject(gameObject.transform.position);
                    break;
                }
            case "LeftEdge":
                {
                    objectRigidbody.AddForce(Vector3.right * horizontalForce / 3, ForceMode.Impulse);
                    break;
                }
            case "RightEdge":
                {
                    objectRigidbody.AddForce(Vector3.left * horizontalForce / 3, ForceMode.Impulse);
                    break;
                }
            case "TopEdge":
                {
                    objectRigidbody.AddForce(Vector3.down * verticalForceMax / 3, ForceMode.Impulse);
                    break;
                }
        }
    }

    #endregion Unity Methods

    #region Object Movement and Position

    public void SetPosition()
    {
        // Spawning game object at a random position under the visible bottom edge
        Vector3 pos = new Vector3(Random.Range(-2.5f, 2.5f), -5.5f, 0);
        this.gameObject.transform.position = pos;
    }

    public void MoveObject()
    {
        // Resetting (= 0) gameObject velocity, needed when reactivating the game object
        objectRigidbody.velocity = Vector3.zero;

        // Moving the food game object up
        objectRigidbody.AddForce(Vector3.up * Random.Range(verticalForceMin, verticalForceMax), ForceMode.Impulse);   // ForceMode.Impulse is used to apply on instant the movement

        // Moving the food game object left or right based on it's spawn position
        horizontalForce = Random.Range(horizontalForceMin, horizontalForceMax);
        if (this.gameObject.transform.position.x < 0.0f) objectRigidbody.AddForce(Vector3.right * horizontalForce, ForceMode.Impulse);
        else if (this.gameObject.transform.position.x > 0.0f) objectRigidbody.AddForce(Vector3.left * horizontalForce, ForceMode.Impulse);

        // Rotating game object on random values
        objectRigidbody.AddTorque(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
    }

    public void DeactivateFood()
    {
        if (this.gameObject.transform.position.y <= -5.6f) this.gameObject.SetActive(false);
    }

    #endregion Object Movement and Position

    #endregion Methods
}
   // EOF - End Of File