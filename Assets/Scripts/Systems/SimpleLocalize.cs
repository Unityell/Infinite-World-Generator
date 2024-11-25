using UnityEngine;

[System.Serializable]
public class SimpleLocalize
{
    public EnumLang Lang;
    [TextArea()] public string Text;
}