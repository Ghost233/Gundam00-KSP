using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Gundam00
{
    public class GNTDBlanket : PartModule
    {
        //时间参数
        private float lastUpdateTime = 0.0f;

        //GN红色粒子转换速率
        [KSPField]
        private float GNRedParticleConsumeCountPerSecond = 950.0f;

        //GN绿色粒子转换效率
        [KSPField]
        private float GNGreenConvertEfficiency = 1.1f;

        //反应物恢复效率
        [KSPField]
        private float restoreGenerateEfficiencyPerSecond = 2.0f;

        //过滤器当前状态
        [KSPField(guiName = "TD Blanke Status", guiActive = true)]
        private string ES = "Activated";

        public override void OnStart(PartModule.StartState state)
        {
            lastUpdateTime = Time.time;
            this.part.force_activate();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            float time = Time.time;
            float timeInterval = time - lastUpdateTime;
            float percentSecond = timeInterval / GNDriveDefine.timeSecond;

            this.restore(percentSecond * restoreGenerateEfficiencyPerSecond);

            float GNRedParticleGen = this.GNRedParticleConsumeCountPerSecond * percentSecond;

            float GNGreenParticleGen = -this.part.RequestResource(GNDriveDefine.redParticleResourceName, GNRedParticleGen);

            float responseGNGreenParticleGen = this.part.RequestResource(GNDriveDefine.greenParticleResourceName, GNGreenParticleGen * GNGreenConvertEfficiency);

            Debug.Log("ClassID:" + this.ClassID + "   UnityID" + this.GetInstanceID() + "   GNRedParticleGen " + GNRedParticleGen + "   GNGreenParticleGen:" + GNGreenParticleGen + "   responseGNGreenParticleGen" + responseGNGreenParticleGen);

            lastUpdateTime = time;
        }
        
        private void restore(float percent)
        {
            List<GNDrive> GNDriveList = new List<GNDrive>();
            foreach (Part p in this.vessel.Parts)
            {
                foreach (PartModule m in p.Modules)
                {
                    if (m.moduleName == "GNDrive")
                    {
                        GNDrive drive = (GNDrive)m;
                        GNDriveList.Add(drive);
                    }
                }
            }

            int count = GNDriveList.Count;
            float restoneCount = percent;

            bool flag = true;
            while (flag)
            {
                flag = false;

                float currectRestoneCount = restoneCount;

                foreach (GNDrive drive in GNDriveList)
                {
                    float restoneLast = 0;
                    flag |= drive.restoreGenerateEfficiency(currectRestoneCount / count, out restoneLast);

                    restoneCount -= restoneLast;
                }
            }
        }
    }
}
