[System.Serializable]
public class Orbits
{
    public bool IsHave;
    public int Count;
    public int BaseValue;
    public float BaseSpeed;
    public int BaseValuePrice;
    public int BaseCountPrice;
    public int BaseSpeedPrice;
    public int ValueUpgrade;
    public int SpeedUpgrade;

    public int ValuePrice
    {
        get {
            return (int)(BaseValuePrice * (1 + (0.5 * ValueUpgrade)));
        }
    }

    public int Value
    {
        get {
            return (int)(BaseValue * (1 + (0.5 * ValueUpgrade)));
        }
    }

    public int CountPrice
    {
        get {
            return (int)(BaseCountPrice * (1 + (1 * (Count - 1))));
        }
    }

    public int SpeedPrice
    {
        get {
            return (int)(BaseSpeedPrice * (1 + (0.5 * SpeedUpgrade)));
        }
    }

    public int Speed
    {
        get {
            return (int)(BaseSpeed * (1 + (0.2 * SpeedUpgrade)));
        }
    }
}
