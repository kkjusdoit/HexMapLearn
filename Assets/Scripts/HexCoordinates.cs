
using UnityEngine;
[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z;
    //x y z���==0��
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
        float y = -x;  // z����0 ������£���������

        float offset = position.z / (HexMetrics.outRadius * 3f); //ÿ���еľ�����3�����⾶��ÿ����x y ��ƫ��1
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

            //�����������������Ǹ���ֻ��עx z ��y����ǰ�����ع�
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
