using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �������̽�
public interface IResourceObserver
{
    void OnResourceUpdated(int metal, int tree, int stone);
}

// ���� ������ ���� Ŭ����
public class PlayerResourceManager
{
    private int metal;
    private int tree;
    private int stone;
    private List<IResourceObserver> observers = new List<IResourceObserver>();

    public void AddObserver(IResourceObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IResourceObserver observer)
    {
        observers.Remove(observer);
    }

    public void AddMetal(int amount)
    {
        metal += amount;
        NotifyObservers();
    }

    public void AddTree(int amount)
    {
        tree += amount;
        NotifyObservers();
    }

    public void AddStone(int amount)
    {
        stone += amount;
        NotifyObservers();
    }
    private void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.OnResourceUpdated(metal, tree, stone);
        }
    }
}

// ��ü���� ������ Ŭ����
public class Blockobserver : IResourceObserver
{
    private string playerName;

    public void OnResourceUpdated(int metal, int tree, int stone)
    {
        Console.WriteLine($"Player resources updated: Metal = {metal}, Tree = {tree}, Stone = {stone}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        PlayerResourceManager player = new PlayerResourceManager();

        // ��� ����
        Blockobserver block = new Blockobserver();

        // ����� �÷��̾��� �����ڷ� ���
        player.AddObserver(block);

        // �÷��̾��� �ڿ� ����
        player.AddMetal(10);
        player.AddTree(5);
        player.AddStone(3);
    }
}
