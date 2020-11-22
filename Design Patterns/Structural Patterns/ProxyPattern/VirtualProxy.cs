using System;

namespace Design_Patterns.Structural_Patterns.ProxyPattern
{
    /*
     * Virtual Proxies wrap expensive objects and load them on-demand.
     * Sometimes we don;t immediately need all functionalities that an
     * object offers, especially if it is memory/time-consuming.
     * Calling objects only when needed might increase performance quite a bit.
     *
     * One example of a virtual proxy is loading images. Let's imagine that we're
     * building a file manager. Like any other file manager, this one should be able
     * to display images in a folder that a user decides to open. If we assume there
     * exists a class, ImageViewer, responsible for loading and displaying images
     *   - we might implement our file manager by using this class directly. This
     *     kind of approach seems logical and straight-forward but it contains a
     *     subtle problem.
     * If we implement the file manager as described above, we're going to be loading
     * every time they appear in the folder. If the user only wishes to see the name
     * or size of an image, this kind of approach would still load the entire image
     * into memory. Since loading and displaying images are expensive operations,
     * this can cause performance issues.
     *
     * A better solution would e to display images only when actually needed.
     * In this sense, we can use a proxy to wrap the existing ImageViewer object.
     * This way, the actual image viewer will only get called when the image
     * needs to be rendered. All other operations (such as obtaining the image name,
     * size, date pf creating, etx.) don't require the actual image and can therefore
     * be obtained through a much lighter proxy object instead.
     */
    
    // Client-side code simulation
    public class VirtualProxy
    {
        // In the code below, we are creating six different image viewers.
        // First, three of them are the concrete image viewers that 
        // automatically load images on creation. The last three images
        // don't load any images into memory on creation.
        public static void Test()
        {
            // all 3 concrete viewers load the images the moment they are 
            // created, thus leading to a very expensive operation
            IMageViewer flowers = new ConcreteImageViewer("./photos/flowers.png");
            IMageViewer trees = new ConcreteImageViewer("./photos/trees.png");
            IMageViewer grass = new ConcreteImageViewer("./photos/grass.png");
            
            // only with the last line will the first proxy viewer start
            // loading the image. Compared to the concrete viewers, 
            // performance benefits are obvious:
            IMageViewer sky = new ImageViewerProxy("./photos/sky.png");
            IMageViewer sun = new ImageViewerProxy("./photos/sun.png");
            IMageViewer clouds = new ImageViewerProxy("./photos/clouds.png");
                        
            sky.DisplayImage();
        }
    }

    public interface IMageViewer
    {
        public void DisplayImage();
    }
    
    // pseudo image functionality
    public class Image
    {
        private string Path;

        private Image(string path)
        {
            Path = path;
        }

        public static Image Load(string path)
        {    
            return new Image(path);
        }

        public void Display()
        {
            Console.WriteLine($"Display image at {Path}");
        }
    }
    
    // concrete impl
    public class ConcreteImageViewer : IMageViewer
    {
        private Image Image;

        public ConcreteImageViewer(string path)
        {
            // Costly operation, takes long time to load an image.
            // We'll wrap the constructor cal which causes this loading in a proxy.
            // We'll make sure to only load an image (calling the constructor) when a client
            // requests explicitly the displayImage() method
            Image = Image.Load(path);
        }

        public void DisplayImage()
        {
            // Light-weigh operation
            Image.Display();
        }
    }
    
    // Implement our lightweight image viewer proxy.
    // This object will call the concrete image viewer only
    // when needed, i.e. when the client calls the DisplayImage() method.
    // Until then, no images will be loaded or processed, which will make 
    // our program much more efficient
    public class ImageViewerProxy : IMageViewer
    {
        private string Path;
        private IMageViewer Viewer;

        public ImageViewerProxy(string path)
        {
            Path = path;
        }

        public void DisplayImage()
        {
            Viewer = new ConcreteImageViewer(Path);
            Viewer.DisplayImage();
        }
    }
    
}