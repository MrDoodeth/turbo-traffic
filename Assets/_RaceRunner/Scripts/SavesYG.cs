namespace YG
{
    public partial class SavesYG
    {
        // Ваши сохранения

        public int currentCarIndex;
        public int currentColorIndex;
        public int highScore;
        public bool[,] isFreeVariantCars = new bool[20, 5];
        public int moneyAmount = 0;
        public int qualityIndex = 1;
        public bool isSoundActive = true;
        public bool isMusicActive = true;

        public SavesYG()
        {
            // Вы можете выполнить какие то действия при загрузке сохранений
            isFreeVariantCars[0, 0] = true;
        }
    }
}