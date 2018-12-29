using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("HighScoreSerializable")]
public class HighScoreSerializable {

    [XmlElement("Name")]
    public string name;

    [XmlElement("Score")]
    public int score;

    public HighScoreSerializable()
    {
        Start();
    }

    void Start()
    {

    }
}
