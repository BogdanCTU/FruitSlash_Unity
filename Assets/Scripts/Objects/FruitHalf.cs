using UnityEngine;

public class FruitHalf : MonoBehaviour
{
    #region Variables

    // Object RigidBody
    [SerializeField] private Rigidbody objectRigidbody;

    // Horizontal Forces
    [SerializeField] private float horizontalForce, horizontalForceMin = 2, horizontalForceMax = 5, rotationA = 15.0f;

    // Falling Direction
    public bool fallRight = true;

    #endregion Variables

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()   // FixedUpdate is called once every fixed interval
                                 // (setted on project settings)
    {
        DeactivateHalfFruit();
    }
    
    private void OnEnable()
    {
        MoveHalfFruit();
    }
    
    #endregion Unity Methods

    #region Object Movement and Position

    public void DeactivateHalfFruit()
    {
        if (this.gameObject.transform.position.y <= -7f) this.gameObject.SetActive(false);
    }

    public void MoveHalfFruit()
    {
        // Resetting (= 0) gameObject velocity, needed when reactivating the game object
        objectRigidbody.velocity = Vector3.zero;

        // Moving the food game object left or right based on it's spawn position
        horizontalForce = Random.Range(horizontalForceMin, horizontalForceMax);
        if (fallRight) objectRigidbody.AddForce(Vector3.right * horizontalForce, ForceMode.Impulse);   // Falling right
        else if (!fallRight) objectRigidbody.AddForce(Vector3.left * horizontalForce, ForceMode.Impulse);   // Falling left

        // Rotating game object on random values
        objectRigidbody.AddTorque(Random.Range(-rotationA, rotationA), Random.Range(-rotationA, rotationA), Random.Range(-rotationA, rotationA));
    }

    #endregion Object Movement and Position

    #endregion
}
