namespace PhotoAlbum.Logging
{
    public static class MyStaticClass
    {
        public static void MyStaticFunction()
        {
            // Since the keyword "this" is not available in a static context we need to use a ruse.
            // Types are objects, a static class may not have an instance but it has a type !
            typeof(MyStaticClass).Info("For your information, this is happening right now");
            typeof(MyStaticClass).Trace("Here is some raw data I received at this point : { 'name': 'yolo' }");
            typeof(MyStaticClass).Warn("I don't like this, something weird happened ...");
        }
    }
}