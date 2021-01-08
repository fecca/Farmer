public interface ILevelGenerator
{
    float[,] GenerateLevel();
    void CenterNodes(int x, int z);
    void SetStartNodes(int x, int z);
}