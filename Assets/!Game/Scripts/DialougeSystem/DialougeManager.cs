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
        [Header("References")]
        [SerializeField] Volume volume;
        [SerializeField] TMP_Text dialougeBox;
        [SerializeField] TMP_Text speakerName;
        [SerializeField] Image speakerImage;
        [Header("Debug")]
        [SerializeField] Vector3 pos;
    
        int currentLine = -1; // -1 = no dialouge right now;

        float returnTimeScale;

        DepthOfField dof;

        RectTransform rt;
        public bool open, awaiting, done = false;

        float halfres;

        private void Start()
        {
            halfres = Screen.currentResolution.height / 2;

            if (!volume.profile.TryGet<DepthOfField>(out dof))
            {
                Debug.LogWarning("Volume does not have a depth of field!");
            }

            //StartDialouge();

            rt = gameObject.GetComponent<RectTransform>();

            rt.localPosition = new Vector3 (0, -halfres - 700, 0);
        }
        
        public void StartDialouge()
        {
            returnTimeScale = Time.timeScale;
            Time.timeScale = dialougeTimeScale;

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

            Speaker s = speakers[lines[currentLine].speaker];

            speakerImage.sprite = s.expressions[lines[currentLine].expression];

            speakerName.text = s.name;
            dialougeBox.text = "";

            StartCoroutine(Typewriter());
        }

        private void CleanupDialouge()
        {
            Time.timeScale = returnTimeScale;

            dof.active = false;

            Destroy(this.gameObject);
        }

        private void Update()
        {
            pos = rt.localPosition;

            if (Input.GetKeyUp(KeyCode.P))
            {
                NextLine();
            }
            if (Input.GetKeyUp(KeyCode.O))
            {
                StartDialouge();
            }

            if (open)
            {
                rt.localPosition = new Vector3(0, Mathf.Lerp(rt.localPosition.y, -halfres, lerp), 0);
            }
            else
            {
                rt.localPosition = new Vector3(0, Mathf.Lerp(rt.localPosition.y, -halfres - 700, lerp), 0);
            }

            if (awaiting)
            {
                if (rt.localPosition.y < -540.1f)
                {
                    awaiting = false;
                    NextLine();
                }
            }
            else if (done)
            {
                if (rt.localPosition.y < -540 + 2.5f)
                {
                    CleanupDialouge();
                }
            }
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