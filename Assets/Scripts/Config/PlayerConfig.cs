using UnityEngine;

namespace Config
{
    public class PlayerConfig : MonoBehaviour
    {
        [SerializeField] private float maxHealthPoints = 100;
        [SerializeField] private float maxSpecialPoints = 100;
        [SerializeField] private float baseSpeed = 5f;
        [SerializeField] private float baseJumpHeight = 2f;
        [SerializeField] private float baseJumpDuration = 1f;
        [SerializeField] private float baseGrindActionScore = 1f;
        [SerializeField] private float trickActionScore = 1f;
        [SerializeField] private float trickActionDuration = 1f;
        [SerializeField] private float specialActionDuration = 8f;
        [SerializeField] private float invincibilityDuration = 1f;
        
        public static float MaxHealthPoints;
        public static float MaxSpecialPoints;
        public static float BaseSpeed;
        public static float JumpHeight;
        public static float JumpDuration;
        public static float GrindActionScore;
        public static float TrickActionScore;
        public static float TrickActionDuration;
        public static float SpecialActionDuration;
        public static float InvincibilityDuration;
        
        private void Awake()
        {
            maxHealthPoints = MaxHealthPoints;
            maxSpecialPoints = MaxSpecialPoints;
            baseSpeed = BaseSpeed;
            baseJumpHeight = JumpHeight;
            baseJumpDuration = JumpDuration;
            baseGrindActionScore = GrindActionScore;
            trickActionScore = TrickActionScore;
            trickActionDuration = TrickActionDuration;
            specialActionDuration = SpecialActionDuration;
            invincibilityDuration = InvincibilityDuration;
        }
    }
}