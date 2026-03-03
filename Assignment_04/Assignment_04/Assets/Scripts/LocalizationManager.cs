using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    English,
    French,
    Spanish
}

public class LocalizationManager : MonoBehaviour
{
    [SerializeField] public static LocalizationManager instance;

    [SerializeField] private LanguageData languageData;
    [SerializeField] private Language currentLanguage = Language.English;

    [SerializeField] private Dictionary<string, LocData> locDictionary;

    private void Awake()
    {
        instance = this;
        BuildDictionary();
    }

    private void BuildDictionary()
    {
        locDictionary = new Dictionary<string, LocData>();

        foreach (var loc in languageData.localizations)
        {
            if (!locDictionary.ContainsKey(loc.key))
            {
                locDictionary.Add(loc.key, loc);
            }
        }
    }

    public string GetText(string key)
    {
        if (!locDictionary.ContainsKey(key))
        {
            return $"MissingKey: {key}";
        }

        LocData data = locDictionary[key];

        switch (currentLanguage)
        {
            case Language.English: return data.en;
            case Language.French: return "[FR]" + data.fr;
            case Language.Spanish: return "[SP]" + data.sp;
            default: return data.en;
        }
    }
}