using System;
using UnityEngine;

namespace BOTD.Dialouge
{
    [Serializeable]
    public struct Line
    {
        [SerializeField] private string text;
        [SerializeField] private byte speaker; // 0 always = player
    }

    [Serializeable]
    public struct speaker
    {
        [SerializeField] private string name;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Vector2 scale;

        [SerializeField] private bool overrideColors;
        [SerializeField] private Color notSpeaking;
        [SerializeField] private Color speaking;
    }
}
