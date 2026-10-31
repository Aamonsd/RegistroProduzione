using RegistroProduzione.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroProduzione.Wpf.ViewModels
{
    public class RegistraProduzioneViewModel:ViewModelBase
    {

        private readonly ServizioProduzione servizio;

        public RegistraProduzioneViewModel(ServizioProduzione servizio)
        {
            this.servizio = servizio;
        }
    }
}
