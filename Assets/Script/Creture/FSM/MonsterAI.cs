using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


[System.Serializable]
public class Node
{
    public Node(bool _isWall,bool _isRe ,int _x, int _z) { isWall = _isWall; isRe = _isRe;  x = _x; z = _z; }

    public bool isWall;
    public bool isRe;
    public Node ParentNode;

    // G : �������κ��� �̵��ߴ� �Ÿ�, H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�, F : G + H
    public int x, z, G, H;
    public int F { get { return G + H; } }
}


public class MonsterAI : MonoBehaviour
{
    [SerializeField]
    private MonsterCreture monster;
    public Vector3Int bottomLeft, topRight, startPos, targetPos;
    public List<Node> FinalNodeList;
    public bool dontCrossCorner;

    int sizeX;
    int sizeZ;
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

    private void Start()
    {
        dontCrossCorner = true;
    }

    public void PathFinding()
    {
      
        sizeX = topRight.x/2 - bottomLeft.x/2 +1;
        sizeZ = topRight.z/2 - bottomLeft.z/2 +1;
        NodeArray = new Node[sizeX, sizeZ];

        //�����ü�� ��ȸ�Ͽ� ó�� [���Ͱ� �������� ���ϴ� ���� üũ��].
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                bool isWall = false;
                bool isRe = false;
                Block block = GameManager.instance.BlockArray[i, j];
                if (block.MonsterBuild != null) isWall = true;
                if (block.Monster != null && block.Monster != monster) isWall = true;
                if (block.ObjectValue != 0) isRe = true;

                


                NodeArray[i, j] = new Node(isWall,isRe, i + bottomLeft.x, j + bottomLeft.z);
            }
        }


        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        StartNode = NodeArray[startPos.x / 2 - bottomLeft.x, startPos.z / 2 - bottomLeft.z];
        TargetNode = NodeArray[targetPos.x / 2 - bottomLeft.x, targetPos.z / 2 - bottomLeft.z];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        while (OpenList.Count > 0)
        {
            // ��������Ʈ �� ���� F�� �۰� F�� ���ٸ� H�� ���� �� ������� �ϰ� ��������Ʈ���� ��������Ʈ�� �ű��
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);
            // ������
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                for (int i = 0; i < FinalNodeList.Count; i++) print(i + "��°�� " + FinalNodeList[i].x + ", " + FinalNodeList[i].z);
                return;
            }

            // �� �� �� ��
            OpenListAdd(CurNode.x, CurNode.z + 1);
            OpenListAdd(CurNode.x + 1, CurNode.z);
            OpenListAdd(CurNode.x, CurNode.z  - 1);
            OpenListAdd(CurNode.x - 1, CurNode.z);
        }
    }

    void OpenListAdd(int checkX, int checkY)            
    {
        if (checkX >= bottomLeft.x && checkX < topRight.x/2 + 1 && checkY >= bottomLeft.z && checkY < topRight.z / 2 + 1 
            && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.z].isWall 
            &&!ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.z]))

        {
     
            if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.z].isWall && 
                NodeArray[checkX - bottomLeft.x, CurNode.z - bottomLeft.z].isWall) return;

            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.z].isWall 
                    || NodeArray[checkX - bottomLeft.x, CurNode.z - bottomLeft.z].isWall) return;

            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.z];
            int MoveCost = CurNode.G + 10;
            MoveCost = CurNode.isRe == false ? MoveCost : MoveCost * 2;

            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.z - TargetNode.z)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }
    //�̵��ؾ��� Ÿ�ٺ�� ��ȯ.
    public Block TargetBlock()
    {
        startPos = Vector3Int.FloorToInt(monster.transform.position);
        targetPos = Vector3Int.FloorToInt(monster.PTarget.transform.position);
        PathFinding();
        int x = FinalNodeList[1].x;
        int z = FinalNodeList[1].z;
        Debug.Log(x*2 + " " +  z*2);
        Block targetBlock = GameManager.instance.BlockArray[x, z];
        Debug.Log(targetBlock);

        return targetBlock;
    }

  
}


