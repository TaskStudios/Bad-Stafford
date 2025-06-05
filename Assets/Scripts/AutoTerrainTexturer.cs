using UnityEngine;

[ExecuteInEditMode]
public class AutoTerrainTexturer : MonoBehaviour
{
    public Terrain terrain;
    public float[] heightThresholds = new float[] { 0.3f, 0.6f }; // Grass < 0.3, Dirt < 0.6, Rock >= 0.6

    void Start()
    {
        if (terrain == null)
            terrain = GetComponent<Terrain>();

        ApplyTextures();
    }

    void ApplyTextures()
    {
        TerrainData terrainData = terrain.terrainData;

        int mapWidth = terrainData.alphamapWidth;
        int mapHeight = terrainData.alphamapHeight;
        int numTextures = terrainData.terrainLayers.Length;

        float[,,] splatmapData = new float[mapWidth, mapHeight, numTextures];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float normX = x * 1.0f / (mapWidth - 1);
                float normY = y * 1.0f / (mapHeight - 1);

                float height = terrainData.GetInterpolatedHeight(normX, normY) / terrainData.size.y;

                float[] splat = new float[numTextures];

                if (height < heightThresholds[0])
                {
                    splat[0] = 1f; // Grass
                }
                else if (height < heightThresholds[1])
                {
                    splat[1] = 1f; // Dirt
                }
                else
                {
                    splat[2] = 1f; // Rock
                }

                for (int i = 0; i < numTextures; i++)
                {
                    splatmapData[x, y, i] = splat[i];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, splatmapData);
    }
}