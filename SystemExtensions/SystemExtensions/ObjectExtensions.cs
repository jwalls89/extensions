namespace Walls.Julian.System.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull<T>(this T @object) where T : class
        {
            return @object == null;
        }

        public static bool IsNotNull<T>(this T @object) where T : class
        {
            return !@object.IsNull();
        }
    }
}
