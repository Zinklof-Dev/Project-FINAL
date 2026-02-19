using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

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

        private void start()
        {
            if (!volume.profile.TryGet<DepthOfFeild>(out dof))
            {
                Debug.LogWarning("Volume does not have a depth of field!")
            }                
        }
        
        public void StartDialouge()
        {
            Time.timeScale = dialougeTimeScale;

            dof.active = true;
            
            NextLine();
        }

        public void NextLine()
        {
            currentLine++:

            speakerName = speakers[lines[currentLine].speaker].name;
            dialougeBox.text = "";
        }

        private void cleanUpDialouge()
        {
            Time.timeScale = returnTimeScale;

            dof.active = false;
        }

        IEnumerator Typewriter()
        {
            foreach (char c in lines[currentLine].text)
            {
                dialougeBox.text += c;
                yield await new WaitForSeconds(timePerChar);
            }
        }
    }
}
