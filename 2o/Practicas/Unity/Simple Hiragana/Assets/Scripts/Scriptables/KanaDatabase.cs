using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "KanaDatabase", menuName = "Learning/Kana Database")]
public class KanaDatabase : ScriptableObject
{
    [Tooltip("List of all hiragana kana entries")]
    public KanaData[] hiragana;

    [Tooltip("List of all katakana kana entries")]
    public KanaData[] katakana;

    /// <summary>
    /// Returns all kana regardless of script.
    /// </summary>
    public IEnumerable<KanaData> GetAll()
    {
        return hiragana.Concat(katakana);
    }

    /// <summary>
    /// Returns kana filtered by group phase (Gojuon, Dakuon, Yoon).
    /// </summary>
    public KanaData[] GetByGroup(KanaGroup group)
    {
        return GetAll().Where(k => k.group == group).ToArray();
    }

    /// <summary>
    /// Returns all hiragana entries.
    /// </summary>
    public KanaData[] GetHiragana() => hiragana;

    /// <summary>
    /// Returns all katakana entries.
    /// </summary>
    public KanaData[] GetKatakana() => katakana;

    /// <summary>
    /// Returns hiragana filtered by group phase.
    /// </summary>
    public KanaData[] GetHiraganaByGroup(KanaGroup group)
    {
        return hiragana.Where(k => k.group == group).ToArray();
    }

    /// <summary>
    /// Returns katakana filtered by group phase.
    /// </summary>
    public KanaData[] GetKatakanaByGroup(KanaGroup group)
    {
        return katakana.Where(k => k.group == group).ToArray();
    }

    // Convenience methods for each group
    public KanaData[] GetGojuon() => GetByGroup(KanaGroup.Gojuon);
    public KanaData[] GetDakuon()  => GetByGroup(KanaGroup.Dakuon);
    public KanaData[] GetYoon()    => GetByGroup(KanaGroup.Yoon);
    public KanaData[] GetHiraganaGojuon() => GetHiraganaByGroup(KanaGroup.Gojuon);
    public KanaData[] GetHiraganaDakuon() => GetHiraganaByGroup(KanaGroup.Dakuon);
    public KanaData[] GetHiraganaYoon()   => GetHiraganaByGroup(KanaGroup.Yoon);
    public KanaData[] GetKatakanaGojuon() => GetKatakanaByGroup(KanaGroup.Gojuon);
    public KanaData[] GetKatakanaDakuon() => GetKatakanaByGroup(KanaGroup.Dakuon);
    public KanaData[] GetKatakanaYoon()   => GetKatakanaByGroup(KanaGroup.Yoon);
}