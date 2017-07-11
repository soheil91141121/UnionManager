using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.Models.Repositories
{
    public class ColorRepository : IDisposable
    {
        private UnionManager.Models.DomainModels.db_UnionEntities db = null;

        public ColorRepository()
        {
            db = new DomainModels.db_UnionEntities();
        }

        public bool Add(UnionManager.Models.DomainModels.Color entity, out string message, bool autoSave = true)
        {
            try
            {
                db.Colors.Add(entity);
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

        public bool Update(UnionManager.Models.DomainModels.Color entity, out string message, bool autoSave = true)
        {
            try
            {
                UnionManager.Models.DomainModels.Color color = Find(entity.Id);
                db.Entry(color).State = System.Data.Entity.EntityState.Detached;
                db.Colors.Attach(entity);
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

        public bool Delete(UnionManager.Models.DomainModels.Color entity, out string message, bool autoSave = true)
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
                var entity = db.Colors.Find(id);
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

        public UnionManager.Models.DomainModels.Color Find(long id)
        {
            try
            {
                return db.Colors.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.Color> Where(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.Color, bool>> Predicate)
        {
            try
            {
                return db.Colors.Where(Predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.Color> Select()
        {
            try
            {
                return db.Colors.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.Color, TResult>> selector)
        {
            try
            {
                return db.Colors.Select(selector);
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
                if (db.Colors.Any())
                    return db.Colors.OrderByDescending(p => p.Id).First().Id;
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

        ~ColorRepository()
        {
            Dispose(false);
        }
    }
}