public interface ILevelService
{
    void GenerateLevel();
    float[,] GetLevel();
    void CenterNodes(int x, int z);
    void SetStartNodes(int x, int z);
    void DrawLevel(int x, int z);
}