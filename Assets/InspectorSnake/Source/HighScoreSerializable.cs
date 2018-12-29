using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;


//Made as a separate class in order to avoid infinite constructor loop
[XmlRoot("HighScoreSerializable")]
public class HighScoreSerializable {

    [XmlElement("Name")]
    public string name;

    [XmlElement("Score")]
    public int score;

    public HighScoreSerializable()
    {

    }
}
