using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animator_circles
{
    public class Circle
    {
        private Point _pos;
        private int _friction = 3;
        private int _radius;
        private int _diameter;
        private int _dx;
        private int _dy;
        public Size _containerSize;
        private Thread? t;
        public bool IsAlive = true;
        public Color Color { get; set; }
        public Circle(Size contSize, Point p)
        {
            Random r = new Random();
            _radius = r.Next(2,70);
            _pos=new Point(p.X-_radius,p.Y-_radius);
            _diameter = _radius * 2;
            Color = Color.FromArgb(r.Next(256),r.Next(256),r.Next(256));
            _containerSize = contSize;
            _dx = r.Next(-50, 50);
            _dy = r.Next(-50,50);
        }
        public void Paint(Graphics g)
        {
            var b = new SolidBrush(Color);
            g.FillEllipse(b,_pos.X, _pos.Y, _diameter, _diameter);

        }

        private bool IsInX()
        {
            if((_pos.X<_containerSize.Width-_diameter-Math.Abs(_dx))&&(_pos.X>Math.Abs(_dx))) return true;
            return false;
        }
        private bool IsInY()
        {
            if ((_pos.Y > Math.Abs(_dy)) && (_pos.Y < _containerSize.Height - _diameter-Math.Abs(_dy))) return true;
            return false;
        }
        private void ChageXSpeed()
        {
            _dx = -_dx;
            if(_dx>0)
            {
                if(_dx>_friction)
                {
                    _dx -= _friction;
                }
                else
                {
                    if(_dx>1)
                    {
                        _dx -= 1;
                    }else
                    {
                        _dx = 1;
                    }
                }
            }else
            {
                if(_dx<0)
                {
                    if(-_dx>_friction)
                    {
                        _dx += _friction;
                    }
                    else
                    {
                        if(_dx<-1)
                        {
                            _dx += 1;
                        }
                        else
                        {
                            _dx = -1;
                        }
                    }
                }
            }
        }
        private void ChangeYSPeed()
        {
            _dy=-_dy;
            if (_dy > 0)
            {
                if (_dy > _friction)
                {
                    _dy -= _friction;
                }
                else
                {
                    if(_dy>1)
                    {
                        _dy -= 1;
                    }else
                    {
                        _dy = 1;
                    }
                }
            }
            if (_dy < 0)
            {
                if (-_dy > _friction)
                {
                    _dy += _friction;
                }
                else
                {
                    if(_dy<-1)
                    {
                        _dy += 1;
                    }
                    else
                    {
                        _dy = -1;
                    }
                }
            }
            
        }

        public void Move()
        {
            if (IsInX())
            {
                _pos.X += _dx;
            }
            else
            {
                ChageXSpeed();
                //_dx=-_dx;
                _pos.X += _dx;
            }
            if (IsInY())
            {
                _pos.Y += _dy;
            }
            else
            {
                ChangeYSPeed();
                //_dy=-_dy;
                _pos.Y += _dy;
            }
            if((Math.Abs(_dx)+Math.Abs(_dy))<=2) IsAlive = false;
        }

        public void Animate()
        {
            if(t?.IsAlive ?? true)
            {
                t = new Thread(() =>
                  {
                      while(IsAlive)
                      {
                          Thread.Sleep(30);
                          Move();
                      }
                  });
                t.IsBackground = true;
                t.Start();
            }
        }
    }
}
