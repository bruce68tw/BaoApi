using Base.Services;

namespace BaoApi.Services
{
    public static class _Xp
    {
        //same to bao_app(flutter)
        public static string AesKey = "YourAesKey";

        public static string GetAesKey()
        {
            return _Str.PreZero(16, AesKey, true);
        }

    } //class
}
