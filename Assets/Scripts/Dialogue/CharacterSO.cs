using UnityEngine;

[CreateAssetMenu(fileName = nameof(CharacterSO), menuName = "FinishOne/" + nameof(CharacterSO))]
public class CharacterSO : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }

    [field: SerializeField] public AudioClip VoiceClip { get; private set; }

    [MinMaxSlider(-3, 3)]
    [SerializeField] private Vector2 pitchRange;

    public Vector2 PitchRange => pitchRange;


    [Range(1, 4)]
    [SerializeField] private int charactersPerVoiceClip;

    public int VoicePace => charactersPerVoiceClip;
}
