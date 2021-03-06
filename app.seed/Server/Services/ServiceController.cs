﻿using Server.Contracts;
using Models.Model;
using Server.Repository;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Windows;
using System;

namespace Server.Services
{
    class ServiceController : IContractControllers
    {

        ControllerRepository ControllerRepository = new ControllerRepository();

        public List<Controller> GetAllControllers()
        {

            List<Controller> controllers = ControllerRepository.GetAllControllers();
            return new List<Controller>(controllers);
        }

        public void SendControllers(DateTime date, List<Controller> controllers)
        { 
            controllers.ForEach(c => { ControllerRepository.SetTempAndHumidity(c); });
            controllers.ForEach(c => { ControllerRepository.SetHistory(date, c); });
        }
    }
}
