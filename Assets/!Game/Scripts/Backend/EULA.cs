using UnityEngine;

namespace BOT
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
            Application.Exit();
        }

        public void Accept()
        {
            SettingsManager.ChangeSetting("eula_accepted", "true");

            Destory(this.gameObject);
        }
    }
}
