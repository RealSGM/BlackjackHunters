[System.Serializable]
public class Troop
{
    private string name;
    private string type;
    private int attack;
    private int defence;
    private int health;
    private int level;
    private float rangeCoefficient;
    private bool isSelected;
    private bool statsUpgraded;
    private bool isUpgraded;

    public bool IsUpgraded
    {
        get { return isUpgraded; }
        set { isUpgraded = value; }
    }

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

    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }

    public bool StatsUpgraded
    {
        get { return statsUpgraded; }
        set { statsUpgraded = value; }
    }

    public Troop(string _name, string _type, int _attack, int _defence, int _health, int _level, float _rangeCoeff, bool _isSelected, bool _statsUpgraded)
    {
        name = _name;
        attack = _attack;
        defence = _defence;
        health = _health;
        level = _level;
        type = _type;
        rangeCoefficient = _rangeCoeff;
        isSelected = _isSelected;
        StatsUpgraded = _statsUpgraded;
    }
}