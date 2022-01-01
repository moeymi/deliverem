public class GameCell
{
    public GameCellType type;
    public int id;
    public GameCell(GameCellType type = GameCellType.Empty, int id = -1)
    {
        this.type = type;
        this.id = id;
    }
}
