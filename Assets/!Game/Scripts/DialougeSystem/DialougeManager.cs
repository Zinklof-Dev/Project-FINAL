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
        [SerializeField] float timePerChar = 0.05f;
        [SerializeField] float dialougeTimeScale = 0f;
        [Header("References")]
        [SerializeField] Volume volume;
        [SerializeField] TMP_Text dialougeBox;
        [SerializeField] TMP_Text speakerName;
    
        int currentLine = -1; // -1 = no dialouge right now;

        float returnTimeScale;

        DepthOfField dof;

        private void Start()
        {
            if (!volume.profile.TryGet<DepthOfField>(out dof))
            {
                Debug.LogWarning("Volume does not have a depth of field!");
            }

            StartDialouge();
        }
        
        public void StartDialouge()
        {
            Time.timeScale = dialougeTimeScale;

            dof.active = true;
            
            NextLine();
        }

        public void NextLine()
        {
            StopAllCoroutines();

            currentLine++;

            if (currentLine > lines.Length - 1)
            {
                cleanUpDialouge();
                return;
            }

            speakerName.text = speakers[lines[currentLine].speaker].name;
            dialougeBox.text = "";

            StartCoroutine(Typewriter());
        }

        private void cleanUpDialouge()
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
        }
    }
}