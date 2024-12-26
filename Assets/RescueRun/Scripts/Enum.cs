using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RescueRun
{
    public enum GameState : byte
    {
        None = 0,
        Start = 1,
        Win = 2,
        Lose = 3,
        Pause = 4,
        Setup = 5,
        MainMenu = 6,
    }

    public enum LineState : byte
    {
        None = 0,
        Start = 1,
        Line1 = 2,
        Line2 = 3,
    }

    public enum ItemType : byte
    {
        None = 0,
        Shoes = 1,
        Cloth = 2,
    }

    public enum BuffType : byte
    {
        None = 0,
        Speed = 1,
        Stamina = 2,
        Income = 3,
    }

    public enum LevelType : byte
    {
        None = 0,
        Easy = 1,
        Medium = 2,
        Hard = 3,
    }
}

