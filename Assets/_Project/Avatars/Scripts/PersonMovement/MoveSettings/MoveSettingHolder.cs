using UnityEngine;

namespace Avatars.PersonMovement.MoveSettings
{
    [CreateAssetMenu(fileName = "MoveSettingHolder", menuName = "ScriptableObjects/MoveSettings/MoveSettingsHolder")]
    public sealed class MoveSettingHolder : ScriptableObject
    {
        [SerializeField] private WalkSettings _walkSettings;

        [SerializeField] private bool _runningIsEnabled;
        [SerializeField] private RunSettings _runSettings;

        [SerializeField] private bool _jumpingIsEnabled;
        [SerializeField] private JumpSettings _jumpSettings;

        [SerializeField] private bool _flyingIsEnabled;
        [SerializeField] private FlySettings _flySettings;

        public WalkSettings WalkSettings => _walkSettings;

        public bool RunningIsEnabled => _runningIsEnabled;

        public RunSettings RunSettings => _runSettings;

        public bool JumpingIsEnabled => _jumpingIsEnabled;

        public JumpSettings JumpSettings => _jumpSettings;

        public bool FlyingIsEnabled => _flyingIsEnabled;

        public FlySettings FlySettings => _flySettings;
    }
}
