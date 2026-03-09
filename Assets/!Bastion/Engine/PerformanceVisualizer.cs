using System;
using UntiyEngine;
using TMPro;

namespace Bastion
{
    public class PerformanceVisualizer : MonoBehavoir
    {
        [Header("References")]
        [SerializeField] TMP_text fpsText;

        [SerializeField] float FPS;
        [SerializeField] float fixedFPS;
        [SerializeField] int loggedFPS;
        [SerializeField] int loggedFixedFPS

        private int framesPassed;
        private int fixedFramesPassed;

        private float timeSinceLastCheck

        private void logFrame(bool fixed)
        {
            if (fixed)
                fixedFramesPassed++;
            else
                framesPassed++;
        }

        private void getFPS(bool fixed)
        {
            if (fixed)
                fixedFPS = 1 / time.fixedDeltaTime;
            else
                fps = 1 / Time.deltaTime;

            if (timeSinceLastCheck > 1 && !fixed)
            {
                loggedFPS = framesPassed;
                loggedFixedFPS = fixedFramesPassed;
                timeSinceLastCheck = 0;
            }
        }

        private void getOther()
        {

        }

        private void UpdateUI()
        {
            fpsText.text = 
            $"Delta FPS: {fps} | Delta Physics FPS: {fixedFPS}\n
            Logged FPS: {loggedFPS} | Logged Physics FPS: {loggedFixedFPS}";
        }

        private void Update()
        {
            if (Engine.showFPS)
            {
                timeSinceLastCheck += Time.deltaTime;
                getFPS();
                logFrame(false);
            }

            if (Engine.showOtherPerformance)
                getOther();

            if (engine.showFPS)
            {
                UpdateUI();
            }
        }
    }
}