// CustomizationMenu.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomizationMenu : MonoBehaviour
{
    [System.Serializable]
    public class CatSkin
    {
        public string skinName;
        public Material arms;
        public Material body;
        public Material eyes;
        public Material legs;
    }

    public CatSkin[] availableSkins;
    public GameObject catPreview;
    public TMPro.TextMeshProUGUI selectedColorText;

    private string[] bodyNames = { "Cube.001" };
    private string[] legNames  = { "Cube.002", "Cube.003" };
    private string[] armNames  = { "Cube.004", "Cube.005" };
    
    private string[] eyeNames  = { "Cube.008", "Cube.009" };

    void Start()
{
    if (CatCustomization.instance != null &&
        CatCustomization.instance.selectedMaterials != null &&
        CatCustomization.instance.selectedMaterials.Length == 4)
    {
        ApplyToPreview(CatCustomization.instance.selectedMaterials);
    }
    else
    {
        SelectSkin(1); // default to White (index 1)
    }
}

    public void SelectSkin(int index)
    {
        if (index >= availableSkins.Length) return;
        CatSkin skin = availableSkins[index];
        Material[] mats = new Material[] { skin.arms, skin.body, skin.eyes, skin.legs };
        ApplyToPreview(mats);
        if (CatCustomization.instance != null)
            CatCustomization.instance.SaveMaterials(mats, skin.skinName);
    }

    void ApplyToPreview(Material[] mats)
    {
        if (catPreview == null) return;
        Renderer[] renderers = catPreview.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            string n = r.gameObject.name;

            if      (System.Array.IndexOf(armNames,  n) >= 0) r.material = mats[0];
            else if (System.Array.IndexOf(bodyNames, n) >= 0) r.material = mats[1];
            else if (System.Array.IndexOf(eyeNames,  n) >= 0) r.material = mats[2];
            else if (System.Array.IndexOf(legNames,  n) >= 0) r.material = mats[3];
           
        }
    }

    public void SaveAndReturn()
    {
        SceneManager.LoadScene("MainMenu");
    }
}