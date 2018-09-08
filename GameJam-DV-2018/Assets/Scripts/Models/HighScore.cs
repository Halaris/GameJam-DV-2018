using System.Collections.Generic;

public class HighScore
{
    private long id;
    private string name;
    private long score;

    public long GetId()
    {
        return this.id;
    }
    public HighScore SetId(long id)
    {
        this.id = id;
        return this;
    }
    public string GetName()
    {
        return this.name;
    }
    public HighScore SetName(string name)
    {
        this.name = name;
        return this;
    }
    public long GetScore()
    {
        return this.score;
    }
    public HighScore SetScore(long score)
    {
        this.score = score;
        return this;
    }
}
