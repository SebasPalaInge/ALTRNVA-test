using System.Collections.Generic;

[System.Serializable]
public class Block
{
    public int R;
    public int C;
    public int number;
}

[System.Serializable]
public class Blocks
{
    public List<Block> blocks;
}
