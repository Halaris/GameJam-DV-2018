using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;

public class HighScoreService
{

    private string dbPath;

    public HighScoreService(string dbPath)
    {
        this.dbPath = dbPath;
    }

    public void CreateTable()
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS HighScore (" +
                                    " _id INTEGER PRIMARY KEY," +
                                    " name TEXT NOT NULL," +
                                    " score INTEGER NULL" +
                                    ");";
                var result = cmd.ExecuteNonQuery();
                Debug.Log("create table HighScore: " + result);
            }
            conn.Close();
        }
    }

    public long Insert(string name, long score)
    {
        long id = 0;
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (SqliteTransaction tr = conn.BeginTransaction())
            {
                try
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO HighScore" +
                                            " (name, score)" +
                                            " VALUES" +
                                            " (:name, :score);" +
                                            "SELECT last_insert_rowid() as id;";
                        cmd.Parameters.AddWithValue("name", name);
                        cmd.Parameters.AddWithValue("score", score);

                        id = (long)cmd.ExecuteScalar();
                        Debug.Log("insert HighScore: " + id);
                    }
                    tr.Commit();
                }
                catch (System.Exception e)
                {
                    Debug.Log("insert HighScore: " + e.Message);
                    if (tr != null)
                        tr.Rollback();
                }
            }
            conn.Close();
        }
        return id;
    }
    
    public List<HighScore> List()
    {
        List<HighScore> highScoreList = new List<HighScore>();
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT _id, name, score" +
                                    " FROM HighScore" +
                                    " ORDER BY score DESC" +
                                    " LIMIT 10;";

                using (var result = cmd.ExecuteReader())
                {
                    Debug.Log("list HighScore: " + result.RecordsAffected);
                    if (result.HasRows)
                    {
                        Debug.Log("tests");
                        HighScore highScore;
                        while (result.Read())
                        {
                            highScore = new HighScore();
                            highScore
                                .SetId(result.GetInt64(0))
                                .SetName(result.GetString(1))
                                .SetScore(result.GetInt64(2));
                            highScoreList.Add(highScore);
                        }
                    }
                }
            }
            conn.Close();
        }
        return highScoreList;
    }
    
}
