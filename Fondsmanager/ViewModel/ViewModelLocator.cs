﻿using Ninject;
using Fondsmanager.Model;

namespace Fondsmanager.ViewModel
{
    public class ViewModelLocator
    {
        private static StandardKernel kernel;

        static ViewModelLocator()
        {
            kernel = new StandardKernel();
            kernel.Bind<IDataService>().To<WcfDataService>().InSingletonScope();
        }

        public MainViewModel Main
        {
            get
            {
                return kernel.Get<MainViewModel>();
            }
        }

        public LoginViewModel Login
        {
            get
            {
                return kernel.Get<LoginViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            kernel.Dispose();
        }
    }
}