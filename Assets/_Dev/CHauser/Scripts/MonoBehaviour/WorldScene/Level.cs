// - zink
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BOTD.LevelManagement
{
    public class Level : MonoBehaviour
    {
        [Header("Level Info")]
        [SerializeField] public string levelName { get; private set; }
        [SerializeField] [TextArea(8, 12)] public string description { get; private set; }
        [SerializeField] public Scene scene { get; private set; }

        public void CreateLevelGUI()
        {
            LevelGUI.instance.ChangeData(levelName, description, scene.buildIndex);
        }
    }
}