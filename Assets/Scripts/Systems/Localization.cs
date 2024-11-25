using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class Localization : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] List<LanguageSetup> Localize;
    
    void OnValidate()
    {
        if(!Text) Text = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        EnumLanguage Language = YandexGame.lang == "ru" ? EnumLanguage.RU : EnumLanguage.EN;
        Text.text = Localize.Find(x => x.Language == Language).Hint;
    }
}