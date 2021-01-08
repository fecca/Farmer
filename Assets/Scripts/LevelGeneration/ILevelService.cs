public interface ILevelService
{
    void GenerateLevel();
    NodeBehaviour[,] GetLevel();
    void CenterNodes(int x, int z);
    void SetStartNodes(int x, int z);
}