// - zink
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BOTD.LevelManagement
{
    public class Level : MonoBehaviour
    {
        [Header("Level Info")]
        [SerializeField] public string levelName;
        [SerializeField] [TextArea(8, 12)] public string description;
        [SerializeField] public int scene;

        public void CreateLevelGUI()
        {
            LevelGUI.instance.ChangeData(levelName, description, scene);
        }
    }
}