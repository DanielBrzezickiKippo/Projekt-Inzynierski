using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuColors : MonoBehaviour
{
    float timeLeft;
    Color targetColor;
    [SerializeField] private Camera mainCamera;

    void Update()
    {
        if (timeLeft <= Time.deltaTime)
        {
            // transition complete
            // assign the target color
            mainCamera.backgroundColor = targetColor;

            // start a new transition
            targetColor = new Color(Random.value, Random.value, Random.value);
            timeLeft = 5.0f;
        }
        else
        {
            // transition in progress
            // calculate interpolated color
            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, targetColor, Time.deltaTime / timeLeft);

            // update the timer
            timeLeft -= Time.deltaTime;
        }
    }
}
