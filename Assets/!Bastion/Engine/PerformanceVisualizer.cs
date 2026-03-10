using System;
using System.Diagnostics;
using UnityEngine;
using TMPro;

namespace Bastion
{
    public class PerformanceVisualizer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] TMP_Text fpsText;
        [Header("States")]
        public bool on;
        [Header("Debug View")]
        [SerializeField] float fps;
        [SerializeField] float fixedFPS;
        [SerializeField] int loggedFPS;
        [SerializeField] int loggedFixedFPS;
        [SerializeField] float totalMemoryMB;
        [SerializeField] float totalMemoryGB;
        [SerializeField] float timeSinceLastCheck;

        private int framesPassed;
        private int fixedFramesPassed;

        private Vector3 startPos;

        private void Start()
        {
            if (Engine.showFPS)
                enabled = true;
            else
                Destroy(this);
            
            fpsText = gameObject.GetComponent<TMP_Text>();
        }

        private void LogFrame(bool fixedFrame)
        {
            if (fixedFrame)
                fixedFramesPassed++;
            else
                framesPassed++;
        }

        private void GetFPS(bool fixedFrame)
        {
            if (fixedFrame)
                fixedFPS = 1 / Time.fixedDeltaTime;
            else
                fps = 1 / Time.deltaTime;

            if (timeSinceLastCheck > 1 && !fixedFrame)
            {
                loggedFPS = framesPassed;
                loggedFixedFPS = fixedFramesPassed;
                timeSinceLastCheck = 0;
            }
        }

        private void GetOther()
        {
            Process.GetCurrentProcess().Refresh();
            long memoryInBytes = Process.GetCurrentProcess().WorkingSet64;

            totalMemoryMB = memoryInBytes / (1024f * 1024f);
            totalMemoryGB = totalMemoryMB / (1024f);
        }

        private void UpdateUI()
        {
            fpsText.text = 
            $"Delta FPS: {fps} | Delta Physics FPS: {fixedFPS}\n Logged FPS: {loggedFPS} | Logged Physics FPS: {loggedFixedFPS}";

            if (Engine.showOtherPerformance)
            {
                fpsText.text += $"\n Ram Usage: {totalMemoryGB} GB ({totalMemoryMB} MB)";
            }
        }

        private void CheckInput()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Alpha0))
            {
                on = !on;

                if (on) // little nesty for update but... actually no screw it new function time
                {
                    transform.position = startPos;
                }
            }
        }

        private void GetDiagnostics()
        {
            if (!on)
            {
                fpsText.text = "";
                fpsText.transform.position = new Vector3(9999,9999,9999);
                return;
            }

            if (Engine.showFPS)
            {
                timeSinceLastCheck += Time.deltaTime;
                GetFPS(false);
                LogFrame(false);
            }

            if (Engine.showOtherPerformance)
                GetOther();

            if (Engine.showFPS)
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