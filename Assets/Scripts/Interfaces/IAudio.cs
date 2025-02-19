
namespace Interfaces
{
        public interface IAudio
        {
                void PlayTrack(string fileName);
                void StopTrack();
                void PlayBackgroundTrack(string fileName);
                void StopBackgroundTrack();
                void PlaySound(string fileName);
        }
}