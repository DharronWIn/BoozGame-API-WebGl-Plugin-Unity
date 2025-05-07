using System;
using UnityEngine;
namespace BoozGameAPIProvider
{
[Serializable]

public class ResultDto
{
    public string id;
    public int highScore;
    public object gameData;
    public DateTime createdAt;
    public DateTime updatedAt;
    public ClientDto user;
    public Game game;
}
}
