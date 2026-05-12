using UnityEngine;

public class CatCustomization : MonoBehaviour
{
    public static CatCustomization instance;
    public Material[] selectedMaterials;

    public Material[] blackCatMaterials;
    public Material[] grayCatMaterials;
    public Material[] whiteCatMaterials;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveMaterials(Material[] mats, string skinName)
    {
        selectedMaterials = mats;
        PlayerPrefs.SetString("CatSkin", skinName);
        PlayerPrefs.Save();
        Debug.Log("Saved skin: " + skinName);
    }

    public void Load()
    {
        string skinName = PlayerPrefs.GetString("CatSkin", "White");
        Debug.Log("Loading skin: " + skinName);

        if (skinName == "Black" && blackCatMaterials != null)
            selectedMaterials = blackCatMaterials;
        else if (skinName == "Gray" && grayCatMaterials != null)
            selectedMaterials = grayCatMaterials;
        else if (skinName == "White" && whiteCatMaterials != null)
            selectedMaterials = whiteCatMaterials;
    }
}