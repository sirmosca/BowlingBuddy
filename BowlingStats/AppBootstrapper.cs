using System;
using System.Collections.Generic;
using System.Linq;
using BowlingStats.Interfaces;
using BowlingStats.UI;
using Caliburn.Micro;
using StructureMap;

namespace BowlingStats
{
    public class AppBootstrapper : Bootstrapper<StartViewModel>
    {
//        private IContainer _container;
//
//        protected override void Configure()
//        {
//            _container = new Container(x => x.For<IShell>().Use<StartViewModel>());
//        }
//
//        protected override IEnumerable<object> GetAllInstances(Type service)
//        {
//            return ObjectFactory.GetAllInstances(service).Cast<object>().ToList();
//        }
//
//        protected override object GetInstance(Type service, string key)
//        {
//            return _container.GetInstance(service);
//        }
    }
}