﻿using QTrans.Models;
using QTrans.Models.ViewModel.Common;
using QTrans.Repositories.Repositories;
using System;
using System.Collections.Concurrent;
using System.Timers;

namespace QTrans.Utility.Common
{
    public class InMemoryStorage
    {
        private Timer reloadData;

        /// <summary>
        /// Default milisecods of an Hour
        /// </summary>
        private readonly int DefaultMiliSeconds;

        public ConcurrentDictionary<int, MaterialType> MaterialTypeStorage;

        public ConcurrentDictionary<int, PackageType> PackageTypeStorage;

        public ConcurrentDictionary<int, VehicleType> VehicleTypeStorage;

        public ConcurrentDictionary<int, CountryState> StateStorage;

        public ConcurrentDictionary<int, StateCity> CityStorage;

        public ConcurrentDictionary<int, CityPincode> PincodeStorage;

        private static InMemoryStorage instance;

        private InMemoryStorage()
        {
            this.DefaultMiliSeconds = 60 * 60 * 1000;
            this.MaterialTypeStorage = new ConcurrentDictionary<int, MaterialType>();
            this.PackageTypeStorage = new ConcurrentDictionary<int, PackageType>();
            this.VehicleTypeStorage = new ConcurrentDictionary<int, VehicleType>();

            this.StateStorage = new ConcurrentDictionary<int, CountryState>();
            this.CityStorage = new ConcurrentDictionary<int, StateCity>();
            this.PincodeStorage = new ConcurrentDictionary<int, CityPincode>();
        }

        public void Init()
        {
            this.LoadLocationDetails(0);
        }

        public static InMemoryStorage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InMemoryStorage();
                }
                return instance;
            }
        }


        public long TimeInterval { get; set; }

        public void LoadLocationDetails(int loadSeq)
        {
            switch (loadSeq)
            {
                case 1:
                    LoadState();
                    break;
                case 2:
                    LoadCity();
                    break;
                case 3:
                    LoadPinCode();
                    break;
                default:
                    LoadState();
                    LoadCity();
                    LoadPinCode();
                    break;
            }

        }

        private void LoadState()
        {
            CommonRepository repository = new CommonRepository();
            foreach (var item in repository.GetState().Response)
            {
                if (this.StateStorage.Keys.Contains(item.StateId))
                {
                    this.StateStorage.AddOrUpdate(item.StateId, item, (key, oldValue) => item);
                }
                else
                {
                    this.StateStorage.TryAdd(item.StateId, item);
                }
            }
        }

        private void LoadCity()
        {
            CommonRepository repository = new CommonRepository();
            foreach (var item in repository.GetCity().Response)
            {
                if (this.CityStorage.Keys.Contains(item.CityId))
                {
                    this.CityStorage.AddOrUpdate(item.CityId, item, (key, oldValue) => item);
                }
                else
                {
                    this.CityStorage.TryAdd(item.CityId, item);
                }
            }
        }

        private void LoadPinCode()
        {
            CommonRepository repository = new CommonRepository();
            foreach (var item in repository.GetPincode().Response)
            {
                if (this.PincodeStorage.Keys.Contains(item.PincodeId))
                {
                    this.PincodeStorage.AddOrUpdate(item.PincodeId, item, (key, oldValue) => item);
                }
                else
                {
                    this.PincodeStorage.TryAdd(item.PincodeId, item);
                }
            }
        }

        public void LoadData()
        {
            CommonRepository repository = new CommonRepository();
            foreach (var item in repository.GetMaterialType().Response)
            {
                if (this.MaterialTypeStorage.Keys.Contains(item.materialtypeid))
                {
                    this.MaterialTypeStorage.AddOrUpdate(item.materialtypeid, item, (key, oldValue) => item);
                }
                else
                {
                    this.MaterialTypeStorage.TryAdd(item.materialtypeid, item);
                }

            }

            foreach (var item in repository.GetPackageType().Response)
            {
                if (this.PackageTypeStorage.Keys.Contains(item.packagetypeid))
                {
                    this.PackageTypeStorage.AddOrUpdate(item.packagetypeid, item, (key, oldValue) => item);
                }
                else
                {
                    this.PackageTypeStorage.TryAdd(item.packagetypeid, item);
                }
            }

            foreach (var item in repository.GetVehicleType().Response)
            {
                if (this.VehicleTypeStorage.Keys.Contains(item.vehicletypeid))
                {
                    this.VehicleTypeStorage.AddOrUpdate(item.vehicletypeid, item, (key, oldValue) => item);
                }
                else
                {
                    this.VehicleTypeStorage.TryAdd(item.vehicletypeid, item);
                }
            }

            this.StartTimer();
        }

        /// <summary>
        /// To get default Timer
        /// </summary>
        /// <returns></returns>
        private double GetTimerInterval()
        {
            if (this.TimeInterval <= 0)
            {
                TimeSpan t1 = new TimeSpan(DateTime.Now.AddDays(1).Day, 1, 10, 0);
                TimeSpan t2 = new TimeSpan(DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                return (t1 - t2).TotalMilliseconds;
            }
            else
            {
                return this.TimeInterval * this.DefaultMiliSeconds;
            }
        }

        /// <summary>
        /// To Start the new timer 
        /// </summary>
        private void StartTimer()
        {
            this.reloadData = new Timer();
            this.reloadData.Elapsed += this.objReloadTimer_Elapsed;
            this.reloadData.Interval = this.GetTimerInterval();
            this.reloadData.Start();
        }

        /// <summary>
        /// Timer Elapsed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objReloadTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.DisposeCleanUpTimer();
            this.LoadData();
        }

        /// <summary>
        /// Dispose Clean Up timer
        /// </summary>
        private void DisposeCleanUpTimer()
        {
            if (this.reloadData == null)
            {
                return;
            }

            this.reloadData.Enabled = false;
            this.reloadData.Stop();
            this.reloadData.Elapsed -= this.objReloadTimer_Elapsed;
            this.reloadData.Dispose();
            this.reloadData = null;
        }
    }
}
