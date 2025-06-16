using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField] private string gameplayMapName;

    private InputActionMap gameplayInputMap;

    [SerializeField] private InputReader inputReader;

    void Start()
    {


        gameplayInputMap = inputAsset.FindActionMap(gameplayMapName);
        
    }
    
    public void SetGameplayInputEnabled(bool active)
    {
        if (active)
        {
            gameplayInputMap.Enable();
        }
        else
        {
            gameplayInputMap.Disable();
        }
    }

    public void SetGameplayInputDisabled(bool disabled)
    {
        SetGameplayInputEnabled(!disabled);
    }
}
