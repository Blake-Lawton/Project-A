namespace Data.Types
{
    public enum Team
    {
        Team1 = 1,
        Team2 = 2,
        Team3 = 3 
    }

    public enum TargetType
    {
        Enemy,
        Ally,
        Both,
        None
    }

    public enum Joint
    {
        RightHand,
        LeftHand,
        SwordTip
    }

    public enum StatusVisuals
    {
        IceSlow,
    }

    public enum CastResult
    {
        Success,
        NoTarget,
        CantTargetSelf,
        CantTargetEnemy,
        CantTargetAlly,
        OutOfRange,
        OnCooldown,
        CrowdControlled,
        Moving,
        OnGlobalCd,
        IsCastingAlready,
        NotInFront,
        CantInterrupt
    }

    public enum GreatSwordSfx
    {
        StrikeSwing,
        StrikeImpact
    }

    public enum RotationState
    {
        RegularInput,
        LookAtTarget,
        LockRotation,
    }

    public enum MovementState
    {
        NoInput,
        Input,
        InMovementAbility,
    }

    public enum AbilityType
    {
        Damage,
        Heal,
        Shield,
    }
}
