using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using WCFCVP.DAL;

namespace Repositorio.DAL.Repositorios.Base
{
    public class Repositorio<TEntity> : IDisposable where TEntity : class
    {
        private CVP_PosicaoOnibusEntities db = new CVP_PosicaoOnibusEntities();

        /// <summary>
        /// Método que busca todos os registros SEM buscar os relacionamentos
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> BuscarTodos()
        {
            return db.Set<TEntity>();
        }

        /// <summary>
        /// Método que busca todos os registros Juntamente com os Relacionamentos
        /// </summary>
        /// <param name="relacionamento">true</param>
        /// <returns></returns>
        public IQueryable<TEntity> BuscarTodos(bool relacionamento)
        {
            db.Configuration.LazyLoadingEnabled = true;
            return db.Set<TEntity>();
        }

        /// <summary>
        /// Método que busca todos os registros com alguma condição, SEM buscar os relacionamentos!
        /// </summary>
        /// <param name="predicate"> condição para filtragem. Ex: x=>x.Status.Equals("A") </param>
        /// <returns></returns>
        public IQueryable<TEntity> BuscarTodosComCondicao(Func<TEntity, bool> predicate)
        {
            return BuscarTodos().Where(predicate).AsQueryable();
        }

        /// <summary>
        /// Método que busca a Contagem de registros
        /// </summary>
        /// <returns></returns>
        public int BuscarCount()
        {
            return BuscarTodos().ToList().Count();
        }

        /// <summary>
        /// Método que busca todos os registros com alguma condição buscando TODOS os relacionamentos!
        /// </summary>
        /// <param name="predicate">condição para filtragem. Ex: x=>x.Status.Equals("A")</param>
        /// <param name="relacionamento">true</param>
        /// <returns></returns>
        public List<TEntity> BuscarTodosComCondicao(Func<TEntity, bool> predicate, bool relacionamento)
        {
            List<TEntity> lista = new List<TEntity>();   
            db.Configuration.LazyLoadingEnabled = true;
            lista = db.Set<TEntity>().Where(predicate).ToList(); 
            return lista;
        }

        /// <summary>
        /// Método que busca todos os registros com alguma condição buscando somente os relacionamentos que deseja!
        /// </summary>
        /// <param name="predicate">condição para filtragem. Ex: x=>x.Status.Equals("A")</param>
        /// <param name="includes"> array de string com os relacionamentos desejáveis. Ex: string[] includes = { "Ex1", "Ex2" };</param> 
        /// <returns></returns>
        public List<TEntity> BuscarTodosComCondicao(Func<TEntity, bool> predicate, string[] includes)
        {
            List<TEntity> lista = new List<TEntity>(); 
            var query = db.Set<TEntity>().AsQueryable();
            foreach (string include in includes)
            {
                query = query.Include(include);
            } 
            lista = query.Where(predicate).ToList(); 
            return lista; 
        }
        /// <summary>
        /// Método que busca um registro atravéz do ID do objeto buscado
        /// </summary>
        /// <param name="key">Identificador do Objeto (ID)</param>
        /// <returns></returns>
        public TEntity BuscarPorID(params object[] key)
        {
            TEntity entity = null;
            db.Configuration.LazyLoadingEnabled = true;
            entity = db.Set<TEntity>().Find(key);
            return entity;
        }

        /// <summary>
        /// Método que atualiza o registro de um objeto
        /// </summary>
        /// <param name="obj">Objeto do tipo Entidade compatível com a estrutura do banco de dados</param>
        public void Atualizar(TEntity obj)
        { 
            db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();
        } 

        public void SalvarTodos()
        { 
            db.SaveChanges();
        }
        /// <summary>
        /// Método que adiciona um novo registro de objeto no banco de dados
        /// </summary>
        /// <param name="obj">Objeto do tipo Entidade compatível com a estrutura do banco de dados</param>
        public void Adicionar(TEntity obj)
        { 
            db.Set<TEntity>().Add(obj);
            db.SaveChanges();
        }
        /// <summary>
        /// Método que exclui um registro de objeto do banco de dados
        /// </summary>
        /// <param name="obj">Objeto do tipo Entidade compatível com a estrutura do banco de dados</param>
        public void Excluir(TEntity obj)
        { 
            db.Set<TEntity>().Remove(obj);
            db.SaveChanges();
        }


        public void Dispose()
        { 
            db.Dispose();
        }
    }
}

