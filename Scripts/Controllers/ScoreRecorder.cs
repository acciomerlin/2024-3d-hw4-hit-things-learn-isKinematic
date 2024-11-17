public class ScoreRecorder
{
    private int points = 0; 

    public void Record(DiskModel disk)
    {
        points += disk.points; 
    }

    public int GetPoints()
    {
        return points; 
    }

    public void Reset()
    {
        points = 0; 
    }
}
