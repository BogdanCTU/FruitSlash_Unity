using UnityEngine;

public class Fruit : MonoBehaviour
{
    #region Variables

    //[SerializeField] private new string name;
    [SerializeField] private int points, bonusTyme;

    // Object RigidBody
    [SerializeField] private Rigidbody objectRigidbody;

    // Horizontal Forces
    [SerializeField] private float horizontalForce, horizontalForceMin = 2, horizontalForceMax = 5, verticalForceMin = 6, verticalForceMax = 10, rotationA = 20.0f;

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
        DeactivateFruit();
    }

    private void OnEnable()
    {
        SetFruitPosition();
        MoveFruit();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "SwipeTrail":
                {
                    Blade blade = other.GetComponent<Blade>();
                    Gameplay_Controller.SharedInstance.GetGameModeFood(points, bonusTyme);   // Adding lives or time according to game difficulty
                    this.gameObject.SetActive(false);   // Deactivate gameObject
                    Particle_Spawner.SharedInstance.SpawnParticleEffect(this.gameObject);   // Spawn particle Effect
                    UI_WorldCanvas_Spawner.SharedInstance.SpawnCoinObject(this.gameObject.transform.position, this.points);
                    // Setting Combo
                    Gameplay_Controller.SharedInstance.comboCount++;
                    Gameplay_Controller.SharedInstance.comboTime = 0.5f;
                    UI_WorldCanvas_Spawner.SharedInstance.SpawnComboObject(this.gameObject.transform.position, Gameplay_Controller.SharedInstance.comboCount);
                    break;
                }
            case "LeftEdge":
                {
                    objectRigidbody.AddForce(Vector3.right * horizontalForce / 2, ForceMode.Impulse);   
                    break;
                }
            case "RightEdge":
                {
                    objectRigidbody.AddForce(Vector3.left * horizontalForce / 2, ForceMode.Impulse);
                    break;
                }
            case "TopEdge":
                {
                    objectRigidbody.AddForce(Vector3.down * verticalForceMax / 2, ForceMode.Impulse);
                    break;
                }
        }
    }

    #endregion Unity Methods

    #region Object Movement and Position

    public void DeactivateFruit()
    {
        if (this.gameObject.transform.position.y <= -5.6f) this.gameObject.SetActive(false);
    }

    public void SetFruitPosition()
    {
        // Spawning game object at a random position under the visible bottom edge
        Vector3 pos = new Vector3(Random.Range(-2.5f, 2.5f), -5.5f, 0);
        this.gameObject.transform.position = pos;
    }

    public void MoveFruit()
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
        objectRigidbody.AddTorque(Random.Range(-rotationA, rotationA), Random.Range(-rotationA, rotationA), Random.Range(-rotationA, rotationA));
    }

    #endregion Object Movement and Position

    #endregion
}
   // EOF - End Of File