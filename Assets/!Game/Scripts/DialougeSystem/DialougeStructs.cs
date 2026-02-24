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
        [SerializeField] public byte expression;
    }

    [Serializable]
    public struct Speaker
    {
        [SerializeField] public string name;
        [SerializeField] public Sprite[] expressions;
        [SerializeField] public Transform lookAtPoint;
    }
}
