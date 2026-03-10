using System;
using System.Diagnostiscs;
using UntiyEngine;
using TMPro;

namespace Bastion
{
    public class PerformanceVisualizer : MonoBehavoir
    {
        [Header("References")]
        [SerializeField] TMP_text fpsText;
        [Header("States")]
        public bool enabled;
        [Header("Debug View")]
        [SerializeField] float FPS;
        [SerializeField] float fixedFPS;
        [SerializeField] int loggedFPS;
        [SerializeField] int loggedFixedFPS
        [SerializeField] float totalMemoryMB;
        [SerializeField] float totalMemoryGB;
        [SerializeField] float timeSinceLastCheck

        private int framesPassed;
        private int fixedFramesPassed;

        private Vector3 startPos;

        private void Start()
        {
            if (Engine.showFPS)
                enabled = true;
            else
                Destroy(This);
            
            fpsText = gameObject.GetComponent<TMP_text>();

            
        }

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
            Process.GetCurrentProcess().Refresh();
            long memoryInBytes = Process.GetCurrentProcess().WorkingSet64;

            totalMemoryMB = memoryInBytes / (1024f * 1024f);
            totalMemoryGB = totalMemoryMB / (1024f);
        }

        private void UpdateUI()
        {
            fpsText.text = 
            $"Delta FPS: {fps} | Delta Physics FPS: {fixedFPS}\n
            Logged FPS: {loggedFPS} | Logged Physics FPS: {loggedFixedFPS}";

            if (Engine.showOtherPerformance)
            {
                fpsText.text += $"\n Ram Usage: {totalMemoryGB} GB ({totalMemoryMB} MB)";
            }
        }

        private void CheckInput()
        {
            if (Input.GetKey(KeyCode.LShift) && Inpt.GetKey(KeyCode.Zero))
            {
                enabled = !enabled;

                if (enabled) // little nesty for update but... actually no screw it new function time
                {
                    transform.position = startPos;
                }
            }
        }

        private void GetDiagnostics()
        {
            if (!enabled)
            {
                fpsText.text = "";
                fpsText.transform.position = new Vector3(9999,9999,9999);
                return;
            }

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

        private void Update()
        {
            CheckInput();
            GetDiagnostics();
        }
    }
}