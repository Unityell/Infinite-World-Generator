using UnityEngine;

public class GameInfo
{
    public EnumLang GetLang()
    {
        switch (PlayerPrefs.GetString("Lang"))
        {
            case "en" :
                return EnumLang.en;
            case "ru" :
                return EnumLang.ru;
            case "tr" :
                return EnumLang.tr;
            default: return EnumLang.en;
        }
    }
}