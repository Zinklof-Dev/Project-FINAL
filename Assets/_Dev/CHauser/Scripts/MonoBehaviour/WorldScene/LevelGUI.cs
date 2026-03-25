// - zink
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace BOTD.LevelManagement
{
    public class LevelGUI : MonoBehaviour
    {
        static public LevelGUI instance;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text descText;
        [SerializeField] int sceneID;
        [SerializeField] CanvasGroup canvasGroup;

        private void Start()
        {
            // this is purposeful, incase the map is ever re-opened it will change the static instance to the new object so we don't have to DNDOL it.
            instance = this;
        }

        // bool exists incase anyone interfaces in another script in the future, I don't use it tbh.
        public void ChangeData(string name, string desc, int sceneID, bool openGUI = true)
        {
            nameText.text = name;
            descText.text = desc;
            this.sceneID = sceneID;

            if (!openGUI)
                return;

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        public void CloseGUI()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(sceneID);
        }
    }
}