using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gundam00
{
    public class GNSystemControl : PartModule
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

        public List<GNDrive> driveList;
        public List<GNEngine> engineList;
        public List<GNRcs> rcsList;
        public List<TwinDrives> twinDrivesList;
        public HashSet<int> gundam00PartSet;

        public GNSystemControl()
        {
            driveList = new List<GNDrive>();
            engineList = new List<GNEngine>();
            rcsList = new List<GNRcs>();
            twinDrivesList = new List<TwinDrives>();
            gundam00PartSet = new HashSet<int>();
        }

        public void resetStatistics()
        {
            driveList.Clear();
            engineList.Clear();
            rcsList.Clear();
            twinDrivesList.Clear();
            gundam00PartSet.Clear();
        }

        public class TwinDrives
        {
            public GNDrive drive1;
            public GNDrive drive2;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            this.reClassifyingAllGundam00();
            this.checkTwinDrives();
        }

        public void reClassifyingAllGundam00()
        {
            this.resetStatistics();

            foreach (Part p in part.vessel.Parts)
            {
                foreach (PartModule m in p.Modules)
                {
                    switch (m.moduleName)
                    {
                        case "GNDrive":
                            {
                                GNDrive drive = (GNDrive)m;
                                driveList.Add(drive);
                                gundam00PartSet.Add(drive.GetInstanceID());
                                break;
                            }

                        case "GNEngine":
                            {
                                GNEngine engine = (GNEngine)m;
                                engineList.Add(engine);
                                gundam00PartSet.Add(engine.GetInstanceID());
                                break;
                            }

                        case "GNRcs":
                            {
                                GNRcs rcs = (GNRcs)m;
                                rcsList.Add(rcs);
                                gundam00PartSet.Add(rcs.GetInstanceID());
                                break;
                            }

                        case "GNTDBlanket":
                            {
                                GNTDBlanket TDBlanket = (GNTDBlanket)m;
                                //rcsList.Add(rcs);
                                gundam00PartSet.Add(TDBlanket.GetInstanceID());
                                break;
                            }
                    }
                }
            }
        }

        public void checkTwinDrives()
        {
            for (int index = twinDrivesList.Count() - 1; index >= 0; index--)
            {
                TwinDrives twinDrives = twinDrivesList[index];
                if (!(gundam00PartSet.Contains(twinDrives.drive1.GetInstanceID()) && gundam00PartSet.Contains(twinDrives.drive2.GetInstanceID())))
                {
                    twinDrivesList.RemoveAt(index);
                }
            }
        }

        public void transAM()
        {

        }

    }
}
