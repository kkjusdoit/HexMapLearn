
using UnityEngine;
[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z;
    //x y z相加==0，
    public int X { get { return x; } }
    public int Z { get { return z; } }
    public int Y {
        get { return -x - z; }
    }



    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x, z);
    }

    public override string ToString()
    {
        return "(" + X.ToString() + ", "+ Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }

    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;  // z等于0 的情况下，是这样的

        float offset = position.z / (HexMetrics.outRadius * 3f); //每两行的距离是3倍的外径，每两行x y 各偏移1
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);
        if (iX + iY + iZ != 0)
        {
            // the coordinate that got rounded the most is incorrect.
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(iZ - iZ);

            //丢弃四舍五入最多的那个，只关注x z ，y依赖前两者重构
            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }

            iY = -iX - iZ;
            if (iX + iY + iZ != 0)
            {
                Debug.LogWarning("rounding error!! " + iX + " " + iY + " " + iZ);

            }
        }
        return new HexCoordinates(iX, iZ);
    }
}
