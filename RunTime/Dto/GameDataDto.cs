using UnityEngine;
using System;
namespace BoozGameAPIProvider
{
    [Serializable]
    public class GameDataDto
    {
        public string gameId;
        public int score;
        public GameProgress gameProgress;
    }

    [Serializable]
    public class GameProgress
    {
        public int level;
        public int progress;
    }

    [Serializable]
    public class Game
    {
        public string id;
        public string title;
        public string description;
        public int numberPlayed;
        public string url;
        public string videoCover;
        public bool isPortrait;
        public bool isActive;
        public DateTime createdAt;
        public DateTime updatedAt;
    }
}
//?q=&gameId=e7adb9d8-7979-4529-a38c-4a94b0af2218
//http://localhost:5473/?q=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI3MjY2MjYxNC1kOWZjLTQ4ZGMtOTYzOS1kY2I4N2UwNDllNjAiLCJpYXQiOjE3NDY1MjE1MjUsImV4cCI6MTc0Njc4MDcyNX0.9qEZPOFmZYUnquuVSbaL3cL9PkBgrnNCSCSKjzqkEiI&gameId=e7adb9d8-7979-4529-a38c-4a94b0af2218