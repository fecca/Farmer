public interface ILevelGenerator
{
    NodeBehaviour[,] GenerateLevel();
    void CenterNodes(int x, int z);
    void SetStartNodes(int x, int z);
}