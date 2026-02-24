using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine.UI;

namespace BOTD.Dialouge
{
    class DialougeManager : MonoBehaviour
    {
        [Header("Dialouge")]
        [SerializeField] Speaker[] speakers;
        [SerializeField] Line[] lines;
        [Header("Settings")]
        [SerializeField] float timePerChar = 0.025f;
        [SerializeField] float dialougeTimeScale = 0f;
        [SerializeField] float lerp = 0.1f;
        [SerializeField] float rotateDegrees;
        [SerializeField] bool doDOF;
        [SerializeField] bool LookatSpeaker;
        [Header("References")]
        [SerializeField] Volume volume;
        [SerializeField] TMP_Text dialougeBox;
        [SerializeField] TMP_Text speakerName;
        [SerializeField] Image speakerImage;

        // Status
        bool open, awaiting, done = false;
        int currentLine = -1; // -1 = no dialouge right now;
        // Memories
        float returnTimeScale;
        // References
        DepthOfField dof;
        RectTransform rt;
        Transform lookAt;
        Transform cameraTransform;
        // Saved Math Results
        float halfres;

        private void Start()
        {
            // save result of common math
            halfres = Screen.currentResolution.height / 2;

            // fetch references
            if (!volume.profile.TryGet<DepthOfField>(out dof))
            {
                Debug.LogWarning("Volume does not have a depth of field!");
            }

            rt = gameObject.GetComponent<RectTransform>();
            cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

            rt.localPosition = new Vector3 (0, -halfres - 700, 0);
        }
        
        public void StartDialouge()
        {
            returnTimeScale = Time.timeScale;
            Time.timeScale = dialougeTimeScale;

            if (doDOF)
                dof.active = true;

            open = true;
            awaiting = true;
        }

        public void NextLine()
        {
            if (awaiting || !open)
            {
                return;
            }
        
            StopAllCoroutines();

            if (++currentLine > lines.Length - 1)
            {
                done = true;
                return;
            }

            Debug.Log(currentLine);

            Speaker s = speakers[lines[currentLine].speaker];

            if (LookatSpeaker)
                lookAt = s.lookAtPoint;

            speakerImage.sprite = s.expressions[lines[currentLine].expression];

            speakerName.text = s.name;
            dialougeBox.text = "";

            StartCoroutine(Typewriter());
        }

        private void CleanupDialouge()
        {
            Time.timeScale = returnTimeScale;

            if (doDOF)
                dof.active = false;

            Destroy(this.gameObject);
        }

        private void DoLerp()
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                NextLine();
            }
            if (Input.GetKeyUp(KeyCode.O))
            {
                StartDialouge();
            }

            if (done)
            {
                rt.localPosition = new Vector3(0, Mathf.Lerp(rt.localPosition.y, -halfres - 735, lerp), 0);
                dof.active = false;
            }
            else if (open)

            {
                rt.localPosition = new Vector3(0, Mathf.Lerp(rt.localPosition.y, -halfres, lerp), 0);
            }
            else
            {
                rt.localPosition = new Vector3(0, Mathf.Lerp(rt.localPosition.y, -halfres - 700, lerp), 0);
            }

            if (awaiting)
            {
                if (rt.localPosition.y < halfres -540.1f)
                {
                    awaiting = false;
                    NextLine();
                }
            }
            else if (done)
            {
                if (rt.localPosition.y < -halfres - 735f + 2.5f)
                { 
                    CleanupDialouge();
                }
            }

            if (LookatSpeaker && open)
                cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, Quaternion.LookRotation(lookAt.position - cameraTransform.position, cameraTransform.up), rotateDegrees);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                NextLine();
            }
            if (Input.GetKeyUp(KeyCode.O))
            {
                StartDialouge();
            }

            DoLerp();
        }

        IEnumerator Typewriter()
        {
            foreach (char c in lines[currentLine].text)
            {
                dialougeBox.text += c;
                yield return new WaitForSecondsRealtime(timePerChar);
            }
        }
    }
}