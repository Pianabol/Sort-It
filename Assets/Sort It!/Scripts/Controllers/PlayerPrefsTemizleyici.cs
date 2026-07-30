using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class PlayerPrefsTemizleyici
{
    // Unity'nin en üstündeki Tools menüsüne sihirli bir buton ekler!
    [MenuItem("Tools/ 🧹 Hafızayı Temizle (PlayerPrefs)")]
    public static void Temizle()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Moruk hafıza cillop gibi oldu, her şey 1. Level'a sıfırlandı!");
    }
}
#endif