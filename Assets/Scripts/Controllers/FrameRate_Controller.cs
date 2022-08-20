using UnityEngine;

public class FrameRate_Controller : MonoBehaviour
{
    void Start()
    {
        // Make the game run at 60 fps
        Application.targetFrameRate = 60;
    }
}
