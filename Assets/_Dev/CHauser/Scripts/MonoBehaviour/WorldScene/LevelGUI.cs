// - zink
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BOTD.LevelManagement
{
    public class LevelGUI : MonoBehaviour
    {
        static public LevelGUI instance;
        [SerializeField] public TMP_Text nameText;
        [SerializeField] public TMP_Text descText;
        [SerializeField] public int sceneID;
        [SerializeField] Image img;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] RectTransform rectTransform;
        [SerializeField] Vector3 closedScale;
        [SerializeField] Vector3 closedPOS;
        [SerializeField] float lerp;

        private Vector3 startScale;
        private Vector3 startPOS;

        bool open;
        float timer = 1f;

        private void Start()
        {
            // this is purposeful, incase the map is ever re-opened it will change the static instance to the new object so we don't have to DNDOL it.
            instance = this;

            startScale = rectTransform.localScale;
            startPOS = rectTransform.localPosition;

            rectTransform.localScale = closedScale;
            rectTransform.localPosition = closedPOS;
        }

        // bool exists incase anyone interfaces in another script in the future, I don't use it tbh.
        public void ChangeData(string name, string desc, int sceneID, Sprite sprite, bool openGUI = true)
        {
            timer = 1f;

            nameText.text = name;
            descText.text = desc;
            this.sceneID = sceneID;
            img.sprite = sprite;

            if (!openGUI)
                return;

            open = true;

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        public void CloseGUI()
        {
            if (timer >= 0)
                return;

            open = false;

            //canvasGroup.alpha = 0f;
            //canvasGroup.blocksRaycasts = false;
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(sceneID);
        }

        public void Update()
        {
            timer -= Time.deltaTime;

            if (open)
            {
                rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, startScale, lerp);
                rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, startPOS, lerp);
            }
            else
            {
                rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, closedScale, lerp/2);
                rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, closedPOS, lerp/2);
            }
        }
    }
}