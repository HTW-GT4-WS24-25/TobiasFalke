public class HealthBoostEffect : IItemEffect
{
    public void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.ChangeHealth(50);
    }
}

public class HealthBoomEffect : IItemEffect
{
    public void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.ChangeHealth(100);
    }
}

public class SpecialBoostEffect : IItemEffect
{
    public void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.ChangeSpecial(25);
    }
}

public class SpecialBoomEffect : IItemEffect
{
    public void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.ChangeSpecial(50);
    }
}

public class SpeedBoostEffect : IItemEffect
{
    public void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.ChangeSpeedMultiplier(50);
    }
}

public class SpeedBoomEffect : IItemEffect
{
    public void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.ChangeSpeedMultiplier(100);
    }
}

public class JumpBoomEffect : IItemEffect
{
    public void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.ChangeJumpDuration(50);
    }
}
public class JumpBoostEffect : IItemEffect
{
    public void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.ChangeJumpDuration(100);
    }
}
public class ScoreBoostEffect : IItemEffect
{
    public void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.ChangeSpecial(100);
    }
}
public class ScoreBoomEffect : IItemEffect
{
    public void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.ChangeSpecial(500);
    }
}
public class ScoreMultiplierBoostEffect : IItemEffect
{
    public void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.ChangeScoreMultiplier(30);
    }
}