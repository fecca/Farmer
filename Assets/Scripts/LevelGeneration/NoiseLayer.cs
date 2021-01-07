using UnityEngine;

[System.Serializable]
public class NoiseLayer
{
    [SerializeField] private float noisePower = 1;
    [SerializeField] private Vector2 noiseOffset;
    [SerializeField] private float noiseScale = 1;

    public float Evaluate(float x, float z, int seed)
    {
        x += seed;
        z += seed;

        var noiseXCoord = noiseOffset.x + x * noiseScale;
        var noiseZCoord = noiseOffset.y + z * noiseScale;


        return (Mathf.PerlinNoise(noiseXCoord, noiseZCoord) - 0.5f) * noisePower;
    }
}