using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animator_circles
{
    public class Animator
    {
        private Size cSize;
        private BufferedGraphics bg;
        private Graphics _g;
        private List<Circle> circs=new List<Circle>();
        private Thread? t;
        private Graphics g
        {
            get => _g;
            set
            {
                _g = value;
                lock(_g)
                {
                    bg=BufferedGraphicsManager.Current.Allocate(g,Rectangle.Ceiling(_g.VisibleClipBounds));
                }
            }
        }

        public Animator(Size containerSize, Graphics g)
        {
            cSize = containerSize;
            this.g = g;
        }
        public void AddCircle(Point location)
        {
            Circle c=new Circle(cSize, location);
            c.Animate();
            circs.Add(c);
        }
        public void Start()
        {
            if(t==null||!t.IsAlive)
            {
                Thread t = new Thread(() =>
                {
                    Graphics tg;
                    lock (bg)
                    {
                        tg = bg.Graphics;
                    }
                    do
                    {
                        tg.Clear(Color.White);
                        circs.RemoveAll(it => !it.IsAlive);
                        for (int i = 0; i < circs.Count; i++)
                        {
                            circs[i].Paint(tg);
                        }
                        bg.Render(g);
                        //Thread.Sleep(5);
                    } while (true);
                });
                t.IsBackground = true;
                t.Start();
            }
        }
        public void Resize(Size new_size, Graphics g)
        {
            cSize=new Size(new_size.Width,new_size.Height);
            this.g = g;
            lock(circs)
            {
                foreach(var c in circs)
                {
                    c._containerSize = new_size;
                }
            }
        }
    }
}
