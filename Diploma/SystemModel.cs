using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Diploma
{
    [Bindable(true)]
    public class SystemModel
    {


        public double yOffset;
        public double xOffset;

        private static SystemModel model;
        public SystemModel()
        {
            if (model != null)
                throw new Exception("Only one SystemModel should exists");
        }
        public static SystemModel getInstance()
        {
            if (model == null)
                model = new SystemModel();
            return model;
        }
    }
}
