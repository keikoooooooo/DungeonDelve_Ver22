using UnityEngine;

public class PlayerDataHandle : MonoBehaviour
{
    
    [field: SerializeField] public UpgradeData UpgradeData { get; private set; }
    [field: SerializeField] public PlayerConfiguration PlayerConfig { get; private set; }
    
    public UserData UserData { get; private set; }
    

    
    
}
