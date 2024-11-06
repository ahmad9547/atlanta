using Avatars.PersonMovement.MoveSettings;
using Avatars.PersonMovement.States;
using System.Collections.Generic;
using System.Linq;

namespace Avatars.PersonMovement
{
    public sealed class PlayerMovementStateMachine
    {
        public MovementState CurrentState { get; private set; }

        private readonly PlayerMovement _playerMovement;
        private readonly MoveSettingHolder _moveSettingsHolder;

        private List<MovementState> _movementStates;

        public PlayerMovementStateMachine(PlayerMovement playerMovement, MoveSettingHolder moveSettingsHolder)
        {
            _playerMovement = playerMovement;
            _moveSettingsHolder = moveSettingsHolder;

            InitializeMovementStates();

            CurrentState = _movementStates[0];
            CurrentState.Enter();
        }

        public void SwitchState<T>() where T : MovementState
        {
            if (!_movementStates.Exists(state => state is T))
            {
                return;
            }

            if (CurrentState.GetType() == typeof(T))
            {
                return;
            }

            CurrentState.Exit();

            CurrentState = _movementStates.First(state => state is T);

            CurrentState.Enter();
        }        

        private void InitializeMovementStates()
        {
            _movementStates = new List<MovementState>();

            _movementStates.Add(new IdleState(_playerMovement));
            _movementStates.Add(new HoldState(_playerMovement));
            _movementStates.Add(new WalkState(_playerMovement, _moveSettingsHolder.WalkSettings));

            if (_moveSettingsHolder.RunningIsEnabled)
            {
                _movementStates.Add(new RunState(_playerMovement, _moveSettingsHolder.RunSettings));
            }

            if (_moveSettingsHolder.JumpingIsEnabled)
            {
                _movementStates.Add(new JumpState(_playerMovement, _moveSettingsHolder.JumpSettings));
            }

            if (_moveSettingsHolder.FlyingIsEnabled)
            {
                _movementStates.Add(new FlyState(_playerMovement, _moveSettingsHolder.FlySettings));
            }
        }
    }
}
