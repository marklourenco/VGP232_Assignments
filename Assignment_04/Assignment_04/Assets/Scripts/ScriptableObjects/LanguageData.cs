using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageData", menuName = "Cards/LanguageData")]
public class LanguageData : ScriptableObject
{
    [SerializeField] public List<LocData> localizations = new List<LocData>();
}