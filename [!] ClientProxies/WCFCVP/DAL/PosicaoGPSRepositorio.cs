using Repositorio.DAL.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFCVP.DAL;

namespace Repositorio.DAL.Repositorios
{
    public class PosicaoGPSRepositorio : Repositorio<PosicaoGPS>
    {
        CVP_PosicaoOnibusEntities db = new CVP_PosicaoOnibusEntities();

        public List<PosicaoGPS> getTop100Positions(string busID, DateTime initialTime)
        {
            var query = from p in db.PosicaoGPS
                        where p.VEIC == busID
                            && (p.DTHR> initialTime)
                        orderby p.DTHR
                        select p;

            if(query.ToList().Count() > 100)
            {
                return query.Take(100).ToList();
            }
            return query.ToList();
        }
    }
}
