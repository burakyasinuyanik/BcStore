﻿using Entities.LogModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    public class LogFilterAttribute:ActionFilterAttribute
    {
        private readonly ILogerService loger;

        public LogFilterAttribute(ILogerService loger)
        {
            this.loger = loger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            loger.LogInfo(Log("OnActionExecuting" , context.RouteData));
          
        }

        private string Log(string modelName, RouteData routeData)
        {
            var logDetails = new LogDetails()
            {
                Action= routeData.Values["action"],
                Controller = routeData.Values["controller"],
                Model=modelName,

            };
            if(routeData.Values.Count>=3)
                logDetails.Id= routeData.Values["id"];
            return logDetails.ToString();
            
        }
    }
}
