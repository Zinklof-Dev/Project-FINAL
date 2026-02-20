using System;
using UnityEngine;
using UnityEngine.UI;

namespace BOTD.Dialouge
{
    [Serializable]
    public struct Line
    {
        [SerializeField] public string text;
        [SerializeField] public byte speaker;
    }

    [Serializable]
    public struct Speaker
    {
        [SerializeField] public string name;
        [SerializeField] public Sprite sprite;

        [SerializeField] private bool overrideColors;
        [SerializeField] private Color notSpeaking;
        [SerializeField] private Color speaking;
    }
}
