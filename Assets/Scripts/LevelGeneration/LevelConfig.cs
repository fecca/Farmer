using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public List<NoiseLayer> noiseLayers;
    public float waterLevel;
    public float globalNoiseScale;
    public int width;
    public int height;
    public int seed;
    public float worldElevation;
    public int xChunks;
    public int zChunks;
}