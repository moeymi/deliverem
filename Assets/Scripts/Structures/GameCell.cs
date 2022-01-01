public class GameCell
{
    public GameCellType type;
    public int id;
    public GameCell() { }

    public GameCell(GameCell cell)
    {
        this.type = cell.type;
        this.id = cell.id;
    }
    public GameCell(GameCellType type = GameCellType.Empty, int id = -1)
    {

        this.type = type;
        this.id = id;
    }
}
