using RegistroProduzione.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroProduzione.Wpf.ViewModels
{
    public class RiepilogoViewModel:ViewModelBase
    {

        private readonly ServizioProduzione servizio;

        public RiepilogoViewModel(ServizioProduzione servizio)
        {
            this.servizio = servizio;
        }


    }
}
