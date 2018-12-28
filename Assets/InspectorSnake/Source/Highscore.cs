using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("HighScore")]
public class HighScore
{
    [XmlElement("Name")]
    public string name = "";

    [XmlElement("Score")]
    public int score;
    
    private XmlDocument scoreSheet;

    public void SetHighScore()
    {
        var highScore = Load();

        name = highScore.name;
        score = highScore.score;
    }

    private HighScore Load()
    {
        var path = Path.Combine(Application.dataPath, "InspectorSnake/highscore");

        var serializer = new XmlSerializer(typeof(HighScore));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return (HighScore)serializer.Deserialize(stream);
        }
    }

    public void Save()
    {
        var path = Path.Combine(Application.dataPath, "InspectorSnake/highscore");

        var serializer = new XmlSerializer(typeof(HighScore));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }
}
