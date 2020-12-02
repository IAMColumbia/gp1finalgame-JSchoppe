using UnityEngine;

namespace SkirmishWars.UnityEditor
{
    /// <summary>
    /// Inspector wrapper for damage tables.
    /// </summary>
    public sealed class DamageTableInstance : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("The base damage trades for full health units.")]
        [SerializeField] private DamageTableEntry[] entries = null;
        #endregion
        #region Retrieval Method
        public DamageTable GetInstance()
        {
            Destroy(this);
            return new DamageTable(entries);
        }
        #endregion
    }
}
