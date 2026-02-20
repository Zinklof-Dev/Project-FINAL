/*
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using System.Collections;
using System.Linq.Expressions;

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
    
        int currentLine = -1; // -1 = no dialouge right now;

        float returnTimeScale;

        DepthOfField dof;

        RectTransform rt;
        bool open, awaiting, done = false;

        private void Start()
        {
            if (!volume.profile.TryGet<DepthOfField>(out dof))
            {
                Debug.LogWarning("Volume does not have a depth of field!");
            }

            StartDialouge();

            rt = gameObject.GetComponent<RectTransform>();

            rt.anchoredPosition.y = -rt.achoredPosition.height
        }
        
        public void StartDialouge()
        {
            returnTimeScale = Time.timeScale;
            Time.timeScale = dialougeTimeScale;

            dof.active = true;

            open = true;
            awaiting true;
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

            Speaker s = lines[currentLine].speaker;

            if (s.overrideColors)
                s.image.color = s.speaking;
            else
                s.image.color = Color.white;

            speakerName.text = s.name;
            dialougeBox.text = "";

            StartCoroutine(Typewriter());
        }

        private void cleanupDialouge()
        {
            Time.timeScale = returnTimeScale;

            dof.active = false;

            Destroy(this.gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                NextLine();
            }
        }

        IEnumerator Typewriter()
        {
            foreach (char c in lines[currentLine].text)
            {
                dialougeBox.text += c;
                yield return new WaitForSecondsRealtime(timePerChar);
            }
            if (open)
            {
                rt.anchoredPosition.y = mathF.lerp(rt.anchoredPosition.y, 0, lerp);
            }
            else
            {
                rt.anchoredPosition.y = mathF.lerp(rt.anchoredPosition.y, -rt.anchoredPosition.height, lerp);
            }

            if (awaiting)
            {
                if (rt.anchoredPosition.y > -2.5f)
                {
                    rt.anchoredPosition.y = 0;
                    
                    NextLine();
                    awaiting = false;
                }
            }
            else if (done)
            {
                if (rt.anchoredPosition.y <-rt.anchoredPosition.height + 2.5f)
                {
                    CleanupDialouge();
                }
            }
        }
    }
}
*/
