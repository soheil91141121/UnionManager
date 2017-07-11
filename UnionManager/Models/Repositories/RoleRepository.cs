using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.Models.Repositories
{
    public class RoleRepository : IDisposable
    {
        private UnionManager.Models.DomainModels.db_UnionEntities db = null;

        public RoleRepository()
        {
            db = new DomainModels.db_UnionEntities();
        }

        public bool Add(UnionManager.Models.DomainModels.Role entity, bool autoSave = true)
        {
            try
            {
                db.Roles.Add(entity);
                if (autoSave)
                {
                    bool result = Convert.ToBoolean(db.SaveChanges());
                    return result;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(UnionManager.Models.DomainModels.Role entity, bool autoSave = true)
        {
            try
            {
                UnionManager.Models.DomainModels.Role role = Find(entity.Id);
                db.Entry(role).State = System.Data.Entity.EntityState.Detached;
                db.Roles.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                if (autoSave)
                {
                    bool result = Convert.ToBoolean(db.SaveChanges());
                    return result;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(UnionManager.Models.DomainModels.Role entity, bool autoSave = true)
        {
            try
            {
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                if (autoSave)
                {
                    bool result = Convert.ToBoolean(db.SaveChanges());
                    return result;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id, bool autoSave = true)
        {
            try
            {
                var entity = db.Roles.Find(id);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                if (autoSave)
                {
                    bool result = Convert.ToBoolean(db.SaveChanges());
                    return result;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public UnionManager.Models.DomainModels.Role Find(long id)
        {
            try
            {
                return db.Roles.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.Role> Where(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.Role, bool>> Predicate)
        {
            try
            {
                return db.Roles.Where(Predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.Role> Select()
        {
            try
            {
                return db.Roles.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.Role, TResult>> selector)
        {
            try
            {
                return db.Roles.Select(selector);
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
                if (db.Roles.Any())
                    return db.Roles.OrderByDescending(p => p.Id).First().Id;
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

        ~RoleRepository()
        {
            Dispose(false);
        }
    }
}