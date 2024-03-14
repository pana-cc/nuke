using System.Runtime.InteropServices;

namespace Nuke.App.OSX;

public static class CoreFoundation
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NSSize
    {
        public double width;
        public double height;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NSPoint
    {
        public double x;

        public double y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NSRect
    {
        public NSPoint origin;
        public NSSize size;
    }

    private static Lazy<nint> _allocSel = new Lazy<nint>(() => SEL("alloc"));
    private static Lazy<nint> _initSel = new Lazy<nint>(() => SEL("init"));

    public static nint AllocSel => _allocSel.Value;
    public static nint InitSel => _initSel.Value;

    public const string CoreFoundationLib = "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";
    public const string AppKitLib = "/System/Library/Frameworks/AppKit.framework/AppKit";

    [DllImport(AppKitLib)]
    public static extern nint NSSelectorFromString(nint selector);

    [DllImport(CoreFoundationLib, EntryPoint = "objc_msgSend")]
    public static extern nint objc_msgSend_retIntPtr(nint obj, nint sel);

    [DllImport(CoreFoundationLib, EntryPoint = "objc_msgSend")]
    public static extern nint objc_msgSend_retIntPtr(IntPtr obj, nint sel, int int1);

    [DllImport(CoreFoundationLib, EntryPoint = "objc_msgSend")]
    public static extern bool objc_msgSend_retBool(IntPtr obj, nint sel, int int1);

    [DllImport(CoreFoundationLib, EntryPoint = "objc_msgSend")]
    public static extern nint objc_msgSend_retIntPtr(IntPtr obj, nint sel, nint ptr1);

    [DllImport(CoreFoundationLib, EntryPoint = "objc_msgSend")]
    public static extern nint objc_msgSend_retIntPtr(IntPtr obj, nint sel, bool bool1);

    [DllImport(CoreFoundationLib, EntryPoint = "objc_msgSend")]
    public static extern void objc_msgSend(IntPtr obj, nint sel);

    [DllImport(CoreFoundationLib, EntryPoint = "objc_msgSend")]
    public static extern void objc_msgSend(IntPtr obj, nint sel, bool bool1);

    [DllImport(CoreFoundationLib)]
    public static extern bool class_addProtocol(IntPtr objcclass, nint name);

    [DllImport(CoreFoundationLib)]
    public static extern nint objc_getProtocol(string name);

    [DllImport(CoreFoundationLib)]
    public static extern nint objc_allocateClassPair(IntPtr superclass, nint name, int extrabytes);

    [DllImport(CoreFoundationLib)]
    public static extern nint sel_registerName(string cfstring);

    public static nint SEL(string name)
    {
        nint cfstrSelector = CFString(name);
        nint selector = NSSelectorFromString(cfstrSelector);
        CFRelease(cfstrSelector);
        return selector;
    }

    public static nint AllocInit(nint cls) => objc_msgSend_retIntPtr(objc_msgSend_retIntPtr(cls, AllocSel), InitSel);
    
    public static nint NEW(string nsclassname) => AllocInit(objc_getClass(nsclassname));

    public static nint Alloc(nint cls) => objc_msgSend_retIntPtr(cls, AllocSel);
    
    [DllImport(CoreFoundationLib)]
    private static extern nint CFStringCreateWithCString(nint allocator, string value, uint encoding);

    private const uint kCFStringEncodingUTF8 = 0x08000100;

    [DllImport(CoreFoundationLib)]
    public static extern void CFRelease(nint obj);

    [DllImport(CoreFoundationLib)]
    public static extern nint objc_getClass(string name);

    public static nint CFString(string str) =>
        CFStringCreateWithCString(nint.Zero, str, kCFStringEncodingUTF8);
}
