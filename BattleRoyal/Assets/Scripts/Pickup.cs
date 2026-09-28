using UnityEngine;
using Photon.Pun;
public enum PickupType
{
    Healt,Ammo
}

public class Pickup : MonoBehaviour
{
    public PickupType type;
    public int value;
}
