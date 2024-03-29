using System.Collections.Generic;

[System.Serializable]
public class Star
{
    public List<Orbits> Orbits;
    public int BaseEnergyPerTouch;
    public int BaseUpgradePrise;
    public int UpgradeCount;

    public int EnergyPerTouch
    {
        get {
            return (int)(BaseEnergyPerTouch + UpgradeCount);
        }
    }

    public int UpgradePrice
    {
        get {
            return (int)(EnergyPerTouch * (1 + (1 * UpgradeCount)));
        }
    }
}
