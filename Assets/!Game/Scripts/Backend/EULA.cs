using UnityEngine;
using Bastion;

namespace BOTD
{
    public class EULA : MonoBehaviour
    {
        private void Start()
        {
            if (SettingsManager.FetchSetting("eula_accepted", "false", "Gameplay", MissingMode.AddThenMinorSave))
            {
                Destroy(this.gameObject);
            }
        }

        public void Disagree()
        {
            Application.Quit();
        }

        public void Accept()
        {
            SettingsManager.ChangeSetting("eula_accepted", "true");

            Destroy(this.gameObject);
        }
    }
}
