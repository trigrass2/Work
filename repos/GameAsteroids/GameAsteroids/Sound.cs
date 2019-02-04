using System.Media;

namespace GameAsteroids
{
    /// <summary>
    /// Класс для звукового сопровождения игры
    /// </summary>
    public static class Sound
    {
        private static SoundPlayer _soundShot = new SoundPlayer(@"resources\sounds\Shot.wav");
        private static SoundPlayer _soundCrash = new SoundPlayer(@"resources\sounds\crush.wav");
        private static SoundPlayer _soundHittingAsteroid = new SoundPlayer(@"resources\sounds\hittingAnAsteroid.wav");

        /// <summary>
        /// Воспроизводит звук выстрела
        /// </summary>
        public static void ShipShot()
        {
            _soundShot.Play();
        }

        /// <summary>
        /// Возпроизвоит звук столкновения
        /// </summary>
        public static void ShipDamaged()
        {
            _soundCrash.Play();
        }

        /// <summary>
        /// Воспроизводит звук попадания по астероиду
        /// </summary>
        public static void HittingAnAsteroid()
        {
            _soundHittingAsteroid.Play();
        }
    }
}
