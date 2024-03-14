
// See for reference: https://github.com/jimon/osx_app_in_plain_c/blob/master/main.c
// It has the app and window initialization and a low-level message loop,
// only the drawing is GL-ish, we'd aim for CoreGraphics 2D or AppKit - NSView and the like.

using Nuke.App.OSX;

new App().Main(() => {
    var window = new Window("My C# MacOS Window");
});
