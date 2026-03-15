using System;
using System.Collections.Generic;
using UnityEngine;
using Bastion.Utils.MathZ;

namespace Bastion.SampleScripts
{
    public class Projectile : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] GameObject model;
        [SerializeField] ParticleSystem hitFX;
        [SerializeField] GameObject hitDecalPrefab;
        [Header("Public View")]
        public Vector3 currentVelocity;
        [Header("Physics Settings")]
        [SerializeField] float gravity;
        [SerializeField] float drag;
        [SerializeField] bool pointTowardsVelocity;
        [SerializeField] bool snap;
        [Header("Garbage Collection Settings")]
        [SerializeField] double maxDistance = -1f;
        [SerializeField] float maxTime = -1f;
        [SerializeField] bool toggleDebug;
        [SerializeField] bool cheapDebugPath;

        private float timeTillCleanup = 2f;
        private bool cleanable = false;

        private double distanceTraveled;
        private float timeExisting;

        private List<Vector3> points = new List<Vector3>();

        private void Start()
        {
            if (toggleDebug)
                points.Add(transform.position);

            //if (pointTowardsVelocity)
             //   model.transform.LookAt(nextPoint);
        }

        private void OnDrawGizmos()
        {
            
            if (points.Count <= 1)
                return;

            Gizmos.color = Color.red;

            Vector3 lastPoint = points[0];

            for (int i = 1; i < points.Count - 1; i++)
            {
                Gizmos.DrawLine(lastPoint, points[i]);
            }
        }

        private void LookTowardsVelocity(Vector3 nextPoint)
        {
            if (snap)
                model.transform.LookAt(nextPoint);
            else
                model.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextPoint - transform.position), 0.3f);
        }

        private void CollisionCheck(Vector3 nextPoint)
        {
            RaycastHit hit = new RaycastHit();

            if (Physics.Linecast(transform.position, nextPoint, out hit))
            {
                float dotProduct = Vector3.Dot(hit.normal, transform.forward);

                SurfaceProperties sp = hit.collider.gameObject.GetComponent<SurfaceProperties>();

                float bounceAngle = sp.bounceAngle;
                float velocityMultiplier = sp.velocityMultiplier;

                if (sp == null)
                {
                    bounceAngle =  0f - (15f / 180f);
                    velocityMultiplier = 0.2f;
                }

                if (dotProduct > bounceAngle) // ricochet/bounce case.
                {
                    currentVelocity = 2 * (dotProduct) * hit.normal + currentVelocity;

                    currentVelocity = currentVelocity * velocityMultiplier; // lose velocity when bouncing

                    transform.position = hit.point;
                    return;
                }

                Vector3 exitPoint = new Vector3();

                float thickness = GetThickness(hit.point, out exitPoint);

                GameObject decal = Instantiate(hitDecalPrefab);

                if (thickness != -1f)
                {
                    transform.position = hit.point;
                    hitFX.Play();

                    decal.transform.position = hit.point;
                    decal.transform.rotation = transform.rotation;

                    // PSUEDO

                    // currentVelocity = currentVelocity * ((1 - thickness) * (1 - hardness))

                    // newdir = hitpoint.normal.flip

                    // currentVelocity = currentvelocity * newdir

                    transform.position = exitPoint;
                }

                transform.position = hit.point;
                model.SetActive(false);
                hitFX.Play();
                decal.transform.position = hit.point;
                decal.transform.rotation = transform.rotation;

                cleanable = true;
            }
            else
                transform.position = nextPoint;
        }

        private float GetThickness(Vector3 hitPoint, out Vector3 exitPoint)
        {
            RaycastHit hit = new RaycastHit();

            exitPoint = Vector3.zero;

            if (Physics.Linecast(hitPoint + (currentVelocity.normalized), hitPoint, out hit))
            {
                exitPoint = hit.point;
                return Vectors.SqrDist3f(hitPoint, hit.point);
            }
            else
                return -1f;
        }

        // at immediete glance this looks like an area that should be switch case not four if statements, but they're not exclusive to each other other than the first one so...
        private void CheckForMax()
        {
            if (cleanable)
                return;

            // distance traveled is calcualted in square space saving a miniscule ammount of time, but could really add up over the long term.
            if (distanceTraveled > maxDistance * maxDistance && maxDistance != -1f)
                Destroy(this);

            if (timeExisting > maxTime && maxTime != -1f)
                Destroy(this);

            if (maxTime != -1f)
                timeExisting += Time.deltaTime;
        }

        private void CheckForCleanup()
        {
            if (timeTillCleanup < 0)
                Destroy(this);

            if (cleanable)
                timeTillCleanup -= Time.deltaTime;

        }

        private void HandleMovement()
        {
            if (cleanable)
                return;

            Vector3 nextPoint = transform.position + ((currentVelocity - (gravity * Vector3.up)) * Time.deltaTime);

            if (maxDistance != -1f)
                distanceTraveled += Vectors.SqrDist3f(transform.position, nextPoint);

            if (pointTowardsVelocity)
                LookTowardsVelocity(nextPoint);

            CollisionCheck(nextPoint);

            currentVelocity -= (currentVelocity * drag) * Time.deltaTime;
        }

        private void Debug()
        {
            if (!toggleDebug)
                return;

            if (!cheapDebugPath)
                points.Add(transform.position);
            else if (cheapDebugPath && (Time.frameCount % 10) == 0)
                points.Add(transform.position);
        }

        private void Update()
        {
            HandleMovement();
            CheckForMax();
            CheckForCleanup();
            Debug();
        }
    }
}