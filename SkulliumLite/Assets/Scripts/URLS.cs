public static class URLS
{
    public static string BaseUrl()
    {
#if DEBUG
        return "http://servoskull.local:5000/";
#endif
#if !DEBUG
        return "https://servoskull.local:5001/";
#endif
    }
}
