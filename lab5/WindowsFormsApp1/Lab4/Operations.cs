namespace WindowsFormsApp1
{
     public static class Operations
    {
        /// <summary>
        ///  Окончательный результат, находящийся в буфере A-D, и есть    почти готовый хэш.Выводя «слова» из этого буфера в обратном  порядке, мы получим готовый хэш.Т.е.md5hash=DCBA.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToDigitsString(uint[] data)
        {
            
            return  reverse(data[0]).ToString("X8") +
                    reverse(data[1]).ToString("X8") +
                    reverse(data[2]).ToString("X8") +
                    reverse(data[3]).ToString("X8");
        }


        /// <summary>
        /// Функция циклически сдвигает входной 32 битный блок влево на s битов
        /// </summary>
        /// <param name="integer"> - входной 32 битный блок</param>
        /// <param name="shift"> - количество битов, на которое мы сдвигаем блок</param>
        /// <returns></returns>
        public static uint lshift(uint integer, ushort shift)
        {
            return ((integer >> 32 - shift) | (integer << shift));
        }


        public static uint reverse(uint integer)
        {
            return (((integer & 0x000000ff) << 24) |
                        (integer >> 24) |
                    ((integer & 0x00ff0000) >> 8) |
                    ((integer & 0x0000ff00) << 8));
        }
    }
}
