using UnityEngine;

public class Background : MonoBehaviour
{
    [Header("Assets")]
    public Texture2D sourceImage; 
    public GameObject cubePrefab; 

    [Header("Settings")]
    public int width = 100;  
    public int height = 200; 
    public float cubeSpacing = 1.0f;
    
    [ContextMenu("Generate Background")]

    void Start()
    {
    
    Generate();
    }
    public void Generate()
    {
        
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        if (sourceImage == null || cubePrefab == null)
        {
            Debug.LogError("Please assign a Source Image and Cube Prefab!");
            return;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Calculate normalized coordinates to sample the texture correctly
                float normX = (float)x / width;
                float normY = (float)y / height;

                // Get color from the source image
                Color pixelColor = sourceImage.GetPixelBilinear(normX, normY);

                // Calculate position
                Vector3 pos = new Vector3(x * cubeSpacing, y * cubeSpacing, 0);

                // Instantiate cube and set color
                GameObject newCube = Instantiate(cubePrefab, pos, Quaternion.identity, transform);
                newCube.name = $"Pixel_{x}_{y}";

                // Apply color to the material
                Renderer rend = newCube.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.material.color = pixelColor;
                    
                   
                }
            }
        }
    }
}