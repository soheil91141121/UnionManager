using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.Models.Repositories
{
    public class TradeRepository : IDisposable
    {
        private UnionManager.Models.DomainModels.db_UnionEntities db = null;

        public TradeRepository()
        {
            db = new DomainModels.db_UnionEntities();
        }

        public bool Add(UnionManager.Models.DomainModels.Trade entity, out string message, bool autoSave = true)
        {
            try
            {
                db.Trades.Add(entity);
                if (autoSave)
                {
                    bool result = Convert.ToBoolean(db.SaveChanges());
                    message = "";
                    return result;
                }
                else
                {
                    message = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = ex.GetBaseException().Message;
                return false;
            }
        }

        public bool Update(UnionManager.Models.DomainModels.Trade entity, out string message, bool autoSave = true)
        {
            try
            {
                UnionManager.Models.DomainModels.Trade trade = Find(entity.Id);
                db.Entry(trade).State = System.Data.Entity.EntityState.Detached;
                db.Trades.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                if (autoSave)
                {
                    bool result = Convert.ToBoolean(db.SaveChanges());
                    message = "";
                    return result;
                }
                else
                {
                    message = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = ex.GetBaseException().Message;
                return false;
            }
        }

        public bool Delete(UnionManager.Models.DomainModels.Trade entity, out string message, bool autoSave = true)
        {
            try
            {
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                if (autoSave)
                {
                    bool result = Convert.ToBoolean(db.SaveChanges());
                    message = "";
                    return result;
                }
                else
                {
                    message = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = ex.GetBaseException().Message;
                return false;
            }
        }

        public bool Delete(long id, out string message, bool autoSave = true)
        {
            try
            {
                var entity = db.Trades.Find(id);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                if (autoSave)
                {
                    bool result = Convert.ToBoolean(db.SaveChanges());
                    message = "";
                    return result;
                }
                else
                {
                    message = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = ex.GetBaseException().Message;
                return false;
            }
        }

        public UnionManager.Models.DomainModels.Trade Find(long id)
        {
            try
            {
                return db.Trades.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.Trade> Where(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.Trade, bool>> Predicate)
        {
            try
            {
                return db.Trades.Where(Predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.Trade> Select()
        {
            try
            {
                return db.Trades.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.Trade, TResult>> selector)
        {
            try
            {
                return db.Trades.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public long GetLastIdentity()
        {
            try
            {
                if (db.Trades.Any())
                    return db.Trades.OrderByDescending(p => p.Id).First().Id;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }

        public int Save()
        {
            try
            {
                return db.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.db != null)
                {
                    this.db.Dispose();
                    this.db = null;
                }
            }
        }

        ~TradeRepository()
        {
            Dispose(false);
        }
    }
}