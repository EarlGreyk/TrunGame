using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 관찰자 인터페이스
public interface IResourceObserver
{
    void OnResourceUpdated(int metal, int tree, int stone);
}

// 관찰 가능한 주제 클래스
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

// 구체적인 관찰자 클래스
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

        // 블록 생성
        Blockobserver block = new Blockobserver();

        // 블록을 플레이어의 관찰자로 등록
        player.AddObserver(block);

        // 플레이어의 자원 변경
        player.AddMetal(10);
        player.AddTree(5);
        player.AddStone(3);
    }
}
