using UnityEngine;

[CreateAssetMenu (menuName = "Physics Constants")]
public class SO_PhysicsConstants : ScriptableObject
{
    [SerializeField] public float GravityStrength;
    [SerializeField] public float GravityRangePerMass;
    [SerializeField] public float StarSizePerMass;
}
