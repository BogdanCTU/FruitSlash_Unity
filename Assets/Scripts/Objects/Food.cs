using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private new string name;
    [SerializeField]
    private int points, bonusTyme;

    // Object RigidBody
    private Rigidbody objectRigidbody;

    // Horizontal Forces
    protected static float horizontalForce, horizontalForceMin = 2, horizontalForceMax = 5, verticalForceMin = 6, verticalForceMax = 10;

    // Particle Effect Colour
    [SerializeField] private Material[] materials;
    [SerializeField] private string[] foods;

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

    /*
    private void OnMouseDown()
    {
        GetGameMode();
        this.gameObject.SetActive(false);   // Deactivate gameObject
        SpawnParticleEffect();
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SwipeTrail")
        {
            GetGameMode();
            this.gameObject.SetActive(false);   // Deactivate gameObject
            SpawnParticleEffect();
        }
        else if (other.gameObject.tag == "LeftEdge")   // Colliding Left Edge
        {
            objectRigidbody.AddForce(Vector3.right * horizontalForce / 2, ForceMode.Impulse);
        }
        else if (other.gameObject.tag == "RightEdge")   // Colliding Right Edge
        {
            objectRigidbody.AddForce(Vector3.left * horizontalForce / 2, ForceMode.Impulse);
        }
        else if (other.gameObject.tag == "TopEdge")
        {
            objectRigidbody.AddForce(Vector3.down * verticalForceMax / 2, ForceMode.Impulse);
        }
    }

    #endregion Unity Methods

    private void GetGameMode()
    {
        int gameMode = Gameplay_Controller.SharedInstance.GetGameMode();
        if (gameMode == 0)   // Easy
        {
            Gameplay_Controller.SharedInstance.AddScore(this.points);   // SetNewScore
            Gameplay_Controller.SharedInstance.AddTime(this.bonusTyme);   // SetNewTime
        }
        else if (gameMode == 1)   // Medium
        {
            Gameplay_Controller.SharedInstance.AddScore((this.points * 2));   // SetNewScore
            Gameplay_Controller.SharedInstance.AddTime((this.bonusTyme / 2));   // SetNewTime
        }
        else if (gameMode == 2)   // Hard
        {
            Gameplay_Controller.SharedInstance.AddScore((this.points * 3));   // SetNewScore
            Gameplay_Controller.SharedInstance.AddTime((this.bonusTyme / 4));   // SetNewTime
        }
    }

    #region Movement and Position

    public void DeactivateFood()
    {
        if (this.gameObject.transform.position.y <= -7f) this.gameObject.SetActive(false);
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

    #endregion Movement and Position

    private void SpawnParticleEffect()
    {
        GameObject gameObject = Particle_Spawner.SharedInstance.GetPooledObject();
        if (gameObject != null)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                if (this.gameObject.tag == foods[i])
                {
                    gameObject.GetComponent<Renderer>().material = materials[i];
                }
            }
            gameObject.transform.position = this.gameObject.transform.position;
            gameObject.SetActive(true);
        }
    }

    #endregion
}

public class FoodType
{
    #region Variables

    private string name;
    private int points, bonusTime;

    #endregion
}
   // EOF - End Of File