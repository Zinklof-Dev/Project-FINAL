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
        [SerializeField] string textValue;
        
        float fps;
        float fixedFPS;
        int loggedFPS;
        int loggedFixedFPS;
        float timeSinceLastCheck;

        public int framesPassed;
        public int fixedFramesPassed;

        private Vector3 startPos;

        private void Start()
        {
            if (Engine.showFPS)
                enabled = true;
            else
                Destroy(this);
            
            fpsText = gameObject.GetComponent<TMP_Text>();

            on = true;
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
                framesPassed = 0;
                fixedFramesPassed = 0;
            }
        }

        private void UpdateUI()
        {
            fpsText.text = 
            $"Delta FPS: {fps} | Delta Physics FPS: {fixedFPS}\nLogged FPS: {loggedFPS} | Logged Physics FPS: {loggedFixedFPS}";
        }

        private void CheckInput()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha0))
            {
                on = !on;

                if (on) // little nesty for update but... actually no screw it new function time
                    transform.position = startPos;
                else
                {
                    fpsText.text = "";
                    fpsText.transform.position = new Vector3(9999,9999,9999);
                }
            }
        }

        private void GetDiagnostics(bool fixedFrame)
        {
            if (!on)
                return;

            if (Engine.showFPS)
            {
                if (!fixedFrame)
                    timeSinceLastCheck += Time.deltaTime;
                GetFPS(fixedFrame);
            }

            if (Engine.showFPS)
            {
                UpdateUI();
            }
        }

        private void Update()
        {
            framesPassed++;

            CheckInput();
            GetDiagnostics(false);
        }

        private void FixedUpdate()
        {
            fixedFramesPassed++;

            GetDiagnostics(true);
            textValue = fpsText.text;
        }
    }
}