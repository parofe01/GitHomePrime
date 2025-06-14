using UnityEngine;

[CreateAssetMenu(fileName = "NewKanaData", menuName = "Learning/Kana Data")]
public class KanaData : ScriptableObject
{
    [Header("Symbol Info")]
    [Tooltip("The kana character (hiragana or katakana)")]
    public string symbol;

    [Tooltip("Romanization (romaji) of the kana")]
    public string romaji;

    [Header("Audio")]
    [Tooltip("Pronunciation audio clip for this kana")]
    public AudioClip pronunciationClip;

    [Header("Example")]
    [Tooltip("Example word using this kana")]
    public string exampleWord;

    [Tooltip("Audio for the example word pronunciation")]
    public AudioClip exampleWordClip;

    [Header("Classification")]
    [Tooltip("Group classification (Gojuon, Dakuon, Yoon)")]
    public KanaGroup group;
}
