namespace Avatars.PersonMovement
{
    public interface IPlayerAnimator
    {
        void SetMovementParameters(float moveX, float moveZ);

        void SetJumpParameter();

        void SetRunParameter(bool run);

        void SetFlyParameter(bool fly);

        void SetSeatingOnBenchParameter(bool state);

        void SetSeatingOnTableChairParameter(bool state);

        void SetSeatingOnAdminChairParameter(bool state);

        void SetSpeakerParameter(bool state);

        void ResetAnimator();
    }
}