using System;
using UnityEngine;
[Serializable]
[CreateAssetMenu(menuName = "Car config", fileName = "CarsConfig", order = 0)]
public class CarsConfig : ScriptableObject
{
    public CarProperties[] cars;
}

[Serializable]
public struct CarProperties
{
    public CarController car;
    public int price;
}