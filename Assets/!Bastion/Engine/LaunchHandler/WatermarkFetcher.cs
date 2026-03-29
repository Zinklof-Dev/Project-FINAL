using TMPro;
using UnityEngine;

namespace Bastion
{
    public class WatermarkFetcher : MonoBehaviour
    {
        private TMP_Text m_Text;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_Text = GetComponent<TMP_Text>();

            string watermark = SettingsManager.FetchSetting("engine.watermark", "", "Engine");

            string buildnumber = SettingsManager.FetchSetting("engine.build_number", "0", "Engine");

            watermark = watermark.Replace("{bldnmbr}", buildnumber);

            m_Text.text = watermark;
        }
    }
}
