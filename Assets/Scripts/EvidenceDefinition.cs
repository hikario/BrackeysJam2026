using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EvidenceDefinition", menuName = "Scriptable Objects/EvidenceDefinition")]
public class EvidenceDefinition : ScriptableObject
{
    [SerializeField] public List<Evidence> evidence = new List<Evidence>();
}
