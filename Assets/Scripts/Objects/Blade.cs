using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]
public class Blade : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private Camera myCamera;
    private Vector3 mousePosition;
    private TrailRenderer bladeTrail;
    private BoxCollider bladeCollider;
    private bool swiping = false;
    [SerializeField] private float minSlideVelocity = 0.001f;

    // Swipe Direction
    public Vector3 swipeDirection { get; private set; }

    #endregion

    #region Methods

    #region Unity Methods
    void Awake()
    {
        bladeTrail = GetComponent<TrailRenderer>();
        bladeCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        StopSwiping();
    }

    private void OnDisable()
    {
        StopSwiping();
    }

    private void Update()
    {
        if (Gameplay_Controller.SharedInstance.gamePaused == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartSwiping();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                StopSwiping();
            }
            if (swiping)
            {
                ContinueSwiping();
            }
        }
    }

    #endregion Unity Methods

    private void StartSwiping()
    {
        mousePosition = myCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        transform.position = mousePosition;

        swiping = true;
        bladeCollider.enabled = true;
        bladeTrail.enabled = true;
        bladeTrail.Clear();
    }

    private void StopSwiping()
    {
        swiping = false;
        bladeCollider.enabled = false;
        bladeTrail.enabled = false;
    }

    void ContinueSwiping()
    {
        mousePosition = myCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        swipeDirection = mousePosition - this.transform.position;
        float velocity = swipeDirection.magnitude / Time.deltaTime;
        bladeCollider.enabled = velocity > minSlideVelocity ? true : false;
        transform.position = mousePosition;
    }

    #endregion
}
// EOF - End Of File