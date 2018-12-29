using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class HighScore
{
    private XmlDocument scoreSheet;

    public string name;
    public int score;

    private HighScoreSerializable serializable;

    public HighScore()
    {
        Start();
    }

    void Start()
    {
        serializable = new HighScoreSerializable();
        SetHighScore();
    }

    public void SetHighScore()
    {
        var highScore = Load();

        name = highScore.name;
        score = highScore.score;
    }

    private HighScoreSerializable Load()
    {
        var path = Path.Combine(Application.dataPath, "InspectorSnake/highscore");
        using (var stream = new FileStream(path, FileMode.Open))
        {
            var serializer = new XmlSerializer(typeof(HighScoreSerializable));
            return (HighScoreSerializable)serializer.Deserialize(stream);
        }
    }

    public void Save()
    {
        var path = Path.Combine(Application.dataPath, "InspectorSnake/highscore");
        
        using (var stream = new FileStream(path, FileMode.Create))
        {
            var serializer = new XmlSerializer(typeof(HighScoreSerializable));

            serializable.score = score;
            serializable.name = name;

            serializer.Serialize(stream, serializable);
        }
    }

    public static int GetScore(Snake snake)
    {
        return snake.snakeBlocksPositions.Count - 4;
    }
}
