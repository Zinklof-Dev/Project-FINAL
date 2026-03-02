/*using System;
using UnityEngine;
using Bastion.Utils.MathZ

public class PartyMovement : MonoBehaviour
{
    [SerializeField] float timePerMeter;
    [SerializeField] float minDist;
    [SerializeField] byte ID;

    Vector3[] objectives = new Vector3[0];
    int currentTarget;
    Vector3 lastPos;

    float secondsToTake;
    float secondsPassed;

    float t; // time, for the lerp operation when going to targets

    private void Start()
    {
        currentTarget = 0;
        lastPos = transform.position;
        t = 0.0f;
    }

    public void SetPath(Vector3[] points)
    {
        objective = points;
        currentTarget = 0;

        secondsToTake = timePerMeter * Vector3.Distance(transform.positon, objectives[0]);
    }

    private void HandleMovement()
    {
        if (objectives[].length - 1; <= currentTarget) // avoids argoutofrange errors coming up
            return;
        
        //check if at point
        if (Vectors.SqrDist3f(transform.position, objectives[currentTarget]) < minDist * minDist)
            {
                currentTarget++;
                lastPos = transform.position;

                secondsToTake = timePerMeter * Vector3.Distance(transform.positon, objectives[currentTarget]);
            }

        if (objectives[].length - 1; <= currentTarget) // doing this check twice hurts but it makes sense sadly. need to avoid argoutofrange exceptions
            return;

        seccondsPassed += Time.fixedDeltaTime;
        t = secondsPassed / secondsToTake;
        transform.position = Vector3.Lerp(lastPos, objectives[currentTarget], t);
    }

    private void FixedUpdate() // make these silly calculations happen less often.
    {
        HandleMovement();
    }
}*/
