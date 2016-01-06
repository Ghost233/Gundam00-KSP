using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gundam00
{
    public class GNSystemControl
    {
        static public GNSystemControl _sharedGNSystemControl = null;
        static public GNSystemControl getInstance()
        {
            if (_sharedGNSystemControl == null)
            {
                _sharedGNSystemControl = new GNSystemControl();
            }
            return _sharedGNSystemControl;
        }

        public GNSystemControl()
        {

        }

    }
}
