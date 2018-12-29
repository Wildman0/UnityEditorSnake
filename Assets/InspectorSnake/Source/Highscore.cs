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

    //Runs at start
    void Start()
    {
        serializable = new HighScoreSerializable();
        SetHighScore();
    }

    //Sets the high score to the current highscore, as defined in the XML file
    public void SetHighScore()
    {
        var highScore = Load();

        name = highScore.name;
        score = highScore.score;
    }

    //Loads the highscore and name from an XML file
    private HighScoreSerializable Load()
    {
        var path = Path.Combine(Application.dataPath, "InspectorSnake/highscore");
        using (var stream = new FileStream(path, FileMode.Open))
        {
            var serializer = new XmlSerializer(typeof(HighScoreSerializable));
            return (HighScoreSerializable)serializer.Deserialize(stream);
        }
    }

    //Saves the highscore and name to an XML file
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
    
    //Gets the score of a given snake
    public static int GetScore(Snake snake)
    {
        return snake.snakeBlocksPositions.Count - 4;
    }
}
