using System;
using UnityEngine;

namespace Bastion.SampleScripts
{
    public enum Surfaces
    {
        Custom = 0,
        ConsumeAll = 1,
        Water = 2,
        Dirt = 3,
        Sand = 4,
        SoftMetal = 5,
        HardMetal = 6,
        Plastic = 7,
        Paper = 8,
    }

    public class SurfaceProperties : MonoBehaviour
    {
        [SerializeField] Surfaces surfaceType = Surfaces.Custom;

        public float velocityMultiplier = 0.2f;
        public float bounceAngle = 15;

        private void Start()
        {
            SetValues();
            CalcDot();
        }

        private void SetValues()
        {
            switch (surfaceType)
            {
                case Surfaces.Custom:
                    return;
                case Surfaces.Water:
                    velocityMultiplier = 0.13f;
                    bounceAngle = 20;
                    return;
                case Surfaces.Dirt:
                    velocityMultiplier = 0.2f;
                    bounceAngle = 8.5f;
                    return;
                case Surfaces.Sand:
                    velocityMultiplier = 0.1f;
                    bounceAngle = 5f;
                    return;
                case Surfaces.SoftMetal:
                    velocityMultiplier = 0.28f;
                    bounceAngle = 20;
                    return;
                case Surfaces.HardMetal:
                    velocityMultiplier = 0.35f;
                    bounceAngle = 25;
                    return;
                case Surfaces.Plastic:
                    velocityMultiplier = 0.2f;
                    bounceAngle = 12;
                    return;
                case Surfaces.Paper:
                    velocityMultiplier = 0.13f;
                    bounceAngle = 5;
                    return;
                default:
                    velocityMultiplier = 0.2f;
                    bounceAngle = 15;
                    return;
            }
        }

        private void CalcDot()
        {
            bounceAngle = 0f - (bounceAngle / 180f);
        }
    }
}