using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.Models.Repositories
{
    public class TradeGroupRepository : IDisposable
    {
        private UnionManager.Models.DomainModels.db_UnionEntities db = null;

        public TradeGroupRepository()
        {
            db = new DomainModels.db_UnionEntities();
        }

        public bool Add(UnionManager.Models.DomainModels.TradeGroup entity, out string message, bool autoSave = true)
        {
            try
            {
                db.TradeGroups.Add(entity);
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
            catch(Exception ex)
            {
                message = ex.GetBaseException().Message;
                return false;
            }
        }

        public bool Update(UnionManager.Models.DomainModels.TradeGroup entity, out string message, bool autoSave = true)
        {
            try
            {
                UnionManager.Models.DomainModels.TradeGroup tradeGroup = Find(entity.Id);
                db.Entry(tradeGroup).State = System.Data.Entity.EntityState.Detached;
                db.TradeGroups.Attach(entity);
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

        public bool Delete(UnionManager.Models.DomainModels.TradeGroup entity, out string message, bool autoSave = true)
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

        public bool Delete(int id, out string message, bool autoSave = true)
        {
            try
            {
                var entity = db.TradeGroups.Find(id);
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

        public UnionManager.Models.DomainModels.TradeGroup Find(long id)
        {
            try
            {
                return db.TradeGroups.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.TradeGroup> Where(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.TradeGroup, bool>> Predicate)
        {
            try
            {
                return db.TradeGroups.Where(Predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.TradeGroup> Select()
        {
            try
            {
                return db.TradeGroups.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.TradeGroup, TResult>> selector)
        {
            try
            {
                return db.TradeGroups.Select(selector);
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
                if (db.TradeGroups.Any())
                    return db.TradeGroups.OrderByDescending(p => p.Id).First().Id;
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

        ~TradeGroupRepository()
        {
            Dispose(false);
        }
    }
}