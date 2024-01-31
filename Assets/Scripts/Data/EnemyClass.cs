public class Enemy
{
    private string name;
    private string type;
    private int attack;
    private int defence;
    private int health;
    private int level;
    private float rangeCoefficient;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    public int Attack
    {
        get { return attack; }
        set { attack = value; }
    }

    public int Defence
    {
        get { return defence; }
        set { defence = value; }
    }

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public float RangeCoefficient
    {
        get { return rangeCoefficient; }
        set { rangeCoefficient = value; }
    }

    public Enemy(string _name, string _type, int _attack, int _defence, int _health, int _level, float _rangeCoeff)
    {
        name = _name;
        attack = _attack;
        defence = _defence;
        health = _health;
        level = _level;
        type = _type;
        rangeCoefficient = _rangeCoeff;
    }
}