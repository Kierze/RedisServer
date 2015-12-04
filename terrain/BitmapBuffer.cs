using System.Drawing;
using System.Drawing.Imaging;

namespace terrain
{
    internal unsafe class BitmapBuffer
    {
        private Bitmap bmp;
        private int w;
        private int h;
        private int s;
        private byte* ptr;
        private BitmapData dat;

        public BitmapBuffer(Bitmap bmp)
        {
            this.bmp = bmp;
            this.w = bmp.Width;
            this.h = bmp.Height;
        }

        public void Lock()
        {
            dat = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
            s = dat.Stride;
            ptr = (byte*)dat.Scan0;
        }

        public void Unlock()
        {
            bmp.UnlockBits(dat);
        }

        public uint this[int x, int y]
        {
            get { return *(uint*)(ptr + x * 4 + y * s); }
            set { *(uint*)(ptr + x * 4 + y * s) = value; }
        }
    }
}
