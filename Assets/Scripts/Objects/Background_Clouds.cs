using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Clouds : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject cloud;
    private float speed = 5.0f;
    [SerializeField] private float speedMin = 2.0f, speedMax = 5.0f;
    [SerializeField] private float yMin = 0.0f, yMax = 9.0f;
    private bool moveRight = true;

    #endregion

    #region Methods

    #region UnityMethods
    private void Awake()
    {
        InitialiseCloud();
        SetCloudPosition();
    }

    private void FixedUpdate()
    {
        AnimateCloud();
    }
    #endregion UnityMethods

    private void InitialiseCloud()
    {
        int move = Random.Range(0, 10);
        if (move < 5) moveRight = false;
        else if (move >= 5) moveRight = true;
        speed = Random.Range(speedMin, speedMax);
    }

    private void SetCloudPosition()
    {
        if (moveRight) cloud.transform.localPosition = new Vector3(-11.0f, Random.Range(yMin, yMax), 0);
        if (moveRight == false) cloud.transform.localPosition = new Vector3(11.0f, Random.Range(yMin, yMax), 0);
    }

    private void AnimateCloud()
    {
        if (moveRight)
        {
            cloud.transform.localPosition += Vector3.right * Time.deltaTime * speed;
            if (cloud.transform.localPosition.x > 11.0f)
            {
                float tempY = cloud.transform.localPosition.y;
                cloud.transform.localPosition = new Vector3(-11.0f, tempY, 0);
            }
        }
        else if(moveRight == false)
        {
            cloud.transform.localPosition -= Vector3.right * Time.deltaTime * speed;
            if (cloud.transform.localPosition.x < -11.0f)
            {
                float tempY = cloud.transform.localPosition.y;
                cloud.transform.localPosition = new Vector3(11.0f, tempY, 0);
            }
        }
    }

    #endregion
}
