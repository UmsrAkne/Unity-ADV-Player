namespace Loaders.XmlElementConverters
{
    public static class StringExtension
    {
        /// <summary>
        /// 指定した n 番目の文字を小文字に変換します。
        /// </summary>
        public static string ToLower(this string self, int no = 0)
        {
            if (no > self.Length)
            {
                return self;
            }

            var array = self.ToCharArray();
            var up = char.ToLower(array[no]);
            array[no] = up;
            return new string(array);
        }
    }
}