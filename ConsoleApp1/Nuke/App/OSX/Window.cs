
namespace Nuke.App.OSX;

using System.Runtime.InteropServices;
using static CoreFoundation;

public class Window
{
    private static Lazy<nint> _NSWindowClass = new Lazy<nint>(() => objc_getClass("NSWindow"));
    protected static nint NSWindowClass => _NSWindowClass.Value;

    private static Lazy<nint> _initWithContentRectSel = new Lazy<nint>(() => SEL("initWithContentRect:styleMask:backing:defer:"));
    protected static nint InitWithContentRectSel => _initWithContentRectSel.Value;

    [DllImport(CoreFoundationLib, EntryPoint = "objc_msgSend")]
    private static extern IntPtr objc_msgSend_retIntPtr(IntPtr obj, IntPtr sel, NSRect rect, uint nswindowstylemask, uint nsbackingstoretype, bool defer);

    [DllImport(CoreFoundationLib, EntryPoint = "objc_msgSend")]
    private static extern IntPtr objc_msgSend_retIntPtr(IntPtr obj, IntPtr sel, NSPoint point);

    [DllImport(CoreFoundationLib, EntryPoint = "objc_msgSend")]
    private static extern IntPtr objc_msgSend_retIntPtr(IntPtr obj, IntPtr sel, NSSize point);

    [DllImport(CoreFoundationLib, EntryPoint = "objc_msgSend")]
    private static extern IntPtr objc_msgSend_retIntPtr(IntPtr obj, IntPtr sel, bool b);

    [DllImport(CoreFoundationLib, EntryPoint = "objc_msgSend")]
    private static extern IntPtr objc_msgSend_retIntPtr(IntPtr obj, IntPtr sel, IntPtr id1);

    protected nint InitWithContentRectStyleMaskBackingDefer(nint id, NSRect rect, uint nswindowstylemask, uint nsbackingstoretype, bool defer)
        => objc_msgSend_retIntPtr(id, InitWithContentRectSel, rect, nswindowstylemask, nsbackingstoretype, defer);

    private nint NSWindowRef;

    public Window(string? title = null)
    {
        // Add some initialization properties,
        // and expose properties common for
        // MacOS, Window, Linux windows, etc.

        this.NSWindowRef = InitWithContentRectStyleMaskBackingDefer(
            Alloc(NSWindowClass),
            rect: new NSRect {
                origin = new NSPoint {
                    x = 200,
                    y = 200
                },
                size = new NSSize {
                    width = 600,
                    height = 400
                }
            },
            nswindowstylemask: 15,
            nsbackingstoretype: 2,
            defer: false
        );

        IntPtr setReleasedWhenClosedSel = SEL("setReleasedWhenClosed:");
        objc_msgSend_retIntPtr(this.NSWindowRef, setReleasedWhenClosedSel, false);

        IntPtr setAcceptsMouseMovedEventsSel = SEL("setAcceptsMouseMovedEvents:");
        objc_msgSend_retIntPtr(this.NSWindowRef, setAcceptsMouseMovedEventsSel, true);

        IntPtr makeKeyAndOrderFrontSel = SEL("makeKeyAndOrderFront:");
        objc_msgSend_retIntPtr(this.NSWindowRef, makeKeyAndOrderFrontSel, nint.Zero);

        if (title != null)
        {
            IntPtr setTitleSel = SEL("setTitle:");
            IntPtr titleStr = CFString(title);
            objc_msgSend_retIntPtr(this.NSWindowRef, setTitleSel, titleStr);
            CFRelease(titleStr);
        }
    }
}