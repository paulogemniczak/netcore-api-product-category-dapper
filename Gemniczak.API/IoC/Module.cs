namespace Gemniczak.API.IoC
{
    /// <summary>
    /// 
    /// </summary>
    public static class Module
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetSingleTypes()
        {
            var result = new List<Type>
            {
                typeof(Validators.CategoryValidator),
                typeof(Validators.ProductValidator),
            };

            return result;
        }
    }
}
