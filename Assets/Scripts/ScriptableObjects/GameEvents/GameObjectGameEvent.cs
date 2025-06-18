using UnityEngine;
using FinishOne.GeneralUtilities;

[CreateAssetMenu(fileName = nameof(GameObjectGameEvent), menuName = "GameEvents/"+nameof(GameObjectGameEvent), order=0)]
public class GameObjectGameEvent : BaseGameEvent<GameObject> {}