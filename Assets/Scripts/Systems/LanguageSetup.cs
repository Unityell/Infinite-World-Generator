using UnityEngine;

[System.Serializable]
public class LanguageSetup
{
    public EnumLanguage Language;
    [TextArea(3, 10)] public string Hint;
}