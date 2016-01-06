using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gundam00
{
    static class GNDriveDefine
    {
        static public string electricResourceName = "ElectricCharge";
        static public string greenParticleResourceName = "GNGreenParticle";
        static public string redParticleResourceName = "GNRedParticle";
        static public float timeSecond = 1.0f;
        static public float ForcePerParticle = 1.0f;
        static public float expoEaseIn(float percent)
        {
            return percent == 0 ? 0 : (float)Math.Pow(2, 10 * (percent / 1 - 1)) - 1 * 0.001f;
        }
        static public float expoEaseOut(float percent)
        {
            return percent == 1 ? 1 : (-(float)Math.Pow(2, -10 * percent / 1) + 1);
        }
    }
}
