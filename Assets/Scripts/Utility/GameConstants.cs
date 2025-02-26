namespace Utility
{
    public static class GameConstants
    {
        public static class Scenes
        {
            public const string MainMenu = "main_menu_scene";
            public const string Level = "level_scene";
            public const string GameOver = "game_over_scene";
        }

        public static class Audio
        {
            public const string MainMenuBGM = "bgm_main_menu_scene";
            public const string LevelBGM = "bgm_level_scene";
            public const string GameOverBGM = "bgm_game_over_scene";
            public const string DriveBGS = "bgs_drive";
            public const string GrindBGS = "bgs_grind";
            public const string MenuClickSFX = "sfx_menu_click";
            public const string JumpActionSFX = "sfx_jump_action";
            public const string GroundLandingSFX = "sfx_landing_ground";
            public const string RailLandingSFX = "sfx_landing_rail";
            public const string EndGrindSFX = "sfx_grind_end";
            public const string TrickActionSFX = "sfx_trick_action";
            public const string SpecialBarFullSFX = "sfx_special_bar_full";
            public const string SpecialActionSFX = "sfx_special_action";
            public const string WallCollisionSFX = "sfx_collision_wall";
            public const string RailCollisionSFX = "sfx_collision_rail";
            public const string HoleCollisionSFX = "sfx_collision_hole";
            public const string PickupCollision1SFX = "sfx_collision_pickup_1";
            public const string PickupCollision2SFX = "sfx_collision_pickup_2";
            public const string PickupCollision3SFX = "sfx_collision_pickup_3";
            public const string PickupCollision4SFX = "sfx_collision_pickup_4";
            public const string PickupCollision5SFX = "sfx_collision_pickup_5";
            public const string GameOverSFX = "sfx_game_over";
        }
        
        public static class Animations
        {
            public const string trickAction = "Flip";
            public const string invincibility = "isInvincible";
            public const string gameOver = "isDead";
        }

        public static class Sprites
        {
            public const string ProfileImagesPath = "Assets/Sprites/UI/Profile Pictures/";
            public const string ProfileImageFileBase = "em_outline_shadow_";
        }
    }
}