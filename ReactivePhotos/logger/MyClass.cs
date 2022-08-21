using System;
using PhotoAlbum.Logging;

namespace PhotoAlbum.Logging
{
    public class MyClass
    {
        public void MyMethod()
        {
            // this.Info, this.Trace and this.Warn can be used at any point in the code.
            this.Info("For your information, this is happening right now");
            this.Trace("Here is some raw data I received at this point : { 'name': 'yolo' }");
            this.Warn("I don't like this, something weird happened ...");

            // The message string is optional.
            // This is useful to track a specific point of the code without using breakpoints.
            this.Trace();
        }

        public void ThrowException()
        {
            throw new Exception("Nope");
        }
    }
}
