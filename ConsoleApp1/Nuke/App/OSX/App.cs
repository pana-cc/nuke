using System.Diagnostics;
using System.Runtime.InteropServices;

using static Nuke.App.OSX.CoreFoundation;

[assembly: System.Reflection.Metadata.MetadataUpdateHandler(typeof(Nuke.App.OSX.App))]

namespace Nuke.App.OSX;

public class App
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool AppCallback(IntPtr self, IntPtr sel, IntPtr sender);

    [DllImport(CoreFoundationLib, EntryPoint = "class_addMethod")]
    private static extern bool class_addMethod_retUint(IntPtr objcclass, IntPtr name, [MarshalAs(UnmanagedType.FunctionPtr)]AppCallback fun, [MarshalAs(UnmanagedType.LPStr)] string types);

    private nint NSAppRef;

    public static App? Instance { get; private set; }

    private static bool applicationShouldTerminate(IntPtr self, IntPtr sel, IntPtr sender)
    {
        return true;
    }

    public App()
    {
        if (App.Instance != null)
        {
            throw new Exception("There may be only one instance of an App.");
        }

        App.Instance = this;
    }

    public void Main(Action run)
    {
        nint pool = NEW("NSAutoreleasePool");
        this.NSAppRef = objc_msgSend_retIntPtr(objc_getClass("NSApplication"), SEL("sharedApplication"));

        bool activated = objc_msgSend_retBool(this.NSAppRef, SEL("setActivationPolicy:"), 0);
        Debug.Assert(activated);

        IntPtr appDelegateStr = CFString("AppDelegate");
        IntPtr AppDelegateClass = objc_allocateClassPair(objc_getClass("NSObject"), appDelegateStr, 0);
        CFRelease(appDelegateStr);

        bool resultAddProtoc = class_addProtocol(AppDelegateClass, objc_getProtocol("NSApplicationDelegate"));
        Debug.Assert(resultAddProtoc);

        IntPtr applicationShouldTerminateSel = sel_registerName("applicationShouldTerminate:");

        bool resultAddMethod = class_addMethod_retUint(AppDelegateClass, applicationShouldTerminateSel, applicationShouldTerminate, "@:@");
	    Debug.Assert(resultAddMethod);

        nint appDelegate = AllocInit(AppDelegateClass);

        IntPtr autoreleaseSel = SEL("autorelease");
        objc_msgSend_retIntPtr(appDelegate, autoreleaseSel);
        objc_msgSend_retIntPtr(this.NSAppRef, SEL("setDelegate:"), appDelegate);
        objc_msgSend_retIntPtr(this.NSAppRef, SEL("activateIgnoringOtherApps:"), true);

        run();

        objc_msgSend_retIntPtr(this.NSAppRef, SEL("run"));
    }

    private void UpdateApplicationInstance(Type[]? types)
    {
        // Schedule a "Hot Reload" event update throughout the application.
    }

    private void ClearCacheInstance(Type[]? types)
    {
    }

    public static void UpdateApplication(Type[]? types)
    {
        App.Instance?.UpdateApplicationInstance(types);
    }

    public static void ClearCache(Type[]? types)
    {
        App.Instance?.ClearCacheInstance(types);
    }
}
