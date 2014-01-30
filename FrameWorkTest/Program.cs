using MJS.Framework.View.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ViewObject vo = new ViewObject();

            vo.SetValue("/root/name", "Mads Jakob Steffansen");
            //vo.SetValue("/root/@id", Guid.NewGuid().ToString());
            vo.SetValue("/root/age", "39");
            vo.SetValue("/root/children/item/name", "Aksel Steffansen");
            Console.WriteLine(vo);
            Console.Read();
        }
    }
}
