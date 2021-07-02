using System;

[Serializable]
public class User
{
    public static User Empty = new User();

    public string Name;
    public int HighScore;
}
