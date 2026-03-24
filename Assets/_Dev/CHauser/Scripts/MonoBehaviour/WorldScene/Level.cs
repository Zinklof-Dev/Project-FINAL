using UnityEngine;

namespace BOTD.LevelManagement
{
    public class Level : Monobehaviour
    {
        [Header("Level Info")]
        [SerializeField] string name { get; private set; }
        [SerializeField] [TextArea(8, 12)] string description { get; private set; }
        [SerializeField] Scene scene { get; private set; }

        public void CreateLevelGUI()
        {
            LevelGUI.instance.ChangeData(name, description, scene.buildIndex);
        }
    }
}