using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gundam00
{
    public class GNEngine : ModuleEnginesFX, GNControlProtocol
    {
        //时间参数
        private float lastFixedUpdate = 0.0f;

        [KSPField]
        public bool agActivated = false;

        [KSPAction("Toggleag", KSPActionGroup.None, guiName = "Toggle Antigravity")]
        private void Toggleag(KSPActionParam param)
        {
            if (agActivated == true)
            {
                Deactivateag();
            }
            else
            {
                Activateag();
            }

        }

        [KSPEvent(name = "Activateag", guiName = "Activate Antigravity", active = true, guiActive = true)]
        public void Activateag()
        {
            this.part.force_activate();
            agActivated = true;
            Events["Deactivateag"].guiActive = true;
            Events["Activateag"].guiActive = false;

        }

        [KSPEvent(name = "Deactivateag", guiName = "Deactivate Antigravity", active = true, guiActive = false)]
        public void Deactivateag()
        {
            agActivated = false;
            Events["Deactivateag"].guiActive = false;
            Events["Activateag"].guiActive = true;
        }

        [KSPField(guiName = "Mass", guiActive = true)]
        private string massString = "N/a";

        [KSPField(guiName = "Anti Gravity Particle", guiActive = true)]
        private string antiGravityConsumeParticleMassString = "N/a";

        new public void OnStart(StartState state)
        {
            base.OnStart(state);
            lastFixedUpdate = Time.time;
        }

        new public void FixedUpdate()
        {
            base.FixedUpdate();

            float time = Time.time;
            float timeInterval = time - lastFixedUpdate;
            float percentSecond = timeInterval / GNDriveDefine.timeSecond;

            float antiGravityConsumeParticleMass = 0;
            float mass = vessel.GetTotalMass();
            massString = mass.ToString("R");

            if (agActivated == true)
            {
                Vector3 gee = FlightGlobals.getGeeForceAtPosition(this.vessel.transform.position);

                antiGravityConsumeParticleMass = gee.magnitude * mass / GNDriveDefine.ForcePerParticle * percentSecond;

                float consumeGreenParticle = antiGravityConsumeParticleMass;
                consumeGreenParticle = this.part.RequestResource(GNDriveDefine.greenParticleResourceName, consumeGreenParticle);
                float consumeRedParticle = antiGravityConsumeParticleMass - consumeGreenParticle;
                consumeRedParticle = this.part.RequestResource(GNDriveDefine.greenParticleResourceName, consumeRedParticle);
                float remainderParticle = antiGravityConsumeParticleMass - consumeGreenParticle - consumeRedParticle;
                float antiGravityPercent = 1 - (remainderParticle / antiGravityConsumeParticleMass);
                antiGravityConsumeParticleMass = antiGravityConsumeParticleMass - remainderParticle;

                foreach (Part p in this.vessel.parts)
                {
                    if ((p.physicalSignificance == Part.PhysicalSignificance.FULL) && (p.rigidbody != null))
                    {
                        p.rigidbody.AddForce(-gee * p.rigidbody.mass * antiGravityPercent);
                    }
                }
            }

            antiGravityConsumeParticleMassString = antiGravityConsumeParticleMass.ToString("R");

            lastFixedUpdate = time;
        }

        public void engineTransAM()
        {

        }

        public void limiterLift()
        {

        }
    }
}
