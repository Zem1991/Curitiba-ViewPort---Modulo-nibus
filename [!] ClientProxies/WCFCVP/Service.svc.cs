using Repositorio.DAL;
using Repositorio.DAL.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WCFCVP.DAL;

namespace WCFCVP
{
    // OBSERVAÇÃO: Você pode usar o comando "Renomear" no menu "Refatorar" para alterar o nome da classe "Service1" no arquivo de código, svc e configuração ao mesmo tempo.
    // OBSERVAÇÃO: Para iniciar o cliente de teste do WCF para testar esse serviço, selecione Service1.svc ou Service1.svc.cs no Gerenciador de Soluções e inicie a depuração.
    public class Service1 : IService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        PosicaoGPSRepositorio pgr = new PosicaoGPSRepositorio();
        // GET api/values/5
        /// <summary>
        /// Get a list of positions from the database depending on the parameters
        /// </summary>
        /// <param name="busID">Identity of the bus</param>
        /// <param name="dthms">Date, Time, Hour, Minute and Second that will be used in the SQL query</param>
        /// <returns>A list containing 100 positions - Longitude - Latitude</returns>
        List<PosicaoGPS> IService.GetBusLocalization(string busID, DateTime dateTime)
        {
            List<PosicaoGPS> busPositions = pgr.getTop100Positions(busID, dateTime);
            return busPositions;
        }
    }
}
